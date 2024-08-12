using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using XLib.Base.ArchiveFrame;
using XNode.SubSystem.ArchiveSystem.Define.Data_1_0;
using XNode.SubSystem.ArchiveSystem.Loader;
using XNode.SubSystem.WindowSystem;

namespace XNode.SubSystem.ArchiveSystem
{
    /// <summary>
    /// 存档管理器
    /// </summary>
    public class ArchiveManager
    {
        #region 单例

        private ArchiveManager() { }
        public static ArchiveManager Instance { get; } = new ArchiveManager();

        #endregion

        #region 属性

        /// <summary>当前版本</summary>
        public string CurrentVersion { get; private set; } = "1.0";

        #endregion

        #region 公开方法

        /// <summary>
        /// 生成存档
        /// </summary>
        public ArchiveFile GenerateArchive()
        {
            return new ArchiveFile
            {
                Version = CurrentVersion,
                Data = Extracter.Extract(),
            };
        }

        /// <summary>
        /// 读取存档文件
        /// </summary>
        public ArchiveFile? ReadArchiveFile(string filePath)
        {
            string jsonData = File.ReadAllText(filePath, Encoding.UTF8);
            return JsonConvert.DeserializeObject<ArchiveFile>(jsonData);
        }

        /// <summary>
        /// 加载存档
        /// </summary>
        public bool LoadArchive(ArchiveFile file, string path, JObject originData)
        {
            // 检查存档版本
            if (!CheckVersion(file))
            {
                WM.ShowError($"读取存档“{path}”失败：无效的版本");
                return false;
            }

            // 比较版本，过新则不加载
            int result = CompareVersion(file.Version);
            if (result < 0)
            {
                WM.ShowTip("存档版本过新，请升级软件后重试");
                return false;
            }

            // 导入存档数据
            if (!ImportArchiveData(file, originData, path))
            {
                WM.ShowError($"存档“{path}”加载失败");
                return false;
            }

            return true;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 检查版本
        /// </summary>
        private bool CheckVersion(ArchiveFile file)
        {
            if (string.IsNullOrEmpty(file.Version) || file.Version == "???") return false;
            try
            {
                ArchiveVersion version = new ArchiveVersion(file.Version);
            }
            catch (Exception) { return false; }
            return true;
        }

        /// <summary>
        /// 比较版本
        /// </summary>
        private int CompareVersion(string version)
        {
            ArchiveVersion file = new ArchiveVersion(version);
            ArchiveVersion current = new ArchiveVersion(CurrentVersion);
            return file.CompareTo(current);
        }

        /// <summary>
        /// 导入存档数据
        /// </summary>
        private bool ImportArchiveData(ArchiveFile file, JObject originData, string archiveFilePath)
        {
            try
            {
                switch (file.Version)
                {
                    case "1.0":
                        Data_1_0? data_1_0 = originData.ToObject<Data_1_0>();
                        if (data_1_0 == null) return false;
                        if (!Loader_1_0.Import(data_1_0, archiveFilePath)) return false;
                        break;
                    default:
                        return false;
                }

                return true;
            }
            catch (Exception) { return false; }
        }

        #endregion
    }
}