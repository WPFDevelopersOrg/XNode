using System.Windows;

namespace XNode.SubSystem.NodeEditSystem.Define
{
    /// <summary>
    /// 目标框
    /// </summary>
    public class TargetBox
    {
        public Point ScreenPoint { get; set; }

        public double Height { get; set; }

        public double Width { get; set; }

        /// <summary>外框偏移。正数向外，负数向内</summary>
        public double BoxOffset { get; set; } = 0;

        /// <summary>
        /// 获取绘制目标框的坐标列表。共绘制八条线，每条线两个坐标
        /// </summary>
        public List<Point> GetPointList(int lineLength)
        {
            List<Point> result = new List<Point>();

            double left = ScreenPoint.X - BoxOffset;
            double right = ScreenPoint.X + Width + BoxOffset;
            double top = ScreenPoint.Y - BoxOffset;
            double bottom = ScreenPoint.Y + Height + BoxOffset;

            double hx1 = left + lineLength;
            double hx2 = right - lineLength;
            double hy1 = top + 0.5;
            double hy2 = bottom - 0.5;

            result.Add(new Point(left, hy1));
            result.Add(new Point(hx1, hy1));
            result.Add(new Point(hx2, hy1));
            result.Add(new Point(right, hy1));

            result.Add(new Point(left, hy2));
            result.Add(new Point(hx1, hy2));
            result.Add(new Point(hx2, hy2));
            result.Add(new Point(right, hy2));

            double vx1 = left + 0.5;
            double vx2 = right - 0.5;
            double vy1 = top + lineLength;
            double vy2 = bottom - lineLength;

            result.Add(new Point(vx1, top));
            result.Add(new Point(vx1, vy1));
            result.Add(new Point(vx1, vy2));
            result.Add(new Point(vx1, bottom));

            result.Add(new Point(vx2, top));
            result.Add(new Point(vx2, vy1));
            result.Add(new Point(vx2, vy2));
            result.Add(new Point(vx2, bottom));

            return result;
        }
    }
}