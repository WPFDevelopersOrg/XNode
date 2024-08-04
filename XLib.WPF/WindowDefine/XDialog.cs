using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace XLib.WPF.WindowDefine
{
    public class XDialog : XWindow
    {
        protected override void AddWindowControl()
        {
            // 关闭按钮
            if (GetTemplateChild("CloseButton") is Button close)
                close.Click += (_, _) => OnCloseClick();
            // 查找背景网格
            if (FindName("BackGrid") is Grid grid)
            {
                // 设置背景色，确保能被鼠标命中
                grid.Background ??= Brushes.Transparent;
                grid.MouseLeftButtonDown += BackGrid_MouseDown;
            }
        }

        /// <summary>
        /// 关闭.单击
        /// </summary>
        protected virtual void OnCloseClick() => Close();

        private void BackGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }
    }
}