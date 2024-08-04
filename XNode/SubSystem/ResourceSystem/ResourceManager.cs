using XLib.Base;

namespace XNode.SubSystem.ResourceSystem
{
    public class ResourceManager : IManager
    {
        #region 单例

        private ResourceManager() { }
        public static ResourceManager Instance { get; } = new ResourceManager();

        #endregion

        #region 属性

        public string Name { get; set; } = "资源管理器";

        #endregion

        #region 接口实现

        public void Init()
        {
            // 光标管理器
            CursorManager.Instance.Init();
            // 引脚图标管理器
            PinIconManager.Instance.Init();
        }

        public void Reset() { }

        public void Clear() { }

        #endregion
    }
}