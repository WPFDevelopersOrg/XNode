using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace XLib.WPF.Drawing
{
    /// <summary>
    /// 简单画板。仅包含一个可视对象
    /// </summary>
    public abstract class SingleBoard : FrameworkElement
    {
        public SingleBoard()
        {
            AddVisualChild(_visual);
            AddLogicalChild(_visual);
        }

        public Point Point { get; set; } = new Point();

        protected override int VisualChildrenCount => 1;

        protected override Visual GetVisualChild(int index) => _visual;

        public virtual void Init() { }

        public void Update()
        {
            _dc = _visual.RenderOpen();
            if (IsEnabled) OnUpdate();
            _dc.Close();
        }

        public void Clear() => _visual.RenderOpen().Close();

        protected abstract void OnUpdate();

        /// <summary>
        /// 绘制顶点
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void DrawVertex(DrawingContext dc, Point point, double size, Brush brush, Pen pen)
        {
            double x = point.X - size / 2;
            double y = point.Y - size / 2;
            dc.DrawRectangle(brush, pen, new Rect(x, y, size, size));
        }

        private readonly DrawingVisual _visual = new DrawingVisual();
        protected DrawingContext? _dc;
    }
}