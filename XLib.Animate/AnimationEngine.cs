using System.Diagnostics;
using XLib.Base;
using XLib.Base.AppFrame;
using XLib.Base.Ex;

namespace XLib.Animate
{
    /// <summary>
    /// 动画引擎
    /// </summary>
    public class AnimationEngine : ServiceBase
    {
        #region 单例

        private AnimationEngine()
        {
            _timer.Interval = 10;
            _timer.Tick += Timer_Tick;
        }

        public static AnimationEngine Instance { get; } = new AnimationEngine();

        #endregion

        #region 公开方法

        public override void Start()
        {
            _stopwatch.Start();
            _timer.Start();
        }

        public override void Stop()
        {
            _stopwatch.Stop();
            _stopwatch.Reset();
            _timer.Stop();
        }

        /// <summary>
        /// 添加动画
        /// </summary>
        public void AddAnimation(IAnimation animation)
        {
            _animationTimeDict.Add(animation, _stopwatch.DoubleMs());
            animation.Finished += Animation_Finished;
            List<IAnimation> newList = new List<IAnimation>(_animationList)
            {
                animation
            };
            _animationList = newList;
        }

        /// <summary>
        /// 移除动画
        /// </summary>
        public void RemoveAnimation(IAnimation animation)
        {
            _animationTimeDict.Remove(animation);
            List<IAnimation> newList = new List<IAnimation>(_animationList);
            newList.Remove(animation);
            _animationList = newList;
        }

        /// <summary>
        /// 清空动画
        /// </summary>
        public void ClearAnimation()
        {
            _animationTimeDict.Clear();
            _animationList = new List<IAnimation>();
        }

        /// <summary>
        /// 驱动动画
        /// </summary>
        public void Drive()
        {
            _currentTime = _stopwatch.DoubleMs();
            foreach (var item in _animationList)
                item.Drive(_currentTime - _animationTimeDict[item]);
        }

        #endregion

        #region 事件处理

        private void Timer_Tick() => Drive();

        private void Animation_Finished(IAnimation sender) => RemoveAnimation(sender);

        #endregion

        #region 字段

        /// <summary>秒表，提供精确时间</summary>
        private readonly Stopwatch _stopwatch = new Stopwatch();
        /// <summary>当前时间</summary>
        private double _currentTime = 0;

        /// <summary>定时器：定时驱动</summary>
        private readonly HighPrecisionTimer _timer = new HighPrecisionTimer();

        /// <summary>动画列表</summary>
        private List<IAnimation> _animationList = new List<IAnimation>();

        /// <summary>动画时间表，记录动画的添加时间</summary>
        private readonly Dictionary<IAnimation, double> _animationTimeDict = new Dictionary<IAnimation, double>();

        #endregion
    }
}