namespace XLib.Animate
{
    /// <summary>
    /// 动画延迟
    /// </summary>
    public class AnimationDelay : IAnimation
    {
        public double Duration { get; set; } = 0;

        public event Action<IAnimation> Finished;

        public void Drive(double time)
        {
            if (time >= Duration) Finished?.Invoke(this);
        }

        public double GetTotalLength() => Duration;

        public void Stop() => Finished?.Invoke(this);
    }
}