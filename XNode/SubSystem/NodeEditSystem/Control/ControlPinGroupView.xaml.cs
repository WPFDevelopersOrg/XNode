using System.Windows;
using XLib.Node;

namespace XNode.SubSystem.NodeEditSystem.Control
{
    /// <summary>
    /// 控件引脚组视图
    /// </summary>
    public partial class ControlPinGroupView : PinGroupViewBase
    {
        public ControlPinGroupView() => InitializeComponent();

        public ControlPinGroup? Instance { get; set; } = null;

        public override void Init()
        {
            if (Instance == null) return;
            ControlBox.Children.Add((UIElement)Instance.ControlInstance);
        }
    }
}