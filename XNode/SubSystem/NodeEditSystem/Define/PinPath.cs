namespace XNode.SubSystem.NodeEditSystem.Define
{
    /// <summary>
    /// 引脚路径：节点编号.引脚组索引.引脚索引
    /// </summary>
    public class PinPath
    {
        /// <summary>
        /// 解析引脚路径
        /// </summary>
        public static PinPath ParsePinPath(string path)
        {
            string[] nodeArray = path.Split(',');
            return new PinPath
            {
                NodeVersion = nodeArray[0],
                NodeID = int.Parse(nodeArray[1]),
                GroupIndex = int.Parse(nodeArray[2]),
                PinIndex = int.Parse(nodeArray[3])
            };
        }

        public string NodeVersion { get; set; } = "1.0";

        public int NodeID { get; set; } = -1;

        public int GroupIndex { get; set; } = -1;

        public int PinIndex { get; set; } = -1;

        public override string ToString() => $"{NodeVersion},{NodeID},{GroupIndex},{PinIndex}";
    }
}