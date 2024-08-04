namespace XLib.Base
{
    /// <summary>
    /// 管理器接口
    /// </summary>
    public interface IManager
    {
        /// <summary>初始化</summary>
        void Init();

        /// <summary>重置</summary>
        void Reset();

        /// <summary>清理</summary>
        void Clear();
    }
}