using Newtonsoft.Json;
using System.IO;
using XLib.Base;
using XNode.SubSystem.OptionSystem;

namespace XNode.SubSystem.CacheSystem
{
    /// <summary>
    /// 缓存管理器
    /// </summary>
    public class CacheManager : IManager
    {
        #region 单例

        private CacheManager() { }
        public static CacheManager Instance { get; } = new CacheManager();

        #endregion

        #region 属性

        public string Name { get; set; } = "缓存管理器";

        /// <summary>缓存数据</summary>
        public CacheData Cache { get; set; } = new CacheData();

        #endregion

        #region “IManager”方法

        public void Init()
        {
            // 不存在缓存，则新建缓存
            if (!File.Exists($"{OptionManager.Instance.CachePath}\\{_cacheFileName}")) SaveCacheFile();
            // 加载缓存文件
            LoadCacheFile();
        }

        public void Reset() { }

        public void Clear() { }

        #endregion

        #region 公开方法

        /// <summary>
        /// 更新缓存
        /// </summary>
        public void UpdateCache() => SaveCacheFile();

        #endregion

        #region 私有方法

        /// <summary>
        /// 保存缓存文件
        /// </summary>
        private void SaveCacheFile()
        {
            string filePath = $"{OptionManager.Instance.CachePath}\\{_cacheFileName}";
            File.WriteAllText(filePath, JsonConvert.SerializeObject(Cache, Formatting.Indented));
        }

        /// <summary>
        /// 加载缓存文件
        /// </summary>
        private void LoadCacheFile()
        {
            string jsonData = File.ReadAllText($"{OptionManager.Instance.CachePath}\\{_cacheFileName}");
            CacheData? cache = JsonConvert.DeserializeObject<CacheData>(jsonData);
            if (cache != null) Cache = cache;
        }

        #endregion

        #region 字段

        /// <summary>缓存文件名</summary>
        private readonly string _cacheFileName = "Cache.json";

        #endregion
    }
}