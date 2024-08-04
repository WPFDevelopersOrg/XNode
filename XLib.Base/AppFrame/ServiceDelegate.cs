namespace XLib.Base.AppFrame
{
    /// <summary>
    /// 服务代理
    /// </summary>
    public class ServiceDelegate
    {
        public List<ServiceBase> ServiceList { get; private set; } = new List<ServiceBase>();

        public void Start()
        {
            foreach (var service in ServiceList) service.Start();
        }

        public void Stop()
        {
            foreach (var service in ServiceList) service.Stop();
        }
    }
}