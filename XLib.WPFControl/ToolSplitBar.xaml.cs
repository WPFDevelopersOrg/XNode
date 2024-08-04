using System.Windows;
using System.Windows.Controls;

namespace XLib.WPFControl
{
    public partial class ToolSplitBar : UserControl
    {
        public Thickness CustomMargin { get; set; } = new Thickness(5);

        public ToolSplitBar() => InitializeComponent();

        private void UserControl_Loaded(object sender, RoutedEventArgs e) => MainGrid.Margin = CustomMargin;
    }
}