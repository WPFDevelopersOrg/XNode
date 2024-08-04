using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using XLib.Node;
using XLib.WPF.UI;
using XNode.AppTool;
using XNode.SubSystem.NodeEditSystem.Define;
using XNode.SubSystem.ResourceSystem;
using XNode.SubSystem.WindowSystem;

namespace XNode.SubSystem.NodeEditSystem.Control
{
    public partial class NodeView : MoveableControl
    {
        #region 构造方法

        public NodeView() => InitializeComponent();

        #endregion

        #region 属性

        /// <summary>节点颜色</summary>
        public Color NodeColor
        {
            get => _nodeColor;
            set
            {
                _nodeColor = value;
                Color_Start.Color = Color.FromArgb(255, _nodeColor.R, _nodeColor.G, _nodeColor.B);
                Color_End.Color = Color.FromArgb(0, _nodeColor.R, _nodeColor.G, _nodeColor.B);
                NodeFillColor.Background = new SolidColorBrush(Color.FromArgb(48, _nodeColor.R, _nodeColor.G, _nodeColor.B));
            }
        }

        /// <summary>节点实例</summary>
        public NodeBase NodeInstance { get; set; }

        /// <summary>
        /// 悬停引脚
        /// </summary>
        public PinBase? HoveredPin
        {
            get
            {
                foreach (var item in _pinGroupViewList)
                    if (item.HoveredPin != null) return item.HoveredPin;
                return null;
            }
        }

        #endregion

        #region 事件

        /// <summary>节点背景.鼠标进入</summary>
        public Action<NodeView>? NodeBackMouseEnter { get; set; } = null;

        /// <summary>节点背景.鼠标离开</summary>
        public Action<NodeView>? NodeBackMouseLeave { get; set; } = null;

        /// <summary>引脚组列表变更</summary>
        public Action? PinGroupListChanged { get; set; } = null;

        /// <summary>节点变更</summary>
        public Action? NodeChanged { get; set; } = null;

        #endregion

        #region 公开方法

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            // 设置节点图标与标题
            NodeIcon.Source = ImageResManager.Instance.GetNodeIcon(NodeInstance.Icon);
            Block_Title.Text = NodeInstance.Title;
            // 加载引脚组
            foreach (var pinGroup in NodeInstance.PinGroupList)
            {
                PinGroupViewBase? pinView = CreatePinGroupView(pinGroup);
                if (pinView == null) continue;
                Stack_PinGroupList.Children.Add(pinView);
                _pinGroupViewList.Add(pinView);
                pinView.Init();
            }
            if (_pinGroupViewList.Count > 0)
                _pinGroupViewList[0].Margin = new Thickness(0);
            // 监听节点
            NodeInstance.StateChanged += Node_StateChanged;
            NodeInstance.ExecuteError += Node_ExecuteError;
            NodeInstance.ParaChanged += Node_ParaChanged;
            NodeInstance.PropertyChanged += Node_PropertyChanged;
            NodeInstance.PinGroupListChanged += Node_PinGroupListChanged;
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            NodeInstance.StateChanged -= Node_StateChanged;
            NodeInstance.ExecuteError -= Node_ExecuteError;
            NodeInstance.ParaChanged -= Node_ParaChanged;
            NodeInstance.PropertyChanged -= Node_PropertyChanged;
            NodeInstance.PinGroupListChanged -= Node_PinGroupListChanged;
        }

        /// <summary>
        /// 获取悬停引脚的偏移
        /// </summary>
        public Point GetHoveredPinOffset()
        {
            foreach (var groupView in _pinGroupViewList)
            {
                if (groupView.HoveredPin != null)
                    return groupView.GetHoveredPinOffset();
            }
            return new Point();
        }

        /// <summary>
        /// 获取引脚相对于节点的坐标
        /// </summary>
        public Point GetPinOffset(PinPath path) => _pinGroupViewList[path.GroupIndex].GetPinOffset(this, path.PinIndex);

        /// <summary>
        /// 更新全部引脚图标
        /// </summary>
        public void UpdateAllPinIcon()
        {
            foreach (var groupCard in _pinGroupViewList) groupCard.UpdatePinIcon();
        }

