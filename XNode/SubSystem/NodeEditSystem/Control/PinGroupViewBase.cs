using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XLib.Node;

namespace XNode.SubSystem.NodeEditSystem.Control
{
    /// <summary>
    /// 引脚组视图基类
    /// </summary>
    public class PinGroupViewBase : UserControl
    {
        /// <summary>悬停引脚</summary>
        public PinBase? HoveredPin { get; set; } = null;

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init() { }

        /// <summary>
        /// 获取引脚区域控件
        /// </summary>
        public virtual Grid GetPinArea() => throw new NotImplementedException();

        /// <summary>
        /// 获取悬停引脚相对于鼠标的偏移量
        /// </summary>
        public Point GetHoveredPinOffset()
        {
            Grid pinArea = GetPinArea();
            Point offset = Mouse.GetPosition(pinArea);
            if (HoveredPin.Flow == PinFlow.Input)
            {
                offset.X = 3 - offset.X;
                offset.Y = 8 - offset.Y;
            }
            else
            {
                offset.X = 14 - offset.X;
                offset.Y = 8 - offset.Y;
            }
            return offset;
        }

        /// <summary>
        /// 获取相对于节点的坐标
        /// </summary>
        public virtual Point GetPinOffset(NodeView card, int pinIndex) => new Point();

        /// <summary>
        /// 更新引脚图标
        /// </summary>
        public virtual void UpdatePinIcon() { }
    }
}