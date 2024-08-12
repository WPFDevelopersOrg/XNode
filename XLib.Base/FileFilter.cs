using XLib.Base.Ex;

namespace XLib.Base
{
    /// <summary>
    /// 类型信息
    /// </summary>
    public class TypeInfo
    {
        public TypeInfo(string name, string extension)
        {
            Name = name;
            Extension = extension;
        }

        /// <summary>类型名</summary>
        public string Name { get; set; } = "文件";

        /// <summary>扩展名</summary>
        public string Extension { get; set; } = "*";

        public override string ToString() => $"{Name}(*.{Extension})|*.{Extension}";
    }

    /// <summary>
    /// 文件过滤器
    /// </summary>
    public class FileFilter
    {
        public List<TypeInfo> TypeList { get; set; } = new List<TypeInfo>();

        public override string ToString() => TypeList.ToListString("|");
    }
}