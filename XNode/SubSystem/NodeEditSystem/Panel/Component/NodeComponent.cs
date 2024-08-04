using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _nodeIDBox.RecycleID(node.ID);
            _nodeDict.Remove(node.ID);
            _nodeList.Remove(node);
        }

        /// <summary>
        /// 获取连接信息
        /// </summary>
        public Dictionary<PinBase, HashSet<PinBase>> GetConnectInfo()
        {
            // 连接信息
            Dictionary<PinBase, HashSet<PinBase>> connectInfo = new Dictionary<PinBase, HashSet<PinBase>>();
            // 遍历节点
            foreach (var node in _nodeList)
            {
                // 遍历全部引脚
                foreach (var pin in node.GetAllPin())
                {
                    // 忽略输入引脚与空输出引脚
                    if (pin.Flow == PinFlow.Input || pin.TargetList.Count == 0) continue;
                    // 添加连接源
                    if (!connectInfo.ContainsKey(pin)) connectInfo.Add(pin, new HashSet<PinBase>());
                    // 添加连接目标
                    foreach (var target in pin.TargetList) connectInfo[pin].Add(target);
                }
            }
            // 返回连接信息
            return connectInfo;
        }

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