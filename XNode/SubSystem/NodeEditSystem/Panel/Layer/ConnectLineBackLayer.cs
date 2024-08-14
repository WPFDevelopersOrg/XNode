using System.Windows;
using System.Windows.Media;
using XLib.WPF.Drawing;

namespace XNode.SubSystem.NodeEditSystem.Panel.Layer
{
    /// <summary>
    /// 连接线背景图层
    /// </summary>
    public class ConnectLineBackLayer : SingleBoard
    {
        public Point Start { get; set; }

        public Point End { get; set; }

        public override void Init() => _pen.Freeze();

        protected override void OnUpdate()
        {
            // 计算连接线区域
            _left = Start.X;
            _right = End.X;
            _top = Start.Y + 0.5;
            _bottom = End.Y + 0.5;

            // 计算贝塞尔曲线的控制线长度
            double controlLineLength = (_right - _left) / 2;
            if (controlLineLength < _minLength) controlLineLength = _minLength;

            // 创建形状
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure();
            geometry.Figures.Add(figure);

            // 计算贝塞尔曲线的控制点与终点
            Point p1 = new Point(_left + controlLineLength, _top);
            Point p2 = new Point(_right - controlLineLength, _bottom);
            Point endPoint = new Point(_right, _bottom);

            // 设置起点并添加贝塞尔曲线
            figure.StartPoint = new Point(_left, _top);
            figure.Segments.Add(new BezierSegment(p1, p2, endPoint, true));

            _dc.DrawGeometry(null, _pen, geometry);
        }

        private double _left = 0;
        private double _right = 0;
        private double _top = 0;
        private double _bottom = 0;

        /// <summary>控制线最短长度</summary>
        private readonly int _minLength = 40;

        private readonly Pen _pen = new Pen(new SolidColorBrush(Color.FromArgb(64, 255, 255, 255)), 5);
    }
}