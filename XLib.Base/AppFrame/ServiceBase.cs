namespace XLib.Base.AppFrame
{
    /// <summary>
    /// 服务基类
    /// </summary>
    public abstract class ServiceBase
    {
        public abstract void Start();

        public abstract void Stop();

        public void Restart()
        {
            Stop();
            Start();
        }
    }
}