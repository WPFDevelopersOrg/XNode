using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace XLib.WPF.WindowDefine
{
    public abstract class XWindow : Window
    {
        #region 属性

        /// <summary>自定义图标</summary>
        public string CustomIcon { get; set; } = "";

        #endregion

        #region 构造方法

        public XWindow()

        {
            Loaded += XWindow_Loaded;
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 添加窗口控制
        /// </summary>
        protected abstract void AddWindowControl();

        /// <summary>
        /// 窗口已加载
        /// </summary>
        protected virtual void XWindowLoaded() { }

        #endregion

        #region 窗口事件

        private void XWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 添加窗口控制
            AddWindowControl();
            // 设置左上角图标
            if (CustomIcon != "")
            {
                if (GetTemplateChild("WindowIcon") is Image icon)
                {
                    var uri = new Uri($"pack://application:,,,/Assets/{CustomIcon}.png");
                    var bitmap = new BitmapImage(uri);
                    icon.Source = bitmap;
                }
            }
            // 调用已加载
            XWindowLoaded();
        }

        #endregion
    }
}