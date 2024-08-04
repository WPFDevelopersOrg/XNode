using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XLib.Node;
using XNode.SubSystem.ResourceSystem;

namespace XNode.SubSystem.NodeEditSystem.Control
{
    public partial class ExecutePinGroupView : PinGroupViewBase
    {
        public ExecutePinGroupView() => InitializeComponent();

        public ExecutePinGroup? Instance { get; set; } = null;

        public override void Init()
        {
            if (Instance == null) return;
            Icon_LeftPin.Source = PinIconManager.Instance.ExecutePin_Null;
            Block_ExecuteDesc.Text = Instance.ExecuteDesc;
            Icon_RightPin.Source = PinIconManager.Instance.ExecutePin_Null;

            LeftPinArea.MouseEnter += LeftPinArea_MouseEnter;
            RightPinArea.MouseEnter += RightPinArea_MouseEnter;
            LeftPinArea.MouseLeave += PinArea_MouseLeave;
            RightPinArea.MouseLeave += PinArea_MouseLeave;
        }

        public override Grid GetPinArea()
        {
            if (HoveredPin == Instance.InputPin) return LeftPinArea;
            if (HoveredPin == Instance.OutputPin) return RightPinArea;
            throw new Exception("无命中引脚");
        }

        private void LeftPinArea_MouseEnter(object sender, MouseEventArgs e)
        {
            HoveredPin = Instance.InputPin;
        }

        private void RightPinArea_MouseEnter(object sender, MouseEventArgs e)
        {
            HoveredPin = Instance.OutputPin;
        }

        private void PinArea_MouseLeave(object sender, MouseEventArgs e)
        {
            HoveredPin = null;
        }

        public override Point GetPinOffset(NodeView card, int pinIndex)
        {
            if (pinIndex == 0) return LeftPinArea.TranslatePoint(new Point(3, 8), card);
            return RightPinArea.TranslatePoint(new Point(14, 8), card);
        }

        public override void UpdatePinIcon()
        {
            if (Instance.InputPin.SourceList.Count == 0) Icon_LeftPin.Source = PinIconManager.Instance.ExecutePin_Null;
            else Icon_LeftPin.Source = PinIconManager.Instance.ExecutePin;
            if (Instance.OutputPin.TargetList.Count == 0) Icon_RightPin.Source = PinIconManager.Instance.ExecutePin_Null;
            else Icon_RightPin.Source = PinIconManager.Instance.ExecutePin;
        }
    }
}