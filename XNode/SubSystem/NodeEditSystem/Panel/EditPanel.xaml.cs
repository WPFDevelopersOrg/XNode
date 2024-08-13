using System.Windows.Controls;
using XLib.Base;
using XLib.Base.UIComponent;
using XLib.Base.VirtualDisk;
using XLib.Node;
using XNode.SubSystem.EventSystem;
using XNode.SubSystem.NodeEditSystem.Define;
using XNode.SubSystem.NodeEditSystem.Panel.Component;

namespace XNode.SubSystem.NodeEditSystem.Panel
{
    public partial class EditPanel : UserControl, IDropable
    {
        #region 属性

        /// <summary>节点列表</summary>
        public List<NodeBase> NodeList => _nodeComponent.NodeList;

        #endregion

        #region 构造方法

        public EditPanel() => InitializeComponent();

        #endregion

        #region 生命周期

        public void Init()
        {
            // 添加核心组件
            _editerComponent = _componentBox.AddComponent<EditerComponent>(this, "编辑器组件");
            // 添加功能组件
            _drawingComponent = _componentBox.AddComponent<DrawingComponent>(this, "绘图组件");
            _nodeComponent = _componentBox.AddComponent<NodeComponent>(this, "节点组件");
            _cardComponent = _componentBox.AddComponent<CardComponent>(this, "卡片组件");
            _interactionComponent = _componentBox.AddComponent<InteractionComponent>(this, "交互组件");
            // 注册核心组件
            _componentBox.RegisterCoreComponent(_editerComponent);
            // 注册功能组件
            _editerComponent.AddComponent(_drawingComponent);
            _editerComponent.AddComponent(_nodeComponent);
            _editerComponent.AddComponent(_cardComponent);
            _editerComponent.AddComponent(_interactionComponent);
            // 初始化组件
            _componentBox.Init();
            // 启用编辑
            _editerComponent.ReqEnable();
            // 监听系统事件
            EM.Instance.Add(EventType.Project_Loaded, Project_Loaded);
        }

        #endregion

        #region IDropable 方法

        public void OnDrag(List<ITreeItem> fileList) { }

        public void OnDrop(List<ITreeItem> fileList)
        {
            _interactionComponent.HandleDrop(fileList);
        }

        public bool CanDrop(List<ITreeItem> fileList) => fileList[0] is File file && file.Extension == "nt";

        #endregion

        #region 公开方法

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset() => _componentBox.Reset();

        /// <summary>
        /// 加载节点
        /// </summary>
        public void LoadNode(NodeBase node) => _nodeComponent.LoadNode(node);

        /// <summary>
        /// 查找引脚
        /// </summary>
        public PinBase? FindPin(PinPath path)
        {
            foreach (var node in _nodeComponent.NodeList)
                if (node.ID == path.NodeID) return node.FindPin(path.NodeVersion, path.GroupIndex, path.PinIndex);
            return null;
        }

        #endregion

        #region 系统事件

        private void Project_Loaded()
        {
            // 更新引脚图标、连接信息
            _interactionComponent.UpdateAllPinIcon();
            _interactionComponent.UpdateConnectInfo();
            // 更新连接线
            _drawingComponent.UpdateConnectLine();
        }

        #endregion

        #region 字段

        /// <summary>组件箱</summary>
        private readonly ComponentBox<EditPanel> _componentBox = new ComponentBox<EditPanel>();

        /// <summary>编辑器组件</summary>
        private EditerComponent _editerComponent;

        /// <summary>绘图组件</summary>
        private DrawingComponent _drawingComponent;
        /// <summary>节点组件</summary>
        private NodeComponent _nodeComponent;
        /// <summary>卡片组件</summary>
        private CardComponent _cardComponent;
        /// <summary>交互组件</summary>
        private InteractionComponent _interactionComponent;

        #endregion
    }
}