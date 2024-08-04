using XLib.Node;

namespace XNode.SubSystem.NodeLibSystem.Define.Functions
{
    public class Func_Sleep : NodeBase
    {
        public override void Init()
        {
            SetViewProperty(NodeColorSet.Function, "Pause", "暂停执行");

            PinGroupList.Add(new ExecutePinGroup(this, "暂停指定毫秒后执行"));
            PinGroupList.Add(new DataPinGroup(this, "int", "时长", "5000") { Readable = false, Writeable = false });

            InitPinGroup();
        }

        protected override void ExecuteNode()
        {
            Thread.Sleep(int.Parse(GetData(1)));
            GetPinGroup<ExecutePinGroup>().Execute();
        }

        public override string GetTypeString() => nameof(Func_Sleep);

        public override Dictionary<string, string> GetParaDict()
        {
            return new Dictionary<string, string>
            {
                { "Time", GetData(1) }
            };
        }

        public override void LoadParaDict(string version, Dictionary<string, string> paraDict)
        {
            try
            {
                SetData(1, paraDict["Time"]);
            }
            catch (Exception) { }
        }

        protected override NodeBase CloneNode() => new Func_Sleep();
    }
}