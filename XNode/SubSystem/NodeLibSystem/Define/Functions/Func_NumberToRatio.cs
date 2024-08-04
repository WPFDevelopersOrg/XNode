using XLib.Base.Ex;
using XLib.Node;

namespace XNode.SubSystem.NodeLibSystem.Define.Functions
{
    public class Func_NumberToRatio : NodeBase
    {
        public override void Init()
        {
            SetViewProperty(NodeColorSet.Function, "Function", "数值转比例");

            PinGroupList.Add(new ExecutePinGroup(this, "将指定范围内的数值转换为比例值"));
            PinGroupList.Add(new DataPinGroup(this, "double", "最小值", "0") { Readable = false, Writeable = false });
            PinGroupList.Add(new DataPinGroup(this, "double", "最大值", "100") { Readable = false, Writeable = false });
            PinGroupList.Add(new DataPinGroup(this, "double", "当前值", "0") { Readable = false });
            PinGroupList.Add(new DataPinGroup(this, "double", "转换结果", "0") { Readable = true, Writeable = false });

            NodeProperty precision = new NodeProperty("int", "小数位数", "2");
            precision.ValueChanged += Precision_ValueChanged;
            PropertyList.Add(precision);

            InitPinGroup();
        }

        protected override void ExecuteNode()
        {
            // 更新当前值
            ((DataPinGroup)PinGroupList[3]).UpdateValue();
            // 解析参数
            double min = double.Parse(((DataPinGroup)PinGroupList[1]).Value);
            double max = double.Parse(((DataPinGroup)PinGroupList[2]).Value);
            double value = double.Parse(((DataPinGroup)PinGroupList[3]).Value).Limit(min, max);
            // 计算结果
            double result = (value - min) / (max - min);
            // 设置结果
            ((DataPinGroup)PinGroupList[4]).Value = result.ToString(_format);

            ((ExecutePinGroup)PinGroupList[0]).Execute();
        }

        public override string GetTypeString() => nameof(Func_NumberToRatio);

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

        public override Dictionary<string, string> GetPropertyDict()
        {
            return new Dictionary<string, string>
            {
                { "Precision", PropertyList[0].Value }
            };
        }

        public override void LoadPropertyDict(string version, Dictionary<string, string> propertyDict)
        {
            try
            {
                PropertyList[0].Value = propertyDict["Precision"];
            }
            catch (Exception) { }
        }

        protected override NodeBase CloneNode() => new Func_NumberToRatio();

        private void Precision_ValueChanged(string value)
        {
            int count = int.Parse(value).Limit(0, 8);
            _format = count == 0 ? "0" : "0." + new string('0', count);
        }

        private string _format = "0.00";
    }
}