using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using XLib.Node;
using XNode.SubSystem.NodeEditSystem.Define;
using XNode.SubSystem.ResourceSystem;

namespace XNode.SubSystem.NodeEditSystem.Control
{
    public partial class DataPinGroupView : PinGroupViewBase
    {
        #region 构造方法

        public DataPinGroupView() => InitializeComponent();

        #endregion

        #region 属性

        public DataPinGroup? Instance { get; set; } = null;

        #endregion

        #region 基类方法

        public override void Init()
        {
            if (Instance == null) return;

            Block_Name.Text = Instance.Name;
            // Block_Name.Text = Instance.Name + GetTypeName();
            Block_Name.Foreground = new SolidColorBrush(GetDataPinColor());
            Input_Value.Text = Instance.Value;
            Input_Value.IsReadOnly = !Instance.CanInput;
            InputBoxArea.Width = new GridLength(Instance.BoxWidth);

            // 无输入引脚：隐藏引脚图标与区域
            if (Instance.InputPin == null)
            {
                Icon_LeftPin.Visibility = Visibility.Collapsed;
                LeftPinArea.Visibility = Visibility.Collapsed;
            }
            // 设置图标
            else
            {
                Icon_LeftPin.Source = GetDataPinIcon();
                LeftPinArea.MouseEnter += LeftPinArea_MouseEnter;
                LeftPinArea.MouseLeave += PinArea_MouseLeave;
            }

            // 无输出引脚：隐藏引脚图标与区域
            if (Instance.OutputPin == null)
            {
                Icon_RightPin.Visibility = Visibility.Collapsed;
                RightPinArea.Visibility = Visibility.Collapsed;
            }
            // 设置图标
            else
            {
                Icon_RightPin.Source = GetDataPinIcon();
                RightPinArea.MouseEnter += RightPinArea_MouseEnter;
                RightPinArea.MouseLeave += PinArea_MouseLeave;
            }

            Instance.ValueChanged += ValueChanged;
            Input_Value.TextChanged += Input_Value_TextChanged;
        }

        public override Grid GetPinArea()
        {
            if (Instance.InputPin != null && HoveredPin == Instance.InputPin) return LeftPinArea;
            if (Instance.OutputPin != null && HoveredPin == Instance.OutputPin) return RightPinArea;
            throw new Exception("无命中引脚");
        }

        public override Point GetPinOffset(NodeView card, int pinIndex)
        {
            if (pinIndex == 0) return LeftPinArea.TranslatePoint(new Point(3, 8), card);
            return RightPinArea.TranslatePoint(new Point(14, 8), card);
        }

        public override void UpdatePinIcon()
        {
            if (Instance.InputPin != null)
                Icon_LeftPin.Source = GetDataPinIcon(Instance.InputPin.SourceList.Count > 0);
            if (Instance.OutputPin != null)
                Icon_RightPin.Source = GetDataPinIcon(Instance.OutputPin.TargetList.Count > 0);
        }

        #endregion

        #region 控件事件

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

        #endregion

        #region 私有方法

        private Color GetDataPinColor()
        {
            return Instance.Type switch
            {
                "int" => PinColorSet.Int,
                "double" => PinColorSet.Double,
                "string" => PinColorSet.String,
                "bool" => PinColorSet.Bool,
                "byte[]" => PinColorSet.ByteArray,
                _ => Colors.White,
            };
        }

        private BitmapSource? GetDataPinIcon(bool solid = false)
        {
            return Instance.Type switch
            {
                "int" or "double" or "string" or "bool" or "byte[]" => PinIconManager.Instance.GetDataPinIcon(Instance.Type, solid),
                _ => null,
            };
        }

        private void ValueChanged()
        {
            Dispatcher.Invoke(() => Input_Value.Text = Instance.Value);
        }

        private void Input_Value_TextChanged(object sender, TextChangedEventArgs e)
        {
            Instance.Value = Input_Value.Text;
        }

        #endregion
    }
}