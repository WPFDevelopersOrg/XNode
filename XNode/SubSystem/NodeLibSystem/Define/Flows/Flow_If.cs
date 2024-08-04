using XLib.Node;

namespace XNode.SubSystem.NodeLibSystem.Define.Flows
{
    public class Flow_If : NodeBase
    {
        public override void Init()
        {
            SetViewProperty(NodeColorSet.Flow, "Flow", "判断");

            PinGroupList.Add(new ExecutePinGroup(this, "根据逻辑值执行不同的逻辑"));
            PinGroupList.Add(new DataPinGroup(this, "bool", "逻辑值", "True") { Readable = false });
            PinGroupList.Add(new ActionPinGroup(this, "逻辑真"));
            PinGroupList.Add(new ActionPinGroup(this, "逻辑假"));

            InitPinGroup();
        }

        protected override void ExecuteNode()
        {
            // 获取逻辑值引脚组
            DataPinGroup logicValueGroup = (DataPinGroup)PinGroupList[1];
            // 执行连接至逻辑值的节点
            if (logicValueGroup.InputPin != null && logicValueGroup.InputPin.SourceList.Count > 0)
                // 输入引脚.第一个源引脚.所属引脚组.所属节点.执行
                logicValueGroup.InputPin.SourceList[0].OwnerGroup.OwnerNode.Execute();
            // 更新逻辑值
            UpdateData(1);
            // 解析值
            bool logicResult = bool.Parse(GetData(1));

            if (logicResult == true) GetPinGroup<ActionPinGroup>(2).Invoke();
            else GetPinGroup<ActionPinGroup>(3).Invoke();

            GetPinGroup<ExecutePinGroup>().Execute();
        }

        public override string GetTypeString() => nameof(Flow_If);

        public override Dictionary<string, string> GetParaDict() =>
            new Dictionary<string, string> { { "LogicValue", GetData(1) } };

        public override void LoadParaDict(string version, Dictionary<string, string> paraDict)
        {
            try
            {
                SetData(1, paraDict["LogicValue"]);
            }
            catch (Exception) { }
        }

        protected override NodeBase CloneNode() => new Flow_If();
    }
}