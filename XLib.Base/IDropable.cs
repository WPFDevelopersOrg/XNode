namespace XLib.Base
{
    public interface IDropable
    {
        /// <summary>
        /// 拖动
        /// </summary>
        public void OnDrag(List<ITreeItem> fileList);

        /// <summary>
        /// 放下
        /// </summary>
        public void OnDrop(List<ITreeItem> fileList);

        /// <summary>
        /// 能否放下
        /// </summary>
        public bool CanDrop(List<ITreeItem> fileList);
    }
}