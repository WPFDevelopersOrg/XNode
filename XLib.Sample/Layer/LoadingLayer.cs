using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using XLib.Animate;
using XLib.Math.Easing;
using XLib.WPF.Drawing;

namespace XLib.Sample.Layer
{
    /// <summary>
    /// 加载图层。播放加载动画，测试动画用
    /// </summary>
    public class LoadingLayer : SingleBoard, IMotion
    {
        public void Start()
        {
            // 创建并添加动画队列
            _group.Add(this.CreateAnimation("offset", 0, 200, 4300, loop: true));
            _group.Add(CreatQueue("x1", 0, 0, 200, 400, 800));
            _group.Add(CreatQueue("x2", 200, 0 - 8, 200 - 8, 400 - 8, 600));
            _group.Add(CreatQueue("x3", 400, 0 - 16, 200 - 16, 400 - 16, 400));
            _group.Add(CreatQueue("x4", 600, 0 - 24, 200 - 24, 400 - 24, 200));
            _group.Add(CreatQueue("x5", 800, 0 - 32, 200 - 32, 400 - 32, 0));
            // 添加动画
            AnimationEngine.Instance.AddAnimation(_group);
        }

        private AnimationQueue CreatQueue(string property, double leftDelay, double x1, double x2, double x3, double rightDelay)
        {
            AnimationQueue queue = new AnimationQueue { Loop = true };
            queue.AnimationList.Add(new AnimationDelay { Duration = leftDelay });
            queue.AnimationList.Add(this.CreateAnimation(property, x1, x2, 1000, EasingType.QuinticEase, EasingMode.EaseOut));
            queue.AnimationList.Add(new AnimationDelay { Duration = 500 });
            queue.AnimationList.Add(this.CreateAnimation(property, x2, x3, 1000, EasingType.QuinticEase, EasingMode.EaseIn));
            queue.AnimationList.Add(new AnimationDelay { Duration = 1000 });
            queue.AnimationList.Add(new AnimationDelay { Duration = rightDelay });
            queue.Init();
            return queue;
        }

        private void Group_Finished(IAnimation sender)
        {
            Dispatcher.Invoke(Clear);
        }

        public void Stop()
        {
            _group.Stop();
        }

        public override void Init() => _brush.Freeze();

        protected override void OnUpdate()
        {
            DrawVertex(_dc, new Point(_x1 + _offset, 0), 4, _brush, null);
            DrawVertex(_dc, new Point(_x2 + _offset, 0), 4, _brush, null);
            DrawVertex(_dc, new Point(_x3 + _offset, 0), 4, _brush, null);
            DrawVertex(_dc, new Point(_x4 + _offset, 0), 4, _brush, null);
            DrawVertex(_dc, new Point(_x5 + _offset, 0), 4, _brush, null);
        }

        public double GetMotionProperty(string propertyName) => 0;

        public void SetMotionProperty(string propertyName, double value)
        {
            switch (propertyName)
            {
                case "offset":
                    _offset = value;
                    break;
                case "x1":
                    _x1 = value;
                    break;
                case "x2":
                    _x2 = value;
                    break;
                case "x3":
                    _x3 = value;
                    break;
                case "x4":
                    _x4 = value;
                    break;
                case "x5":
                    _x5 = value;
                    break;
            }
            Dispatcher.Invoke(Update);
        }

        private double _offset = 0;
        private double _x1 = 0;
        private double _x2 = 0;
        private double _x3 = 0;
        private double _x4 = 0;
        private double _x5 = 0;

        private AnimationGroup _group= new AnimationGroup();

        private readonly Brush _brush = new SolidColorBrush(Colors.Black);
    }
}