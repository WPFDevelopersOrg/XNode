using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using XLib.Base;

namespace XLib.WPF.WindowDefine
{
    public class XMainWindow : XWindow
    {
        #region 属性

        /// <summary>
        /// 窗口边框宽度
        /// </summary>
        public double WindowBorderWdith
        {
            get
            {
                // 获取系统缩放比例
                Graphics currentGraphics = Graphics.FromHwnd(new WindowInteropHelper(this).Handle);
                double dpixRatio = currentGraphics.DpiX / 96;
                // 调整尺寸的边框宽度
                int resizeBorderWidth = OSTool.GetSystemMetrics(OSTool.SystemMetric.SM_CXFRAME);
                // 窗口填充宽度
                int addedBorderWidth = OSTool.GetSystemMetrics(OSTool.SystemMetric.SM_CXPADDEDBORDER);
                return (resizeBorderWidth + addedBorderWidth) / dpixRatio;
            }
        }

        #endregion

        #region 构造方法

        public XMainWindow() => Closed += XMainWindow_Closed;

        #endregion

        #region XWindow 方法

        protected override void AddWindowControl()
        {
            // 图标区域
            if (GetTemplateChild("Grid_Icon") is Grid gridIcon)
                gridIcon.MouseDown += TitleBar_MouseDown;
            // 标题区域
            if (GetTemplateChild("Grid_Title") is Grid gridTitle)
                gridTitle.MouseDown += TitleBar_MouseDown;
            // 最小化按钮
            if (GetTemplateChild("MinButton") is Button min)
                min.Click += delegate { WindowState = WindowState.Minimized; };
            // 最大化按钮
            if (GetTemplateChild("MaxButton") is Button max)
                max.Click += delegate
                {
                    // 切换最大化与还原
                    if (WindowState != WindowState.Maximized) WindowState = WindowState.Maximized;
                    else if (WindowState == WindowState.Maximized) WindowState = WindowState.Normal;
                };
            // 关闭按钮
            if (GetTemplateChild("CloseButton") is Button close)
                close.Click += delegate { CloseButton_Click(); };
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 准备关闭
        /// </summary>
        protected virtual bool ReadyClose() => true;

        #endregion

        #region 窗口事件

        private void XMainWindow_Closed(object? sender, EventArgs e) => Application.Current.Shutdown();

        #endregion

        #region 控件事件

        /// <summary>
        /// 标题栏.鼠标按下
        /// </summary>
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // 左键按下，则启用拖动
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        /// <summary>
        /// 关闭按钮.单击
        /// </summary>
        private void CloseButton_Click()
        {
            bool needClose = true;
            try
            {
                if (!ReadyClose())
                {
                    needClose = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("退出时发生异常：" + ex.Message, ex);
            }
            finally
            {
                if (needClose) Close();
            }
        }

        #endregion
    }
}