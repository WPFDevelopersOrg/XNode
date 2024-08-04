using System.Windows.Controls;
using System.Windows.Input;

namespace XNode.SubSystem.NodeEditSystem.Control.PropertyBar
{
    public partial class TextInput : PropertyBarBase
    {
        public TextInput()
        {
            InitializeComponent();
            // Input_Value.TextChanged += OnTextChanged;
            Input_Value.KeyDown += Input_Value_KeyDown;
        }

        public string Text { get; set; } = "";

        public event Action<string> TextChanged;

        /// <summary>
        /// 加载属性
        /// </summary>
        public override void LoadProperty(string text)
        {
            Block_Title.Text = Title;
            Input_Value.Text = text;
        }

        private void MainGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            Block_Title.Foreground = _hovered;
        }

        private void MainGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            Block_Title.Foreground = _default;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(Input_Value.Text);
            Text = Input_Value.Text;
        }

        private void Input_Value_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                TextChanged?.Invoke(Input_Value.Text);
                Text = Input_Value.Text;
            }
        }
    }
}