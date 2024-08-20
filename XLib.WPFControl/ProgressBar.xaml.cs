using System.Windows;
using System.Windows.Controls;
using XLib.Base.Ex;

namespace XLib.WPFControl
{
    public partial class ProgressBar : UserControl
    {
        /// <summary>进度</summary>
        public double Progress
        {
            get => _progress;
            set
            {
                _progress = value.Limit(0, 1);
                if (double.IsNaN(_progress)) _progress = 0;
                UpdateProgress();
            }
        }

        public ProgressBar()
        {
            InitializeComponent();
            SizeChanged += ProgressBar_SizeChanged;
        }

        private void ProgressBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged) UpdateProgress();
        }

        /// <summary>
        /// 更新进度
        /// </summary>
        private void UpdateProgress()
        {
            Progress_Inner.Width = MainGrid.ActualWidth;
            Progress_Box.Width = (int)(MainGrid.ActualWidth * _progress);
        }

        private double _progress = 0;
    }
}