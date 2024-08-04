using XLib.Base.Drive;
using XLib.Node;
using XNode.SubSystem.ControlSystem;

namespace XNode.SubSystem.NodeLibSystem.Define.Drivers
{
    /// <summary>
    /// 帧驱动器
    /// </summary>
    public class FrameDriver : NodeBase, IFrameDriver
    {
        #region IFrameDriver 属性

        public string Name { get; set; } = "帧驱动器";

        #endregion

        #region 生命周期

        public override void Init()
        {
            SetViewProperty(NodeColorSet.Driver, "FrameDriver", "帧驱动器");

            DataPinGroup fps = new DataPinGroup(this, "double", "帧率", "25") { Readable = false, Writeable = false };
            fps.ValueChanged += FpsChanged;
            PinGroupList.Add(fps);
            PinGroupList.Add(new ActionPinGroup(this, "更新"));

            InitPinGroup();
        }
        public override void Enable()
        {
            ControlEngine.Instance.Connect(this);
        }

        public override void Disable()
        {
            ControlEngine.Instance.Disconnect(this);
        }

        public override void Clear()
        {
            ControlEngine.Instance.Disconnect(this);
        }

        #endregion

        #region 公开方法

        public override string GetTypeString() => nameof(FrameDriver);

        public override Dictionary<string, string> GetParaDict() => new Dictionary<string, string> { { "Fps", GetData(0) } };

        public override void LoadParaDict(string version, Dictionary<string, string> paraDict)
        {
            try
            {
                SetData(0, paraDict["Fps"]);
            }
            catch (Exception) { }
        }

        #endregion

        #region IFrameDriver 方法

        public void Update()
        {
            _delay++;
            while (_delay >= _frameLength)
            {
                ((ActionPinGroup)PinGroupList[1]).Invoke();
                _delay -= _frameLength;
            }
        }

        #endregion

        #region 内部方法

        protected override NodeBase CloneNode() => new FrameDriver();

        #endregion

        #region 私有方法

        private void FpsChanged()
        {
            try
            {
                double fps = double.Parse(GetData(0));
                if (fps > 120) fps = 120;
                if (fps > 0) _frameLength = 1000 / fps;
            }
            catch (Exception) { }
        }

        #endregion

        #region 字段

        /// <summary>单帧时长：毫秒</summary>
        private double _frameLength = 40;
        /// <summary>当前延迟</summary>
        private double _delay = 0;

        #endregion
    }
}