        /// <summary>
        /// 获取可命中矩形区域
        /// </summary>
        public Rect GetHittableRect() => new Rect(Canvas.GetLeft(this) + 11, Canvas.GetTop(this), ActualWidth - 22, ActualHeight);

        #endregion

        #region MoveableControl 方法

        protected override void OnOffsetChanged() => NodeInstance.Point = new NodePoint((int)Point.X, (int)Point.Y);

        #endregion

        #region 控件事件

        private void NodeBack_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            NodeBackMouseEnter?.Invoke(this);
        }

        private void NodeBack_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            NodeBackMouseLeave?.Invoke(this);
        }

        #endregion

        #region 节点事件

        private void Node_StateChanged()
        {
            AppDelegate.Invoke(() =>
            {
                if (NodeInstance.State == NodeState.Enable)
                    Image_Light.Source = ImageResManager.Instance.GetSubSystemImage("NodeEditSystem", "Control/Image", "Light_Green");
                else
                {
                    if (NodeInstance.RunError)
                        Image_Light.Source = ImageResManager.Instance.GetSubSystemImage("NodeEditSystem", "Control/Image", "Light_Red");
                    else
                        Image_Light.Source = ImageResManager.Instance.GetSubSystemImage("NodeEditSystem", "Control/Image", "Light_Black");
                }
            });
        }

        private void Node_ExecuteError(Exception ex)
        {
            AppDelegate.Invoke(() =>
            {
                WM.ShowError("节点执行异常：" + ex.Message);
            });
        }

        private void Node_ParaChanged() => NodeChanged?.Invoke();

        private void Node_PropertyChanged() => NodeChanged?.Invoke();

        private void Node_PinGroupListChanged()
        {
            // 清空当前引脚组
            Stack_PinGroupList.Children.Clear();
            _pinGroupViewList.Clear();
            // 加载引脚组
            foreach (var pinGroup in NodeInstance.PinGroupList)
            {
                PinGroupViewBase? pinView = CreatePinGroupView(pinGroup);
                if (pinView == null) continue;
                Stack_PinGroupList.Children.Add(pinView);
                _pinGroupViewList.Add(pinView);
                pinView.Init();
            }
            if (_pinGroupViewList.Count > 0)
                _pinGroupViewList[0].Margin = new Thickness(0);

            UpdateLayout();
            // 通知引脚组列表变更
            PinGroupListChanged?.Invoke();
            // 通知节点变更
            NodeChanged?.Invoke();
        }

        

        /// <summary>
        /// 查找引脚组
        /// </summary>
        private PinGroupBase? FindPinGroup(string title)
        {
            foreach (var pinGroup in NodeInstance.PinGroupList)
                if (pinGroup.GetTitle() == title) return pinGroup;
            return null;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 创建引脚组视图
        /// </summary>
        private PinGroupViewBase? CreatePinGroupView(PinGroupBase pinGroup)
        {
            switch (pinGroup.GroupType)
            {
                case PinGroupType.Execute:
                    return new ExecutePinGroupView
                    {
                        Instance = (ExecutePinGroup)pinGroup,
                        Margin = new Thickness(0, _pinItemInterval, 0, 0),
                    };
                case PinGroupType.Data:
                    return new DataPinGroupView
                    {
                        Instance = (DataPinGroup)pinGroup,
                        Margin = new Thickness(0, _pinItemInterval, 0, 0),
                    };
                case PinGroupType.Action:
                    return new ActionPinGroupView
                    {
                        Instance = (ActionPinGroup)pinGroup,
                        Margin = new Thickness(0, _pinItemInterval, 0, 0),
                    };
                case PinGroupType.Control:
                    return new ControlPinGroupView
                    {
                        Margin = new Thickness(0, _pinItemInterval, 0, 0),
                        Instance = (ControlPinGroup)pinGroup,
                    };
            }
            return null;
        }

        #endregion

        #region 字段

        private readonly int _pinItemInterval = 5;

        /// <summary>引脚组列表</summary>
        private readonly List<PinGroupViewBase> _pinGroupViewList = new List<PinGroupViewBase>();
        /// <summary>引脚信息列表</summary>
        private readonly List<PinConnectInfo> _connectInfoList = new List<PinConnectInfo>();

        private Color _nodeColor = Colors.White;

        #endregion
    }
}