using System.Windows;
using XLib.Base.ID;
using XLib.Base.UIComponent;
using XLib.Node;
using XNode.SubSystem.NodeEditSystem.Control;

namespace XNode.SubSystem.NodeEditSystem.Panel.Component
{
    /// <summary>
    /// 节点组件：管理编辑器中的节点实例
    /// </summary>
    public class NodeComponent : Component<EditPanel>
    {
        #region 属性

        /// <summary>节点列表</summary>
        public List<NodeBase> NodeList => _nodeList;

        #endregion

        #region 生命周期

        protected override void Reset()
        {
            _nodeIDBox.Reset();
            _nodeDict.Clear();
            _nodeList.Clear();
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 放下节点
        /// </summary>
        public NodeView? DropNode(int fileID, NodeType nodeType, Point screenPoint)
        {
            // 获取世界坐标
            var worldPoint = GetComponent<DrawingComponent>().ScreenToWorld(screenPoint);
            // 创建节点实例
            NodeBase nodeInstance = nodeType.NewInstance();
            nodeInstance.PinBreaked += Node_PinBreaked;
            nodeInstance.TypeID = fileID;
            // 设置节点编号、坐标
            nodeInstance.ID = _nodeIDBox.TakeID();
            nodeInstance.Point = new NodePoint((int)worldPoint.X, (int)worldPoint.Y);
            // 添加节点引用
            _nodeDict.Add(nodeInstance.ID, nodeInstance);
            _nodeList.Add(nodeInstance);
            // 生成节点卡片
            NodeView card = GetComponent<CardComponent>().GenerateNodeCard(nodeInstance);
            // 启动节点
            nodeInstance.Start();
            return card;
        }

        /// <summary>
        /// 加载节点
        /// </summary>
        public void LoadNode(NodeBase node)
        {
            node.PinBreaked += Node_PinBreaked;
            _nodeIDBox.UseID(node.ID);
            _nodeDict.Add(node.ID, node);
            _nodeList.Add(node);
            NodeView card = GetComponent<CardComponent>().GenerateNodeCard(node);
            card.UpdateLayout();
            GetComponent<InteractionComponent>().ListenNodeCard(card);
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        public void DeleteNode(NodeBase node)
        {
            node.Clear();
            node.BreakAllPin();
            node.PinBreaked -= Node_PinBreaked;
            _nodeIDBox.RecycleID(node.ID);
            _nodeDict.Remove(node.ID);
            _nodeList.Remove(node);
        }

        /// <summary>
        /// 生成连接线
        /// </summary>
        public void GenerateConnectLine()
        {
            // 遍历节点
            foreach (var node in _nodeList)
            {
                // 遍历全部引脚
                foreach (var pin in node.GetAllPin())
                {
                    // 忽略输入引脚与空输出引脚
                    if (pin.Flow == PinFlow.Input || pin.TargetList.Count == 0) continue;
                    // 添加连接线
                    foreach (var target in pin.TargetList)
                        GetComponent<DrawingComponent>().AddConnectLine(pin, target);
                }
            }
        }

        #endregion

        #region 节点事件

        private void Node_PinBreaked(PinBase start, PinBase end) => GetComponent<DrawingComponent>().RemoveConnectLine(start, end);

        #endregion

        #region 字段

        /// <summary>节点编号箱</summary>
        private readonly IDBox _nodeIDBox = new IDBox();

        /// <summary>节点字典</summary>
        private readonly Dictionary<int, NodeBase> _nodeDict = new Dictionary<int, NodeBase>();
        /// <summary>节点列表</summary>
        private List<NodeBase> _nodeList = new List<NodeBase>();

        #endregion
    }
}