using Range = XLib.Math.Range;

namespace XLib.Animate
{
    /// <summary>
    /// 动画队列：用于按顺序驱动一组动画
    /// </summary>
    public class AnimationQueue : IAnimation
    {
        #region 属性、事件

        /// <summary>动画列表</summary>
        public List<IAnimation> AnimationList { get; set; } = new List<IAnimation>();

        public bool Loop { get; set; } = false;

        /// <summary>动画已完成</summary>
        public event Action<IAnimation> Finished;

        #endregion

        #region IDriveable 方法

        public double GetTotalLength() => _totalLength;

        public void Drive(double time)
        {
            if (time < 0) return;

            if (Loop && time >= _totalLength) time %= _totalLength;
            if (time >= _totalLength)
            {
                Finished?.Invoke(this);
                return;
            }

            // 获取下一个动画
            _next = GetAnimation(time);
            // 切换动画
            if (_next != _current)
            {
                // 当前时间跑到前面时，驱动至初始状态
                if (time < _prevTime) BackToStart();
                // 在切换前确保当前动画播放至最后
                _current?.Drive(time - _timeDict[_current].Left);
                // 切换
                _current = _next;
            }
            // 驱动当前动画
            _current?.Drive(time - _timeDict[_current].Left);
            _prevTime = time;
        }

        public void Stop() => Finished?.Invoke(this);

        #endregion

        #region 公开方法

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if (AnimationList.Count == 0) throw new Exception("当前队列没有动画");

            // 计算每个动画的时间段
            double current = 0;
            foreach (var item in AnimationList)
            {
                double itemLength = item.GetTotalLength();
                _timeDict.Add(item, new Range(current, current + itemLength));
                current += itemLength;
                _totalLength += itemLength;
            }
            // 设置第一个动画
            _current = GetAnimation(0);
            // 驱动至初始状态
            BackToStart();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取当前时间对应的动画
        /// </summary>
        private IAnimation? GetAnimation(double time)
        {
            foreach (var item in _timeDict)
                if (item.Value.Left <= time && time < item.Value.Right) return item.Key;
            return null;
        }

        /// <summary>
        /// 回到第一个动画的状态
        /// </summary>
        private void BackToStart()
        {
            // Trace.WriteLine("回到起点");
            foreach (var item in AnimationList)
            {
                // 忽略延迟
                if (item is AnimationDelay) continue;
                // 回到起点
                item.Drive(0);
                break;
            }
        }

        #endregion

        #region 字段

        /// <summary>动画时间表</summary>
        private readonly Dictionary<IAnimation, Range> _timeDict = new Dictionary<IAnimation, Range>();
        /// <summary>总时长</summary>
        private double _totalLength = 0;

        /// <summary>当前动画</summary>
        private IAnimation? _current = null;
        /// <summary>下一个动画</summary>
        private IAnimation? _next = null;

        /// <summary>上一次驱动时间</summary>
        private double _prevTime = 0;

        #endregion
    }
}