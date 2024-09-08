using XLib.Base.AppFrame;
using XLib.Base.Drive;
using XNode.SubSystem.TimerSystem;

namespace XNode.SubSystem.ControlSystem
{
    public class ControlEngine : ServiceBase, ITimerHandler
    {
        #region 单例

        private ControlEngine() { }
        public static ControlEngine Instance { get; } = new ControlEngine();

        #endregion

        #region 生命周期

        public override void Start() => TimeEngine.Instance.AddTimerHandler(this);

        public override void Stop() => TimeEngine.Instance.RemoveHandler(this);

        #endregion

        #region 接口实现

        public void Tick()
        {
            foreach (var frameDriver in _frameDriverList) frameDriver.Update();
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 连接驱动器
        /// </summary>
        public void Connect(IFrameDriver frameDriver)
        {
            if (_frameDriverList.Contains(frameDriver)) return;
            _frameDriverList = new List<IFrameDriver>(_frameDriverList) { frameDriver };
        }

        /// <summary>
        /// 断开驱动器
        /// </summary>
        public void Disconnect(IFrameDriver frameDriver)
        {
            List<IFrameDriver> newList = new List<IFrameDriver>(_frameDriverList);
            newList.Remove(frameDriver);
            _frameDriverList = newList;
        }

        #endregion

        #region 字段

        /// <summary>帧驱动器列表</summary>
        private List<IFrameDriver> _frameDriverList = new List<IFrameDriver>();

        #endregion
    }
}