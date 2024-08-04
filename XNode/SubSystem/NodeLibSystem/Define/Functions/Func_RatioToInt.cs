using XLib.Node;

namespace XNode.SubSystem.NodeLibSystem.Define.Functions
{
    /// <summary>
    /// 比例转整数
    /// </summary>
    public class Func_RatioToInt : NodeBase
    {
        public override void Init()
        {
            SetViewProperty(NodeColorSet.Function, "Function", "比例转整数");

            PinGroupList.Add(new ExecutePinGroup(this, "将比例值转换为指定范围内的整数"));
            PinGroupList.Add(new DataPinGroup(this, "int", "最小值", "0") { Readable = false, Writeable = false });
            PinGroupList.Add(new DataPinGroup(this, "int", "最大值", "100") { Readable = false, Writeable = false });
            PinGroupList.Add(new DataPinGroup(this, "double", "比例", "0") { Readable = false });
            PinGroupList.Add(new DataPinGroup(this, "int", "转换结果", "0") { Readable = true, Writeable = false });

            InitPinGroup();
        }

        protected override void ExecuteNode()
        {
            // 更新比例
            ((DataPinGroup)PinGroupList[3]).UpdateValue();
            // 解析参数
            int min = int.Parse(((DataPinGroup)PinGroupList[1]).Value);
            int max = int.Parse(((DataPinGroup)PinGroupList[2]).Value);
            double ratio = double.Parse(((DataPinGroup)PinGroupList[3]).Value);
            // 计算结果
            int result = (int)(Math.Round((max - min) * ratio) + min);
            // 设置结果
            ((DataPinGroup)PinGroupList[4]).Value = result.ToString();

            ((ExecutePinGroup)PinGroupList[0]).Execute();
        }

        public override string GetTypeString() => nameof(Func_RatioToInt);

        public override Dictionary<string, string> GetParaDict()
        {
            Dictionary<string, string> result = new Dictionary<string, string>
            {
                { "Min", GetData(1) },
                { "Max", GetData(2) },
                { "Current", GetData(3) },
                { "Result", GetData(4) }
            };
            return result;
        }

        public override void LoadParaDict(string version, Dictionary<string, string> paraDict)
        {
            try
            {
                SetData(1, paraDict["Min"]);
                SetData(2, paraDict["Max"]);
                SetData(3, paraDict["Current"]);
                SetData(4, paraDict["Result"]);
            }
            catch (Exception) { }
        }

        protected override NodeBase CloneNode() => new Func_RatioToInt();
    }
}