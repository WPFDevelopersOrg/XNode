using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XLib.Node;
using XNode.SubSystem.ResourceSystem;

namespace XNode.SubSystem.NodeEditSystem.Control
{
    public partial class ActionPinGroupView : PinGroupViewBase
    {
        public ActionPinGroupView() => InitializeComponent();

        public ActionPinGroup? Instance { get; set; } = null;

        public override void Init()
        {
            if (Instance == null) return;
            Title_Pin.Text = Instance.ActionName;
            Instance.ActionNameChanged = (name) => Title_Pin.Text = name;
            Icon_Pin.Source = PinIconManager.Instance.ExecutePin_Null;

            PinArea.MouseEnter += PinArea_MouseEnter;
            PinArea.MouseLeave += PinArea_MouseLeave;
        }

        public override Grid GetPinArea()
        {
            if (HoveredPin != null) return PinArea;
            throw new Exception("无命中引脚");
        }

        private void PinArea_MouseEnter(object sender, MouseEventArgs e)
        {
            HoveredPin = Instance.OutputPin;
        }

        private void PinArea_MouseLeave(object sender, MouseEventArgs e)
        {
            HoveredPin = null;
        }

        public override Point GetPinOffset(NodeView card, int pinIndex) => PinArea.TranslatePoint(new Point(14, 8), card);

        public override void UpdatePinIcon()
        {
            if (Instance.OutputPin.TargetList.Count == 0) Icon_Pin.Source = PinIconManager.Instance.ExecutePin_Null;
            else Icon_Pin.Source = PinIconManager.Instance.ExecutePin;
        }
    }
}