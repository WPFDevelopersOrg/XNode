using System.Diagnostics;

namespace XLib.Animate
{
    /// <summary>
    /// 动画组：用于同时驱动一组动画
    /// </summary>
    public class AnimationGroup : IAnimation
    {
        #region 属性、事件

        public List<IAnimation> AnimationList { get; set; } = new List<IAnimation>();

        /// <summary>动画已完成</summary>
        public event Action<IAnimation> Finished;

        #endregion

        #region IDriveable 方法

        public double GetTotalLength()
        {
            double total = 0;
            foreach (var animation in AnimationList)
            {
                double length = animation.GetTotalLength();
                if (length > total) total = length;
            }
            return total;
        }

        public void Drive(double time)
        {
            foreach (var animation in AnimationList) animation.Drive(time);
        }

        public void Stop()
        {
            foreach (var animation in AnimationList) animation.Stop();
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 添加动画
        /// </summary>
        public void Add(IAnimation animation)
        {
            // 监听动画完成
            animation.Finished += Animation_Finished;
            // 添加到动画组
            AnimationList.Add(animation);
        }
        
        #endregion

        #region 私有方法

        private void Animation_Finished(IAnimation driveable)
        {
            _finishedCount++;
            // 全部完成时，调用动画组完成
            if (_finishedCount == AnimationList.Count) Finished?.Invoke(this);
        }

        #endregion

        private int _finishedCount = 0;
    }
}