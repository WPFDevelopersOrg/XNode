using System.IO;
using System.Windows.Media.Imaging;

namespace XNode.SubSystem.ResourceSystem
{
    public class ImageResManager
    {
        #region 单例

        private ImageResManager() { }
        public static ImageResManager Instance { get; } = new ImageResManager();

        #endregion

        /// <summary>
        /// 获取图片
        /// </summary>
        public BitmapImage GetImage(string path)
        {
            if (!path.StartsWith("pack:") && !File.Exists(path))
                throw new Exception("图片不存在");

            // 已加载过此图片，直接返回
            if (_imageResDict.ContainsKey(path))
                return _imageResDict[path].CloneCurrentValue();

            // 创建图片实例
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            // 设置加载图片后释放文件
            image.CacheOption = BitmapCacheOption.OnLoad;
            // 设置图片源
            image.UriSource = new Uri(path);
            image.EndInit();
            // 保存图片引用
            _imageResDict.Add(path, image);

            // 返回图片实例
            return image;
        }

        /// <summary>
        /// 获取资源图片
        /// </summary>
        public BitmapImage GetAssetsImage(string path)
        {
            if (path == "") throw new Exception("路径不能为空");
            return GetImage($"pack://application:,,,/Assets/{path}");
        }

        /// <summary>
        /// 获取节点图标
        /// </summary>
        public BitmapImage GetNodeIcon(string iconName)
        {
            if (iconName == "") throw new Exception("图标名不能为空");
            return GetImage($"pack://application:,,,/Assets/Icon16/Node/{iconName}.png");
        }

        /// <summary>
        /// 获取小图标
        /// </summary>
        public BitmapImage? GetIcon15(string path)
        {
            if (path == "") throw new Exception("路径不能为空");
            return GetImage($"pack://application:,,,/Assets/Icon15/{path}");
        }

        /// <summary>
        /// 获取图片字体
        /// </summary>
        public BitmapImage GetImageFont(string path)
        {
            if (path == "") throw new Exception("路径不能为空");
            return GetAssetsImage($"Font/Number/{path}");
        }

        /// <summary>
        /// 获取子系统图片
        /// </summary>
        public BitmapImage? GetSubSystemImage(string subSystem, string imageName)
        {
            if (subSystem == "" || imageName == "") return null;
            return GetImage($"pack://application:,,,/SubSystem/{subSystem}/Image/{imageName}.png");
        }

        /// <summary>
        /// 获取子系统图片
        /// </summary>
        public BitmapImage? GetSubSystemImage(string subSystem, string subPath, string imageName)
        {
            if (subSystem == "" || subPath == "" || imageName == "") return null;
            return GetImage($"pack://application:,,,/SubSystem/{subSystem}/{subPath}/{imageName}.png");
        }

        private readonly Dictionary<string, BitmapImage> _imageResDict = new Dictionary<string, BitmapImage>();
    }
}