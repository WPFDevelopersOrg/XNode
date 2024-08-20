namespace XLib.Node
{
    /// <summary>
    /// 进度获取器
    /// </summary>
    public interface IProgressGetter
    {
        /// <summary>
        /// 获取进度
        /// </summary>
        public double GetProgress();
    }
}