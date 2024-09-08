using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using XLib.Animate;
using XLib.Math.Easing;
using XLib.WPF.Drawing;

namespace XLib.Sample.Layer
{
    /// <summary>
    /// 目标点图层。测试动画用
    /// </summary>
    public class TargetPointLayer : SingleBoard, IMotion
    {
        public Point TargetPoint { get; set; } = new Point();

        public override void Init()
        {
            _pen.Freeze();
        }

        protected override void OnUpdate()
        {
            Opacity = _opacity;
            _dc.DrawEllipse(null, _pen, TargetPoint, _size / 2, _size / 2);
        }

        public double GetMotionProperty(string propertyName) => 0;

        public void SetMotionProperty(string propertyName, double value)
        {
            switch (propertyName)
            {
                case "Size":
                    _size = value;
                    Console.WriteLine(_size);
                    break;
                case "Alpha":
                    _opacity = value;
                    break;
            }
            Dispatcher.Invoke(Update);
        }

        public void SetTargetPoint(Point point)
        {
            TargetPoint = point;
            // 创建动画组
            AnimationGroup group = new AnimationGroup();
            group.Add(this.CreateAnimation("Size", 0, 400, 2800, EasingType.QuinticEase, EasingMode.EaseOut, loop: true));
            group.Add(this.CreateAnimation("Alpha", 1, 0, 2800, loop: true));
            // 添加动画组
            AnimationEngine.Instance.AddAnimation(group);
        }

        private double _size = 0;
        private double _opacity = 1;

        private readonly Pen _pen = new Pen(Brushes.Black, 1);
    }
}