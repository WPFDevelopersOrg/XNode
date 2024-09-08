using System.Windows;
using System.Windows.Media;
using XLib.Base;
using XLib.WPF.WindowDefine;
using XLib.WPFControl;
using XNode.AppTool;
using XNode.SubSystem.CacheSystem;
using XNode.SubSystem.EventSystem;
using XNode.SubSystem.ProjectSystem;
using XNode.SubSystem.ResourceSystem;
using XNode.SubSystem.WindowSystem;

namespace XNode
{
    public partial class MainWindow : XMainWindow
    {
        #region 属性

        /// <summary>核心编辑器实例</summary>
        public CoreEditer Editer
        {
            get
            {
                if (_coreEditer == null) throw new Exception("核心编辑器为空");
                return _coreEditer;
            }
        }

        #endregion

        #region 构造方法

        public MainWindow() => InitializeComponent();

        #endregion

        #region XMainWindow 方法

        protected override void XWindowLoaded()
        {
            // 恢复窗口状态并监听窗口状态
            RecoverWindowState();
            ListenWindowState();

            // 设置主窗口实例
            WM.Main = this;

            // 初始化工具栏
            InitToolBar();
            // 加载核心编辑器
            LoadCoreEditer();

            // 监听系统事件
            EM.Instance.Add(EventType.Project_Changed, UpdateTitle);
        }

        protected override bool ReadyClose()
        {
            // 项目未保存
            if (!ProjectManager.Instance.Saved)
            {
                bool? result = WM.ShowAsk("当前项目未保存，是否保存？");
                // 保存
                if (result == true)
                {
                    bool saved = ProjectManager.Instance.SaveProject();
                    // 确定保存，但未执行，则取消操作
                    if (!saved) return false;
                }
                // 取消操作
                else if (result == null) return false;
            }

            // 关闭项目
            ProjectManager.Instance.CloseProject();

            return true;
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
        /// 恢复窗口状态
        /// </summary>
        private void RecoverWindowState()
        {
            WindowState = CacheManager.Instance.Cache.MainWindow.State;
            Width = CacheManager.Instance.Cache.MainWindow.Width;
            Height = CacheManager.Instance.Cache.MainWindow.Height;
            // 居中窗口
            Left = (SystemParameters.WorkArea.Width - Width) / 2;
            Top = (SystemParameters.WorkArea.Height - Height) / 2;
        }

        /// <summary>
        /// 监听窗口状态
        /// </summary>
        private void ListenWindowState()
        {
            StateChanged += (s, e) =>
            {
                if (WindowState is WindowState.Normal or WindowState.Maximized)
                {
                    CacheManager.Instance.Cache.MainWindow.State = WindowState;
                    CacheManager.Instance.UpdateCache();
                }
            };
            SizeChanged += (s, e) =>
            {
                if (WindowState == WindowState.Maximized) return;
                CacheManager.Instance.Cache.MainWindow.Width = (int)Width;
                CacheManager.Instance.Cache.MainWindow.Height = (int)Height;
                CacheManager.Instance.UpdateCache();
            };
        }

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
                // _toolBar.DisableAllTool();
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
                    ProjectManager.Instance.NewProject();
                    UpdateTitle();
                    break;
                // 打开项目
                case "OpenProject":
                    ProjectManager.Instance.OpenProject();
                    UpdateTitle();
                    break;
                // 保存项目
                case "SaveProject":
                    ProjectManager.Instance.SaveProject();
                    UpdateTitle();
                    break;
                // 另存为项目
                case "SaveAs":
                    ProjectManager.Instance.SaveAsProject();
                    UpdateTitle();
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

        /// <summary>
        /// 更新标题
        /// </summary>
        private void UpdateTitle()
        {
            if (ProjectManager.Instance.CurrentProject != null)
            {
                Title = ProjectManager.Instance.CurrentProject.ProjectName;
                if (!ProjectManager.Instance.Saved) Title += "*";
                Title += " - " + AppDelegate.AppTitle;
            }
            else Title = AppDelegate.AppTitle;
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