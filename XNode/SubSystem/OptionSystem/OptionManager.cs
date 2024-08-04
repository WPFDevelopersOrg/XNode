using System.IO;
using XLib.Base;

namespace XNode.SubSystem.OptionSystem
{
    /// <summary>
    /// 选项管理器
    /// </summary>
    public class OptionManager : IManager
    {
        #region 单例

        private OptionManager() { }
        public static OptionManager Instance { get; } = new OptionManager();

        #endregion

        #region 属性

        /// <summary>缓存路径</summary>
        public string CachePath => _root + "Cache\\";

        /// <summary>项目路径</summary>
        public string ProjectPath => _root + "Project\\";

        /// <summary>节点库路径</summary>
        public string NodeLibPath => _root + "NodeLib\\";

        #endregion

        #region “IManager”方法

        public void Init()
        {
            if (!Directory.Exists(CachePath)) Directory.CreateDirectory(CachePath);
            if (!Directory.Exists(ProjectPath)) Directory.CreateDirectory(ProjectPath);
            if (!Directory.Exists(NodeLibPath)) Directory.CreateDirectory(NodeLibPath);
        }

        public void Reset() { }

        public void Clear() { }

        #endregion

        #region 字段

        /// <summary>根路径</summary>
        private readonly string _root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\XNode\\";

        #endregion
    }
}