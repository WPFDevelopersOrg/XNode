using XLib.Node;

namespace XNode.SubSystem.NodeLibSystem.Define.Data
{
    public class Data_Double : NodeBase
    {
        public override void Init()
        {
            SetViewProperty(NodeColorSet.Data, "Variate", "小数");

            PinGroupList.Add(new DataPinGroup(this, "double", "数据", "0.0")
            {
                Readable = true,
                Writeable = false
            });

            InitPinGroup();
        }

        public override string GetTypeString() => nameof(Data_Double);

        public override Dictionary<string, string> GetParaDict()
        {
            Dictionary<string, string> result = new Dictionary<string, string>
            {
                { "Data", GetData(0) },
            };
            return result;
        }

        public override void LoadParaDict(string version, Dictionary<string, string> paraDict)
        {
            SetData(0, paraDict["Data"]);
        }

        protected override NodeBase CloneNode() => new Data_Double();
    }
}