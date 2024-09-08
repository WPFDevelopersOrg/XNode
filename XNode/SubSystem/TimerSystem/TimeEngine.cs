using System.Diagnostics;
using XLib.Base;
using XLib.Base.AppFrame;
using XLib.Base.Ex;

namespace XNode.SubSystem.TimerSystem
{
    /// <summary>
    /// 时间引擎
    /// 其中定时器以每秒一千次模拟实时触发
    /// </summary>
    public class TimeEngine : ServiceBase
    {
        #region 单例

        private TimeEngine() => _timer.Tick += Timer_Tick;
        public static TimeEngine Instance { get; } = new TimeEngine();

        #endregion

        #region 属性

        /// <summary>当前时间。单位：毫秒</summary>
        public static double Time => Instance._stopwatch.DoubleMs();

        /// <summary>秒表实例</summary>
        public static Stopwatch Watch => Instance._stopwatch;

        #endregion

        #region 生命周期

        public override void Start()
        {
            _timer.Start();
            _stopwatch.Start();
        }

        public override void Stop()
        {
            _timer.Stop();
            _stopwatch.Stop();
        }

        #endregion

        #region 公开方法

        public void AddTimerHandler(ITimerHandler handler)
        {
            if (_handlerList.Contains(handler)) return;
            _handlerList = new List<ITimerHandler>(_handlerList) { handler };
        }

        public void RemoveHandler(ITimerHandler handler)
        {
            List<ITimerHandler> newList = new List<ITimerHandler>(_handlerList);
            newList.Remove(handler);
            _handlerList = newList;
        }

        #endregion

        #region 私有方法

        private void Timer_Tick()
        {
            try
            {
                foreach (var item in _handlerList) item.Tick();
            }
            catch (Exception) { }
        }

        #endregion

        #region 字段

        /// <summary>秒表：用作应用程序的精确时间参考</summary>
        private readonly Stopwatch _stopwatch = new Stopwatch();
        /// <summary>定时器：定时驱动引擎</summary>
        private readonly HighPrecisionTimer _timer = new HighPrecisionTimer();

        /// <summary>定时处理器列表</summary>
        private List<ITimerHandler> _handlerList = new List<ITimerHandler>();

        #endregion
    }
}