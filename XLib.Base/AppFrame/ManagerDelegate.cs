namespace XLib.Base.AppFrame
{
    /// <summary>
    /// 管理器代理
    /// </summary>
    public class ManagerDelegate
    {
        public List<IManager> ManagerList { get; private set; } = new List<IManager>();

        public virtual void Init()
        {
            foreach (var manager in ManagerList) manager.Init();
            InitFinish();
        }

        public virtual void Clear()
        {
            foreach (var manager in ManagerList) manager.Clear();
        }

        protected virtual void InitFinish() { }
    }
}