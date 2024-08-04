using XLib.Node;

namespace XNode.SubSystem.NodeLibSystem.Define.Flows
{
    public class Flow_LoopByCount : NodeBase
    {
        public override void Init()
        {
            SetViewProperty(NodeColorSet.Flow, "Loop", "计数循环");

            PinGroupList.Add(new ExecutePinGroup(this, "循环执行多少次循环体"));
            PinGroupList.Add(new DataPinGroup(this, "int", "次数", "10") { Readable = false });
            PinGroupList.Add(new ActionPinGroup(this, "循环体"));

            InitPinGroup();
        }

        protected override void ExecuteNode()
        {
            int count = int.Parse(GetData(1));
            for (int counter = 0; counter < count; counter++)
                GetPinGroup<ActionPinGroup>(2).Invoke();
            GetPinGroup<ExecutePinGroup>().Execute();
        }

        public override string GetTypeString() => nameof(Flow_LoopByCount);

        public override Dictionary<string, string> GetParaDict() =>
            new Dictionary<string, string> { { "Times", GetData(1) } };

        protected override NodeBase CloneNode() => new Flow_LoopByCount();
    }
}