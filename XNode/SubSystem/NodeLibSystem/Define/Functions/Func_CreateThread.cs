using XLib.Node;

namespace XNode.SubSystem.NodeLibSystem.Define.Functions
{
    public class Func_CreateThread : NodeBase
    {
        public override void Init()
        {
            SetViewProperty(NodeColorSet.Function, "Function", "多线程执行");

            PinGroupList.Add(new ExecutePinGroup(this, "创建新线程，并在新线程中执行后续节点"));

            InitPinGroup();
        }

        protected override void ExecuteNode()
        {
            Task.Run(() => GetPinGroup<ExecutePinGroup>().Execute());
        }

        public override string GetTypeString() => nameof(Func_CreateThread);

        public override Dictionary<string, string> GetParaDict() => new Dictionary<string, string>();

        protected override NodeBase CloneNode() => new Func_CreateThread();
    }
}