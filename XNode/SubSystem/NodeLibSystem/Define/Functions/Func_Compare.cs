using XLib.Node;

namespace XNode.SubSystem.NodeLibSystem.Define.Functions
{
    public class Func_Compare : NodeBase
    {
        public override void Init()
        {
            SetViewProperty(NodeColorSet.Function, "Function", "关系运算");

            PinGroupList.Add(new ExecutePinGroup(this, "对比两个值的关系"));
            PinGroupList.Add(new DataPinGroup(this, "double", "数值一", "0") { Readable = false });
            PinGroupList.Add(new DataPinGroup(this, "string", "运算符", "=") { Readable = false, Writeable = false });
            PinGroupList.Add(new DataPinGroup(this, "double", "数值二", "0") { Readable = false });
            PinGroupList.Add(new DataPinGroup(this, "bool", "对比结果", "True") { Writeable = false });

            InitPinGroup();
        }

        protected override void ExecuteNode()
        {
            // 更新数值一、数值二
            ((DataPinGroup)PinGroupList[1]).UpdateValue();
            ((DataPinGroup)PinGroupList[3]).UpdateValue();
            // 解析数值
            double num1 = double.Parse(((DataPinGroup)PinGroupList[1]).Value);
            double num2 = double.Parse(((DataPinGroup)PinGroupList[3]).Value);
            // 执行对比
            bool result = false;
            switch (((DataPinGroup)PinGroupList[2]).Value)
            {
                case "<":
                    result = num1 < num2;
                    break;
                case "=":
                    result = num1 == num2;
                    break;
                case ">":
                    result = num1 > num2;
                    break;
                case "<=":
                    result = num1 < num2 || num1 == num2;
                    break;
                case ">=":
                    result = num1 > num2 || num1 == num2;
                    break;
                case "!=":
                    result = num1 != num2;
                    break;
            }
            // 更新结果
            ((DataPinGroup)PinGroupList[4]).Value = result == true ? "True" : "False";

            ((ExecutePinGroup)PinGroupList[0]).Execute();
        }

        public override string GetTypeString() => nameof(Func_Compare);

        public override Dictionary<string, string> GetParaDict()
        {
            Dictionary<string, string> result = new Dictionary<string, string>
            {
                { "Num1", GetData(1) },
                { "Operator", GetData(2) },
                { "Num2", GetData(3) },
                { "Result", GetData(4) }
            };
            return result;
        }

        public override void LoadParaDict(string version, Dictionary<string, string> paraDict)
        {
            try
            {
                SetData(1, paraDict["Num1"]);
                SetData(2, paraDict["Operator"]);
                SetData(3, paraDict["Num2"]);
                SetData(4, paraDict["Result"]);
            }
            catch (Exception) { }
        }

        protected override NodeBase CloneNode() => new Func_Compare();
    }
}