using System.Windows;
using System.Windows.Controls;
using XLib.Node;
using XNode.SubSystem.NodeEditSystem.Define;
using XNode.SubSystem.ProjectSystem;
using XNode.SubSystem.WindowSystem;

namespace XNode
{
    public partial class CoreEditer : UserControl
    {
        #region 属性

        /// <summary>节点列表</summary>
        public List<NodeBase> NodeList => Panel_NodeEditer.NodeList;

        #endregion

        #region 构造方法

        public CoreEditer()
        {
            InitializeComponent();
            Loaded += CoreEditer_Loaded;
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 重置编辑器
        /// </summary>
        public void ResetEditer() => Panel_NodeEditer.Reset();

        /// <summary>
        /// 加载节点
        /// </summary>
        public void LoadNode(NodeBase node) => Panel_NodeEditer.LoadNode(node);

        /// <summary>
        /// 查找引脚
        /// </summary>
        public PinBase? FindPin(PinPath path) => Panel_NodeEditer.FindPin(path);

        #endregion

        #region 控件事件

        private void CoreEditer_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
            ProjectManager.Instance.NewProject();
            ProjectManager.Instance.Saved = true;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化核心编辑器
        /// </summary>
        private void Init()
        {
            // 初始化编辑器面板
            Panel_NodeEditer.Init();
            // 初始化节点库面板
            Panel_NodeLib.Init();
        }

        #endregion

        #region 字段



        #endregion
    }
}