using System.Windows.Controls;
using System.Windows.Media;

namespace XNode.SubSystem.NodeEditSystem.Control.PropertyBar
{
    public class PropertyBarBase : UserControl
    {
        public string Title { get; set; } = "";

        public virtual void LoadProperty(string value) { }

        protected SolidColorBrush _default = new SolidColorBrush(Color.FromRgb(140, 140, 140));
        protected SolidColorBrush _hovered = new SolidColorBrush(Color.FromRgb(255, 255, 255));
    }
}