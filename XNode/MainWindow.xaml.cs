using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using XLib.Base;
using XLib.WPF.WindowDefine;
using XLib.WPFControl;
using XNode.SubSystem.EventSystem;
using XNode.SubSystem.ResourceSystem;
using XNode.SubSystem.WindowSystem;

namespace XNode
{
    public partial class MainWindow : XMainWindow
    {
        #region 构造方法

        public MainWindow() => InitializeComponent();

        #endregion

        #region XMainWindow 方法

        protected override void XWindowLoaded()
        {
            // 设置主窗口实例
            WM.Main = this;

            // 初始化工具栏
            InitToolBar();
            // 加载核心编辑器
            LoadCoreEditer();
        }

        #endregion

        #region 窗口事件

        private void XMainWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            EM.Instance.Invoke(EventType.KeyDown, e.Key.ToString());
        }

        private void XMainWindow_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            EM.Instance.Invoke(EventType.KeyUp, e.Key.ToString());
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化工具栏
        /// </summary>
        private void InitToolBar()
        {
            // 获取工具栏
            if (GetTemplateChild("TopToolBar") is ToolBar bar)
            {
                _toolBar = bar;
                bar.ToolStyle = (Style)FindResource("ToolBarButton");
                // 填充工具按钮
                bar.AddSplit(new Thickness(0, 5, 5, 5));
                bar.AddTool(GetToolIcon("NewFile"), "NewProject", "新建项目");
                bar.AddTool(GetToolIcon("OpenFile"), "OpenProject", "打开项目");
                bar.AddTool(GetToolIcon("Save"), "SaveProject", "保存项目");
                bar.AddTool(GetToolIcon("SaveAs"), "SaveAs", "另存为项目");
                bar.AddSplit();
                bar.AddTool(GetToolIcon("Undo"), "Undo", "撤销");
                bar.AddTool(GetToolIcon("Redo"), "Redo", "重做");
                bar.AddSplit();
                bar.AddTool(GetToolIcon("Console"), "Console", "控制台");
                bar.AddTool(GetToolIcon("ClearConsole"), "ClearConsole", "清空控制台");
                // 监听工具栏
                bar.ToolClick += ToolBar_ToolClick;
                // 禁用工具栏
                _toolBar.DisableAllTool();
                _toolBar.EnableTool("Console");
                _toolBar.EnableTool("ClearConsole");
            }
        }

        /// <summary>
        /// 获取工具图标
        /// </summary>
        private ImageSource GetToolIcon(string name) => ImageResManager.Instance.GetAssetsImage($"Icon16/{name}.png");

        /// <summary>
        /// 工具栏.单击工具
        /// </summary>
        private void ToolBar_ToolClick(string name)
        {
            switch (name)
            {
                // 新建项目
                case "NewProject":
                    break;
                // 打开项目
                case "OpenProject":
                    break;
                // 保存项目
                case "SaveProject":
                    break;
                // 另存为项目
                case "SaveAs":
                    break;
                // 撤销
                case "Undo":
                    break;
                // 重做
                case "Redo":
                    break;
                // 控制台
                case "Console":
                    if (_consoleOpened)
                    {
                        OSTool.CloseConsole();
                        _consoleOpened = false;
                    }
                    else
                    {
                        OSTool.OpenConsole();
                        _consoleOpened = true;
                    }
                    break;
                // 清空控制台
                case "ClearConsole":
                    Console.Clear();
                    break;
            }
        }

        /// <summary>
        /// 加载核心编辑器
        /// </summary>
        private void LoadCoreEditer()
        {
            _coreEditer = new CoreEditer { Margin = new Thickness(0, 2, 0, 0) };
            MainGrid.Children.Add(_coreEditer);
        }

        #endregion

        #region 字段

        /// <summary>工具栏</summary>
        private ToolBar? _toolBar = null;

        /// <summary>核心编辑器</summary>
        private CoreEditer? _coreEditer = null;

        /// <summary>控制台已打开</summary>
        private bool _consoleOpened = false;

        #endregion
    }
}