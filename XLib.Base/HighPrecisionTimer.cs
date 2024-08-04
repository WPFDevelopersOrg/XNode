using System.ComponentModel;
using System.Runtime.InteropServices;

namespace XLib.Base
{
    [StructLayout(LayoutKind.Sequential)]
    struct TimerCaps
    {
        public int periodMin;
        public int periodMax;
    }

    /// <summary>
    /// 高精度定时器
    /// </summary>
    public sealed class HighPrecisionTimer : IComponent, IDisposable
    {
        // 系统定时器回调
        private delegate void TimerCallback(int id, int msg, int user, int param1, int param2);

        #region 动态库接口

        [DllImport("winmm.dll")]
        /// <summary>查询计时器设备以确定其分辨率</summary>
        private static extern int timeGetDevCaps(ref TimerCaps caps, int sizeOfTimerCaps);

        [DllImport("winmm.dll")]
        /// <summary>绑定定时器事件</summary>
        private static extern int timeSetEvent(int delay, int resolution, TimerCallback callback, int user, int mode);

        [DllImport("winmm.dll")]
        /// <summary>终止定时器</summary>
        private static extern int timeKillEvent(int id);

        #endregion

        #region 构造、析构方法

        // 静态构造方法
        static HighPrecisionTimer()
        {
            _ = timeGetDevCaps(ref _caps, Marshal.SizeOf(_caps));
        }

        public HighPrecisionTimer()
        {
            IsRunning = false;

            _interval = _caps.periodMin;
            _resolution = _caps.periodMin;
            // 绑定回调
            _timerCallback = new TimerCallback(TimerEventCallback);
        }

        public HighPrecisionTimer(IContainer container) : this()
        {
            container.Add(this);
        }

        ~HighPrecisionTimer()
        {
            _ = timeKillEvent(_timerID);
        }

        #endregion

        #region 属性

        public int Interval
        {
            get => _interval;
            set
            {
                if ((value < _caps.periodMin) || (value > _caps.periodMax))
                    throw new Exception("超出计时范围！");
                _interval = value;
            }
        }

        public bool IsRunning { get; private set; }

        public ISite? Site { get; set; }

        #endregion

        #region 公开事件

        public event EventHandler? Disposed;
        public event Action? Tick;

        #endregion

        #region 公开方法

        public void Start()
        {
            if (!IsRunning)
            {
                // 尝试在系统中设置一个定时器，设置成功会返回定时器编号
                _timerID = timeSetEvent(_interval, _resolution, _timerCallback, 0, 1);
                // 设置定时器失败
                if (_timerID == 0) throw new Exception("无法启动计时器");
                IsRunning = true;
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                _ = timeKillEvent(_timerID);
                IsRunning = false;
            }
        }

        public void Dispose()
        {
            _ = timeKillEvent(_timerID);
            GC.SuppressFinalize(this);
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region 内部方法

        // 定时器回调
        private void TimerEventCallback(int id, int msg, int user, int param1, int param2)
        {
            // 引发事件
            Tick?.Invoke();
        }

        #endregion

        #region 字段

        // 系统定时器分辨率
        private static TimerCaps _caps;
        // 定时器间隔
        private int _interval = 1;
        // 定时器分辨率
        private readonly int _resolution;
        // 定时器回调
        private readonly TimerCallback _timerCallback;
        // 定时器编号
        private int _timerID;

        #endregion
    }
}