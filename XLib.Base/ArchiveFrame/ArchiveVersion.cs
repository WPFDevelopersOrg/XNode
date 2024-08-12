namespace XLib.Base.ArchiveFrame
{
    /// <summary>
    /// 存档版本
    /// </summary>
    public class ArchiveVersion
    {
        public ArchiveVersion(string version)
        {
            string[] versionInfo = version.Split('.');
            Main = int.Parse(versionInfo[0]);
            Sub = int.Parse(versionInfo[1]);
        }

        public int Main { get; set; } = 1;

        public int Sub { get; set; } = 0;

        public string VersionString => $"{Main}.{Sub}";

        public int CompareTo(ArchiveVersion version)
        {
            if (version.Main != Main) return version.Main.CompareTo(Main);
            if (version.Sub != Sub) return version.Sub.CompareTo(Sub);
            return 0;
        }
    }
}