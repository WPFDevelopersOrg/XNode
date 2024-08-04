namespace XLib.Base.VirtualDisk
{
    /// <summary>
    /// 文件类型管理器
    /// </summary>
    public class FileTypeManager
    {
        #region 单例

        private FileTypeManager() { }
        public static FileTypeManager Instance { get; } = new FileTypeManager();

        #endregion

        #region 公开方法

        /// <summary>
        /// 注册文件类型
        /// </summary>
        public void RegisterFileType(string extension, string icon, string typeName)
        {
            if (extension == "")
                throw new Exception("扩展名不能为空");
            if (_fileTypeDict.ContainsKey(extension))
                throw new Exception("已注册扩展名：" + extension);

            _fileTypeDict.Add(extension, new FileType()
            {
                Extension = extension,
                IconName = icon,
                TypeName = typeName
            });
        }

        /// <summary>
        /// 获取文件类型
        /// </summary>
        public FileType GetFileType(string extension)
        {
            if (extension == "")
                throw new Exception("扩展名不能为空");
            if (!_fileTypeDict.ContainsKey(extension))
                throw new Exception("未注册扩展名：" + extension);

            return _fileTypeDict[extension];
        }

        #endregion

        #region 字段

        /// <summary>文件类型表</summary>
        private readonly Dictionary<string, FileType> _fileTypeDict = new Dictionary<string, FileType>();

        #endregion
    }
}