using System.Windows;
using System.Windows.Controls;

namespace XNode.SubSystem.NodeEditSystem.Control.PropertyBar
{
    public partial class CustomListItem : UserControl
    {
        public CustomListItem()
        {
            InitializeComponent();
        }

        public string ItemValue
        {
            get => Input_Value.Text;
            set => Input_Value.Text = value;
        }

        public Action<CustomListItem>? Remove_Click { get; set; } = null;

        public Action<CustomListItem>? Value_Changed { get; set; } = null;

        private void Input_Value_TextChanged(object sender, TextChangedEventArgs e) => Value_Changed?.Invoke(this);

        private void Tool_Remove_Click(object sender, RoutedEventArgs e) => Remove_Click?.Invoke(this);
    }
}