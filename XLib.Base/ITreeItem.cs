namespace XLib.Base
{
    public interface ITreeItem
    {
        #region 属性

        /// <summary>是否为文件夹</summary>
        public bool IsFolder { get; }

        #endregion

        #region 方法

        /// <summary>
        /// 重命名
        /// </summary>
        public void Rename(string newName);

        /// <summary>
        /// 能否移动至
        /// </summary>
        public bool CanMoveTo(ITreeItem target, out string reason);

        /// <summary>
        /// 通知移动至
        /// </summary>
        public void NotifyMoveTo(ITreeItem target);

        #endregion
    }
}