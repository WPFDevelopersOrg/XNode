namespace XNode.SubSystem.ArchiveSystem.Define.Data_1_0
{
    /// <summary>
    /// 节点数据
    /// </summary>
    public class NodeData
    {
        /// <summary>基本数据</summary>
        public string BaseData { get; set; } = "";

        /// <summary>参数表</summary>
        public Dictionary<string, string> ParaDict { get; set; } = new Dictionary<string, string>();

        /// <summary>属性表</summary>
        public Dictionary<string, string> PropertyDict { get; set; } = new Dictionary<string, string>();
    }
}