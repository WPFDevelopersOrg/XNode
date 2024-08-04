using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace XLib.WPFControl
{
    public partial class ToolBar : UserControl
    {
        #region 属性、事件

        public Style? ToolStyle { get; set; } = null;

        public int ToolWidth { get; set; } = 30;

        public int ToolHeight { get; set; } = 30;

        public int IconSize { get; set; } = 16;

        public event Action<string>? ToolClick;

        #endregion

        #region 构造方法

        public ToolBar() => InitializeComponent();

        #endregion

        #region 公开方法

        /// <summary>
        /// 添加工具
        /// </summary>
        /// <param name="icon">图标</param>
        /// <param name="name">控件名</param>
        /// <param name="title">标题</param>
        public Button AddTool(ImageSource icon, string name, string title)
        {
            // 创建按钮
            Button tool = new Button
            {
                Width = ToolWidth,
                Height = ToolHeight,
                Content = new Image { Source = icon, Width = IconSize, Height = IconSize },
                Name = name,
                ToolTip = title
            };
            if (ToolStyle != null) tool.Style = ToolStyle;
            // 添加按钮
            Stack_Tool.Children.Add(tool);
            _toolDict.Add(name, tool);
            // 添加事件
            tool.Click += Tool_Click;
            return tool;
        }

        /// <summary>
        /// 添加分割线
        /// </summary>
        public void AddSplit() => Stack_Tool.Children.Add(new ToolSplitBar());

        public void AddSplit(Thickness margin) => Stack_Tool.Children.Add(new ToolSplitBar { CustomMargin = margin });

        /// <summary>
        /// 禁用工具
        /// </summary>
        public void DisableTool(string name)
        {
            if (_toolDict.ContainsKey(name)) _toolDict[name].IsEnabled = false;
        }

        /// <summary>
        /// 禁用全部工具
        /// </summary>
        public void DisableAllTool()
        {
            foreach (var pair in _toolDict) pair.Value.IsEnabled = false;
        }

        /// <summary>
        /// 启用工具
        /// </summary>
        public void EnableTool(string name)
        {
            if (_toolDict.ContainsKey(name)) _toolDict[name].IsEnabled = true;
        }

        /// <summary>
        /// 启用全部工具
        /// </summary>
        public void EnableAllTool()
        {
            foreach (var pair in _toolDict) pair.Value.IsEnabled = true;
        }

        /// <summary>
        /// 设置可见性
        /// </summary>
        public void SetVisibility(string name, Visibility visibility)
        {
            if (_toolDict.ContainsKey(name)) _toolDict[name].Visibility = visibility;
        }

        /// <summary>
        /// 更新图标
        /// </summary>
        public void UpdateIcon(string toolName, ImageSource icon)
        {
            _toolDict[toolName].Content = new Image { Source = icon, Width = IconSize, Height = IconSize };
        }

        #endregion

        #region 控件事件

        private void Tool_Click(object sender, RoutedEventArgs e) => ToolClick?.Invoke(((Button)sender).Name);

        #endregion

        #region 字段

        private readonly Dictionary<string, Button> _toolDict = new Dictionary<string, Button>();

        #endregion
    }
}