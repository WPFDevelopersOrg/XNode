using XLib.Base.Drive;
using XLib.Base.Ex;
using XLib.Node;
using XNode.SubSystem.ControlSystem;

namespace XNode.SubSystem.NodeLibSystem.Define.Drivers
{
    /// <summary>
    /// 定时驱动器
    /// </summary>
    public class TimerDriver : NodeBase, IFrameDriver, IProgressGetter
    {
        #region 生命周期

        public override void Init()
        {
            SetViewProperty(NodeColorSet.Driver, "Timer", "定时驱动器");

            DataPinGroup time = new DataPinGroup(this, "double", "间隔毫秒", "5000") { Readable = false, Writeable = false };
            time.ValueChanged += Time_ValueChanged;
            PinGroupList.Add(time);
            PinGroupList.Add(new ActionPinGroup(this, "更新"));

            InitPinGroup();
        }

        public override void Enable()
        {
            OpenProgressBar?.Invoke(this);
            ControlEngine.Instance.Connect(this);
        }

        public override void Disable()
        {
            CloseProgressBar?.Invoke();
            ControlEngine.Instance.Disconnect(this);
            _delay = 0;
        }

        public override void Clear()
        {
            CloseProgressBar?.Invoke();
            ControlEngine.Instance.Disconnect(this);
            _delay = 0;
        }

        #endregion

        #region NodeBase 方法

        public override string GetTypeString() => nameof(TimerDriver);

        public override Dictionary<string, string> GetParaDict()
        {
            return new Dictionary<string, string>
            {
                { "Time", GetData(0) }
            };
        }

        public override void LoadParaDict(string version, Dictionary<string, string> paraDict)
        {
            try
            {
                SetData(0, paraDict["Time"]);
            }
            catch (Exception) { }
        }

        protected override NodeBase CloneNode() => new TimerDriver();

        #endregion

        #region IFrameDriver 方法

        public void Update()
        {
            _delay++;
            while (_delay >= _frameLength)
            {
                GetPinGroup<ActionPinGroup>(1).Invoke();
                _delay -= _frameLength;
            }
        }

        #endregion

        #region IProgressGetter 方法

        public double GetProgress() => _delay / _frameLength;

        #endregion

        #region 私有方法

        private void Time_ValueChanged()
        {
            try
            {
                _frameLength = double.Parse(GetData(0)).Limit(1000 / 120.0, double.MaxValue);
            }
            catch (Exception) { }
        }

        #endregion

        #region 字段

        /// <summary>单帧时长：毫秒</summary>
        private double _frameLength = 5000;
        /// <summary>当前延迟</summary>
        private double _delay = 0;

        #endregion
    }
}