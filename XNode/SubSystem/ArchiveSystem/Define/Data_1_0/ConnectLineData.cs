namespace XNode.SubSystem.ArchiveSystem.Define.Data_1_0
{
    /// <summary>
    /// 连接线数据
    /// </summary>
    public class ConnectLineData
    {
        public string Start { get; set; } = "";

        public string End { get; set; } = "";

        public override string ToString() => $"{Start}-{End}";
    }
}