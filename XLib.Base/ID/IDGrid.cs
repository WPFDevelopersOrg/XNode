namespace XLib.Base.ID
{
    /// <summary>
    /// 编号格子
    /// </summary>
    public class IDGrid
    {
        public int ID { get; set; } = -1;

        public bool Used { get; set; } = false;

        public override string ToString() => $"{ID:00}{(Used ? " Used" : "")}";
    }
}