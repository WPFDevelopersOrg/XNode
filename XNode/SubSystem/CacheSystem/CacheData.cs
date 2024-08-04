using System.Windows;

namespace XNode.SubSystem.CacheSystem
{
    public class MainWindowCache
    {
        public WindowState State { get; set; } = WindowState.Normal;
        public int Width { get; set; } = 1280;
        public int Height { get; set; } = 720;
    }

    /// <summary>
    /// 节点库缓存
    /// </summary>
    public class NodeLibCache
    {
        public List<string> ExpandedFolderList { get; set; } = new List<string>();

        /// <summary>
        /// 判断指定的文件夹是否处于展开状态
        /// </summary>
        public bool IsExpanded(string folderPath) => ExpandedFolderList.Contains(folderPath);

        /// <summary>
        /// 折叠指定文件夹
        /// </summary>
        public void Fold(string folderPath)
        {
            if (ExpandedFolderList.Contains(folderPath)) ExpandedFolderList.Remove(folderPath);
            CacheManager.Instance.UpdateCache();
        }

        /// <summary>
        /// 展开指定文件夹
        /// </summary>
        public void Expand(string folderPath)
        {
            if (!ExpandedFolderList.Contains(folderPath)) ExpandedFolderList.Add(folderPath);
            CacheManager.Instance.UpdateCache();
        }
    }

    public class CacheData
    {
        public MainWindowCache MainWindow { get; set; } = new MainWindowCache();

        public NodeLibCache NodeLib { get; set; } = new NodeLibCache();
    }
}