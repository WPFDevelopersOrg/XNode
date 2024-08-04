using System.Windows;
using System.Windows.Media;
using XLib.WPF.Drawing;
using XNode.SubSystem.NodeEditSystem.Define;

namespace XNode.SubSystem.NodeEditSystem.Panel.Layer
{
    public class ConnectLineLayer : SingleBoard
    {
        public List<ConnectLine> ConnectLineList { get; set; } = new List<ConnectLine>();

        public override void Init() => _penExecute.Freeze();

        protected override void OnUpdate()
        {
            foreach (var line in ConnectLineList)
            {
                // 计算连接线区域
                _left = line.Start.X;
                _right = line.End.X;
                _top = line.Start.Y + 0.5;
                _bottom = line.End.Y + 0.5;

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

                Pen pen = new Pen(new SolidColorBrush(line.Color), 1);
                // 绘制形状
                if (!line.IsData) _dc.DrawGeometry(null, _penExecute, geometry);
                else _dc.DrawGeometry(null, new Pen(new SolidColorBrush(line.Color), 1), geometry);
            }
        }

        private double _left = 0;
        private double _right = 0;
        private double _top = 0;
        private double _bottom = 0;

        /// <summary>控制线最短长度</summary>
        private readonly int _minLength = 40;

        /// <summary>执行引脚线</summary>
        private readonly Pen _penExecute = new Pen(new SolidColorBrush(Color.FromArgb(255, 196, 126, 255)), 1);
    }
}