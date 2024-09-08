using System.Diagnostics;
using System.Windows.Threading;
using XLib.Base.AppFrame;

namespace XNode.SubSystem.TimerSystem
{
    /// <summary>
    /// 应用程序定时器
    /// </summary>
    public class AppTimer : ServiceBase
    {
        #region 单例

        private AppTimer()
        {
            _timer.Interval = TimeSpan.FromMilliseconds(1000.0 / 30.0);
            _timer.Tick += Timer_Tick;
        }
        public static AppTimer Instance { get; } = new AppTimer();

        #endregion

        #region 生命周期

        public override void Start()
        {
            if (_handlerList.Count > 0) _timer.Start();
        }

        public override void Stop()
        {
            _timer.Stop();
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 添加定时器处理器
        /// </summary>
        public void AddTimerHandler(ITimerHandler handler)
        {
            List<ITimerHandler> newList = new List<ITimerHandler>(_handlerList);
            if (!newList.Contains(handler)) newList.Add(handler);
            _handlerList = newList;
            _timer.Start();
        }

        /// <summary>
        /// 移除定时器处理器
        /// </summary>
        public void RemoveTimerHandler(ITimerHandler handler)
        {
            List<ITimerHandler> newList = new List<ITimerHandler>(_handlerList);
            newList.Remove(handler);
            _handlerList = newList;
            if (_handlerList.Count == 0) _timer.Stop();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 定时器.走动
        /// </summary>
        private void Timer_Tick(object? sender, EventArgs e)
        {
            foreach (var handler in _handlerList) handler.Tick();
        }

        #endregion

        #region 字段

        /// <summary>定时器</summary>
        private readonly DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.Background);
        /// <summary>定时器处理器列表</summary>
        private List<ITimerHandler> _handlerList = new List<ITimerHandler>();

        #endregion
    }
}