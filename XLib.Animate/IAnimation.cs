namespace XLib.Animate
{
    /// <summary>
    /// 表示一个动画对象
    /// </summary>
    public interface IAnimation
    {
        /// <summary>动画已完成</summary>
        public event Action<IAnimation> Finished;

        /// <summary>
        /// 获取总时长
        /// </summary>
        public double GetTotalLength();

        /// <summary>
        /// 驱动至指定毫秒
        /// </summary>
        public void Drive(double time);

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop();
    }
}