using XLib.Node;

namespace XNode.SubSystem.NodeLibSystem.Define.Flows
{
    public class Flow_While : NodeBase
    {
        public override void Init()
        {
            SetViewProperty(NodeColorSet.Flow, "Loop", "条件循环");

            PinGroupList.Add(new ExecutePinGroup(this, "逻辑值为真，则无限循环执行"));
            PinGroupList.Add(new DataPinGroup(this, "bool", "逻辑值", "True") { Readable = false });
            PinGroupList.Add(new ActionPinGroup(this, "循环体"));

            InitPinGroup();
        }

        protected override void ExecuteNode()
        {
            // 获取逻辑值引脚组
            DataPinGroup logicValueGroup = (DataPinGroup)PinGroupList[1];
            // 获取循环体引脚组
            ActionPinGroup loopBodyGroup = GetPinGroup<ActionPinGroup>(2);

            SynchronizationContext? context = SynchronizationContext.Current;
            if (context == null)
            {
                context = new SynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(context);
            }
            Task.Run(() => ExecuteWhileSync(logicValueGroup, loopBodyGroup, context));
        }

        public override string GetTypeString() => nameof(Flow_While);

        public override Dictionary<string, string> GetParaDict() =>
            new Dictionary<string, string> { { "LogicValue", GetData(1) } };

        protected override NodeBase CloneNode() => new Flow_While();

        private void ExecuteWhileSync(DataPinGroup logicValueGroup, ActionPinGroup loopBodyGroup, SynchronizationContext context)
        {
            try
            {
                while (true)
                {
                    // 执行连接至逻辑值的节点
                    if (logicValueGroup.InputPin != null && logicValueGroup.InputPin.SourceList.Count > 0)
                        // 输入引脚.第一个源引脚.所属引脚组.所属节点.执行
                        logicValueGroup.InputPin.SourceList[0].OwnerGroup.OwnerNode.Execute();
                    // 更新逻辑值
                    UpdateData(1);
                    // 如果值为假，则退出循环
                    if (!bool.Parse(GetData(1))) break;
                    // 执行循环体
                    loopBodyGroup.Invoke();
                }
                context.Post(_ => GetPinGroup<ExecutePinGroup>().Execute(), null);
            }
            catch (Exception ex)
            {
                context.Post(_ =>
                {
                    RunError = true;
                    Stop();
                    InvokeExecuteError(ex);
                }, null);
            }
        }
    }
}