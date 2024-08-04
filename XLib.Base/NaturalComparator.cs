using System.Runtime.InteropServices;

namespace XLib.Base
{
    /// <summary>
    /// 自然比较器
    /// </summary>
    public class NaturalComparator
    {
        /// <summary>
        /// 操作系统自带的文件名排序函数
        /// </summary>
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int StrCmpLogicalW(string? x, string? y);

        public static int Compare(string? x, string? y) => StrCmpLogicalW(x, y);
    }
}