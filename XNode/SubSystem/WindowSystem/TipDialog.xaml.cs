using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using XLib.WPF.WindowDefine;

namespace XNode.SubSystem.WindowSystem
{
    public partial class TipDialog : XDialog
    {
        public TipDialog()
        {
            InitializeComponent();
            KeyDown += TipDialog_KeyDown;
            OK.Click += OK_Click;
        }

        /// <summary>提示级别</summary>
        public TipLevel Level { get; set; } = TipLevel.Info;

        /// <summary>提示信息</summary>
        public string Message { get; set; } = "";

        protected override void XWindowLoaded()
        {
            // 获取标题栏
            Grid? titleBar = GetTemplateChild("TitleBar") as Grid;
            titleBar.MouseLeftButtonDown += TitleBar_MouseLeftButtonDown;
            // 获取边框对象
            Border? titleBorder = GetTemplateChild("TitleBorder") as Border;
            Border? clientBorder = GetTemplateChild("ClientBorder") as Border;
            // 设置提示图标
            Icon_Info.Visibility = Visibility.Collapsed;
            Icon_Warning.Visibility = Visibility.Collapsed;
            Icon_Error.Visibility = Visibility.Collapsed;
            switch (Level)
            {
                case TipLevel.Info:
                    Icon_Info.Visibility = Visibility.Visible;
                    _currentBrush = _info;
                    break;
                case TipLevel.Break:
                    Title = "中断";
                    Icon_Break.Visibility = Visibility.Visible;
                    _currentBrush = _break;
                    break;
                case TipLevel.Warning:
                    Title = "警告";
                    Icon_Warning.Visibility = Visibility.Visible;
                    _currentBrush = _warning;
                    break;
                case TipLevel.Error:
                    Title = "错误";
                    Icon_Error.Visibility = Visibility.Visible;
                    _currentBrush = _error;
                    break;
            }
            // 设置边框颜色
            titleBorder.BorderBrush = _currentBrush;
            clientBorder.BorderBrush = _currentBrush;
            // 设置提示文本
            TipTextBlock.Inlines.Add(new Run(Message) { Foreground = _tip });
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void TipDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #region 字段

        private readonly SolidColorBrush _tip = new SolidColorBrush(Color.FromRgb(160, 160, 160));

        private readonly SolidColorBrush _info = new SolidColorBrush(Color.FromRgb(119, 180, 255));
        private readonly SolidColorBrush _break = new SolidColorBrush(Color.FromRgb(249, 202, 124));
        private readonly SolidColorBrush _warning = new SolidColorBrush(Color.FromRgb(255, 180, 0));
        private readonly SolidColorBrush _error = new SolidColorBrush(Color.FromRgb(224, 67, 67));

        private SolidColorBrush _currentBrush;

        #endregion
    }
}