namespace XNode.SubSystem.ArchiveSystem.Define.Data_1_0
{
    public class NodeBaseData
    {
        public NodeBaseData() { }

        public NodeBaseData(string data)
        {
            string[] array = data.Split('/');
            NodeLibName = array[0];
            TypeString = array[1];
            Version = array[2];
            ID = int.Parse(array[3]);
            Point = array[4];
        }

        /// <summary>节点库名称</summary>
        public string NodeLibName { get; set; } = "Inner";

        /// <summary>类型字符串</summary>
        public string TypeString { get; set; } = "";

        /// <summary>节点版本</summary>
        public string Version { get; set; } = "1.0";

        /// <summary>编号</summary>
        public int ID { get; set; } = -1;

        /// <summary>坐标</summary>
        public string Point { get; set; } = "0,0";

        public override string ToString() => $"{NodeLibName}/{TypeString}/{Version}/{ID}/{Point}";
    }
}