namespace XLib.Base.ArchiveFrame
{
    /// <summary>
    /// 存档文件
    /// </summary>
    public class ArchiveFile
    {
        /// <summary>存档版本</summary>
        public string Version { get; set; } = "1.0";

        /// <summary>存档数据</summary>
        public object? Data { get; set; } = null;
    }
}