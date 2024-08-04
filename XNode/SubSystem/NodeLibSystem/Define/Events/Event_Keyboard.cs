using XLib.Node;
using XNode.SubSystem.EventSystem;

namespace XNode.SubSystem.NodeLibSystem.Define.Events
{
    /// <summary>
    /// 键盘事件
    /// </summary>
    public class Event_Keyboard : NodeBase
    {
        public override void Init()
        {
            SetViewProperty(NodeColorSet.Event, "Key", "按键");

            PinGroupList.Add(new DataPinGroup(this, "string", "当前按键", "") { BoxWidth = 120, Writeable = false });
            PinGroupList.Add(new DataPinGroup(this, "string", "监听按键", "Space") { BoxWidth = 120, Readable = false, Writeable = false });
            PinGroupList.Add(new ActionPinGroup(this, "按下"));
            PinGroupList.Add(new ActionPinGroup(this, "松开"));

            InitPinGroup();
        }

        public override void Enable()
        {
            EM.Instance.Add<string>(EventType.KeyDown, OnKeyDown);
            EM.Instance.Add<string>(EventType.KeyUp, OnKeyUp);
        }

        public override void Disable()
        {
            EM.Instance.Remove<string>(EventType.KeyDown, OnKeyDown);
            EM.Instance.Remove<string>(EventType.KeyUp, OnKeyUp);
        }

        public override void Clear()
        {
            EM.Instance.Remove<string>(EventType.KeyDown, OnKeyDown);
            EM.Instance.Remove<string>(EventType.KeyUp, OnKeyUp);
        }

        public override string GetTypeString() => nameof(Event_Keyboard);

        public override Dictionary<string, string> GetParaDict() =>
            new Dictionary<string, string> { { "ListenKey", GetData(1) } };

        public override void LoadParaDict(string version, Dictionary<string, string> paraDict)
        {
            try
            {
                SetData(1, paraDict["ListenKey"]);
            }
            catch (Exception) { }
        }

        protected override NodeBase CloneNode() => new Event_Keyboard();

        private void OnKeyDown(string key)
        {
            SetData(0, key);
            if (key == GetData(1))
                GetPinGroup<ActionPinGroup>(2).Invoke();
        }

        private void OnKeyUp(string key)
        {
            SetData(0, key);
            if (key == GetData(1))
                GetPinGroup<ActionPinGroup>(3).Invoke();
        }
    }
}