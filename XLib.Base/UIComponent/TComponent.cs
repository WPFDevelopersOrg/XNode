namespace XLib.Base.UIComponent
{
    public class Component<THost> where THost : class
    {
        #region 属性

        /// <summary>组件箱</summary>
        public ComponentBox<THost> Box
        {
            get => _box;
            set => _box ??= value;
        }

        /// <summary>宿主</summary>
        public THost Host
        {
            get => _host;
            set => _host ??= value;
        }

        /// <summary>名称</summary>
        public string Name { get; set; } = "未命名组件";

        /// <summary>已启用</summary>
        public bool IsEnabled { get; set; } = false;

        #endregion

        #region 生命周期方法

        /// <summary>
        /// 请求初始化
        /// </summary>
        internal void ReqInit()
        {
            foreach (var component in _subComponentList) component.ReqInit();
            Init();
        }

        /// <summary>
        /// 请求启用
        /// </summary>
        public void ReqEnable()
        {
            if (IsEnabled) return;
            foreach (var component in _subComponentList) component.ReqEnable();
            Enable();
            IsEnabled = true;
        }

        /// <summary>
        /// 请求重置
        /// </summary>
        public void ReqReset()
        {
            foreach (var component in _subComponentList) component.ReqReset();
            Reset();
        }

        /// <summary>
        /// 请求禁用
        /// </summary>
        public void ReqDisable()
        {
            if (!IsEnabled) return;
            foreach (var component in _subComponentList) component.ReqDisable();
            Disable();
            IsEnabled = false;
        }

        /// <summary>
        /// 请求移除
        /// </summary>
        internal void ReqRemove()
        {
            // 请求移除子组件
            foreach (var component in _subComponentList) component.ReqRemove();
            // 清空子组件引用
            _subComponentList.Clear();
            Remove();
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 添加组件
        /// </summary>
        public void AddComponent(Component<THost> component) => _subComponentList.Add(component);

        #endregion

        #region 生命周期

        /// <summary>添加全部组件后调用：初始化此组件。只调用一次</summary>
        protected virtual void Init() { }

        /// <summary>请求启用时调用：启用组件功能</summary>
        protected virtual void Enable() { }

        /// <summary>请求重置时调用：恢复至启用时状态</summary>
        protected virtual void Reset() { }

        /// <summary>请求禁用时调用：恢复至启用前状态</summary>
        protected virtual void Disable() { }

        /// <summary>移除全部组件前调用：恢复至启用前状态，然后执行一些清理工作。只调用一次</summary>
        protected virtual void Remove() { }

        #endregion

        #region 内部方法

        /// <summary>
        /// 获取组件
        /// </summary>
        protected TComponent GetComponent<TComponent>() where TComponent : Component<THost>
        {
            return _box.GetComponent<TComponent>();
        }

        #endregion

        #region 字段

        /// <summary>子组件列表</summary>
        private readonly List<Component<THost>> _subComponentList = new List<Component<THost>>();

        #endregion

        #region 属性字段

        private ComponentBox<THost> _box;
        protected THost _host;

        #endregion
    }

    public class Component<THost, TEvent> : Component<THost>
        where THost : class
        where TEvent : Enum
    {
        public Component() => _lem.Init();

        public void AddEvent(TEvent eventType, Action action) => _lem.Add(eventType, action);

        public void AddEvent<T>(TEvent eventType, Action<T> action) => _lem.Add(eventType, action);

        public void RemoveEvent(TEvent eventType, Action action) => _lem.Remove(eventType, action);

        public void RemoveEvent<T>(TEvent eventType, Action<T> action) => _lem.Remove(eventType, action);

        protected void Invoke(TEvent eventType) => _lem.Invoke(eventType);

        protected void Invoke<T>(TEvent eventType, T value) => _lem.Invoke(eventType, value);

        private class LocalEM : EM<TEvent>
        {
            public void Invoke(TEvent eventType) => InnerInvoke(eventType);

            public void Invoke<T>(TEvent eventType, T value) => InnerInvoke(eventType, value);
        }

        /// <summary>局域事件管理器</summary>
        private readonly LocalEM _lem = new LocalEM();
    }
}