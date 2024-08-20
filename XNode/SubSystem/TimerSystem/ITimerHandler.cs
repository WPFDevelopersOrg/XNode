namespace XNode.SubSystem.TimerSystem
{
    public interface ITimerHandler
    {
        void Tick();
    }

    /// <summary>
    /// 通用定时器处理器
    /// </summary>
    public class TimerHandler : ITimerHandler
    {
        public Action? OnTick { get; set; } = null;

        public void Tick() => OnTick?.Invoke();
    }
}