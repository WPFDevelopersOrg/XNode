using XLib.Node;

namespace XNode.SubSystem.NodeLibSystem.Define.Functions
{
    public class Func_Log : NodeBase
    {
        public override void Init()
        {
            SetViewProperty(NodeColorSet.Function, "Function", "日志");

            PinGroupList.Add(new ExecutePinGroup(this, "发送消息至控制台"));
            PinGroupList.Add(new DataPinGroup(this, "string", "消息", "Message") { BoxWidth = 180, Readable = false });

            InitPinGroup();
        }

        protected override void ExecuteNode()
        {
            ((DataPinGroup)PinGroupList[1]).UpdateValue();

            Console.WriteLine(((DataPinGroup)PinGroupList[1]).Value);

            ((ExecutePinGroup)PinGroupList[0]).Execute();
        }

        public override string GetTypeString() => nameof(Func_Log);

        public override Dictionary<string, string> GetParaDict() =>
            new Dictionary<string, string> { { "Msg", GetData(1) } };

        public override void LoadParaDict(string version, Dictionary<string, string> paraDict)
        {
            try
            {
                SetData(1, paraDict["Msg"]);
            }
            catch (Exception) { }
        }

        protected override NodeBase CloneNode() => new Func_Log();
    }
}