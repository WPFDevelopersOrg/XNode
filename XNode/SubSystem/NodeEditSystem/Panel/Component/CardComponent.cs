using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using XLib.Base.UIComponent;
using XLib.Node;
using XNode.SubSystem.NodeEditSystem.Control;

namespace XNode.SubSystem.NodeEditSystem.Panel.Component
{
    /// <summary>
    /// 卡片组件：管理编辑器中的节点控件
    /// </summary>
    public class CardComponent : Component<EditPanel>
    {
        #region 属性

        /// <summary>全部卡片</summary>
        public List<NodeView> AllCard => _cardList;

        /// <summary>选中卡片列表</summary>
        public List<NodeView> SelectedCardList => _selectedCardSet.ToList();

        #endregion

        #region 公开方法

        /// <summary>
        /// 生成节点卡片
        /// </summary>
        public NodeView GenerateNodeCard(NodeBase node)
        {
            // 创建节点卡片
            NodeView card = new NodeView
            {
                NodeColor = Color.FromRgb(node.Color.r, node.Color.g, node.Color.b),
                NodeInstance = node,
                Point = new Point(node.Point.X, node.Point.Y),
            };
            // 设置卡片坐标
            Point center = GetComponent<DrawingComponent>().WorldCenter;
            Canvas.SetLeft(card, center.X + node.Point.X - 12);
            Canvas.SetTop(card, center.Y + node.Point.Y - 1);
            // 初始化节点卡片
            card.Init();
            // 添加视图
            _host.LayerBox_Node.Children.Add(card);
            _cardList.Add(card);
            // 调用已加载
            node.Loaded();
            // 返回节点视图
            return card;
        }

        /// <summary>
        /// 添加选择
        /// </summary>
        public void AddSelect(NodeView view) => _selectedCardSet.Add(view);

        /// <summary>
        /// 添加选择
        /// </summary>
        public void RemoveSelect(NodeView view) => _selectedCardSet.Remove(view);

        /// <summary>
        /// 清空选择
        /// </summary>
        public void ClearSelect() => _selectedCardSet.Clear();

        /// <summary>
        /// 更新节点卡片
        /// </summary>
        public void UpdateNodeCard()
        {
            Point center = GetComponent<DrawingComponent>().WorldCenter;
            foreach (var card in _cardList)
            {
                Canvas.SetLeft(card, center.X + card.Point.X - 12);
                Canvas.SetTop(card, center.Y + card.Point.Y - 1);
            }
        }

        /// <summary>
        /// 将卡片置顶
        /// </summary>
        public void SetTop(NodeView view)
        {
            _host.LayerBox_Node.Children.Remove(view);
            _host.LayerBox_Node.Children.Add(view);
            _cardList.Remove(view);
            _cardList.Add(view);
        }

        /// <summary>
        /// 获取节点卡片
        /// </summary>
        public NodeView GetNodeCard(int nodeID)
        {
            foreach (var card in _cardList)
                if (card.NodeInstance.ID == nodeID) return card;
            throw new Exception("获取节点卡片失败");
        }

        /// <summary>
        /// 删除节点卡片
        /// </summary>
        public void DeleteNodeCard(NodeView card)
        {
            card.Clear();
            _host.LayerBox_Node.Children.Remove(card);
            _cardList.Remove(card);
        }

        #endregion

        #region 字段

        /// <summary>卡片列表</summary>
        private readonly List<NodeView> _cardList = new List<NodeView>();
        /// <summary>选中卡片集</summary>
        private readonly HashSet<NodeView> _selectedCardSet = new HashSet<NodeView>();

        #endregion
    }
}