using XLib.Math.Easing;

namespace XLib.Animate
{
    /// <summary>
    /// 表示一个动画
    /// </summary>
    public class Animation : IAnimation
    {
        #region 属性

        /// <summary>运动对象</summary>
        public IMotion? MotionObject { get; set; } = null;

        /// <summary>运动属性</summary>
        public string MotionProperty { get; set; } = "";

        /// <summary>起始值</summary>
        public double BeginValue { get; set; } = 0;

        /// <summary>结束值</summary>
        public double EndValue { get; set; } = 1;

        /// <summary>持续时间</summary>
        public double Duration { get; set; } = 1;

        /// <summary>缓动函数。默认为直线</summary
        public IEasingFunction EasingFunction { get; set; } = new LinearEase();

        /// <summary>起始延迟：起始处等待的时长</summary>
        public double StartDelay { get; set; } = 0;

        /// <summary>结束延迟：结束处等待的时长</summary>
        public double EndDelay { get; set; } = 0;

        /// <summary>循环播放</summary>
        public bool Loop { get; set; } = false;

        #endregion

        #region 事件

        /// <summary>动画已完成</summary>
        public event Action<IAnimation> Finished;

        #endregion

        #region 构造方法

        public Animation(IMotion obj)
        {
            MotionObject = obj;
        }

        #endregion

        #region IDriveable 方法

        public double GetTotalLength() => StartDelay + Duration + EndDelay;

        public void Drive(double time)
        {
            // 处理延迟
            // time -= Delay;
            time = _timeStart + time;
            // 处理循环
            double offset = time - _timeStart;
            if (Loop && offset > _timeLength)
            {
                offset %= _timeLength;
                time = _timeStart + offset;
            }

            if (time < 0)
            {
                MotionObject.SetMotionProperty(MotionProperty, BeginValue);
            }
            else if (time >= Duration)
            {
                MotionObject.SetMotionProperty(MotionProperty, EndValue);
                if (time >= Duration + EndDelay) Finished?.Invoke(this);
            }
            else
            {
                // 计算时间比例
                double t = EasingFunction.Ease(time / Duration);
                // 计算当前值
                double value = BeginValue + _difference * t;
                // 设置属性值
                MotionObject.SetMotionProperty(MotionProperty, value);
            }
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            // 计算差值
            _difference = EndValue - BeginValue;
            // 计算时间起点
            _timeStart = -StartDelay;
            // 计算总时长
            _timeLength = Duration + StartDelay + EndDelay;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop() => Finished?.Invoke(this);

        #endregion

        #region 字段

        /// <summary>差值</summary>
        private double _difference = 0;

        /// <summary>时间起点</summary>
        private double _timeStart = 0;
        /// <summary>时长</summary>
        private double _timeLength = 0;

        #endregion
    }
}