namespace XLib.Base
{
    /// <summary>
    /// 泛型事件管理器
    /// </summary>
    /// <typeparam name="T">事件类型</typeparam>
    public class EM<T> : IManager where T : Enum
    {
        #region 生命周期

        public void Init()
        {
            foreach (T eventType in Enum.GetValues(typeof(T)))
                _listenerDict.Add(eventType, new List<Delegate>());
            _inited = true;
        }

        public void Reset() { }

        public void Clear() { }

        #endregion

        #region 添加监听器

        public void Add(T eventType, Action listener)
        {
            if (_inited)
                _listenerDict[eventType].Add(listener);
        }

        public void Add<T1>(T eventType, Action<T1> listener)
        {
            if (_inited)
                _listenerDict[eventType].Add(listener);
        }

        public void Add<T1, T2>(T eventType, Action<T1, T2> listener)
        {
            if (_inited)
                _listenerDict[eventType].Add(listener);
        }

        public void Add<T1, T2, T3>(T eventType, Action<T1, T2, T3> listener)
        {
            if (_inited)
                _listenerDict[eventType].Add(listener);
        }

        #endregion

        #region 移除监听器

        public void Remove(T eventType, Action listener)
        {
            if (_inited)
                _listenerDict[eventType].Remove(listener);
        }

        public void Remove<T1>(T eventType, Action<T1> listener)
        {
            if (_inited)
                _listenerDict[eventType].Remove(listener);
        }

        public void Remove<T1, T2>(T eventType, Action<T1, T2> listener)
        {
            if (_inited)
                _listenerDict[eventType].Remove(listener);
        }

        public void Remove<T1, T2, T3>(T eventType, Action<T1, T2, T3> listener)
        {
            if (_inited)
                _listenerDict[eventType].Remove(listener);
        }

        #endregion

        #region 清空监听器

        /// <summary>
        /// 清空指定事件类型的监听器
        /// </summary>
        public void Clear(T eventType)
        {
            if (_inited)
                _listenerDict[eventType].Clear();
        }

        /// <summary>
        /// 清空所有事件类型的监听器
        /// </summary>
        public void ClearAll()
        {
            foreach (var item in _listenerDict) item.Value.Clear();
        }

        #endregion

        #region 引发事件

        protected void InnerInvoke(T eventType)
        {
            if (_inited)
                foreach (var item in _listenerDict[eventType])
                    if (item is Action listener) listener?.Invoke();
        }

        protected void InnerInvoke<T1>(T eventType, T1 value1)
        {
            if (_inited)
                foreach (var item in _listenerDict[eventType])
                    if (item is Action<T1> listener) listener?.Invoke(value1);
        }

        protected void InnerInvoke<T1, T2>(T eventType, T1 value1, T2 value2)
        {
            if (_inited)
                foreach (var item in _listenerDict[eventType])
                    if (item is Action<T1, T2> listener) listener?.Invoke(value1, value2);
        }

        protected void InnerInvoke<T1, T2, T3>(T eventType, T1 value1, T2 value2, T3 value3)
        {
            if (_inited)
                foreach (var item in _listenerDict[eventType])
                    if (item is Action<T1, T2, T3> listener) listener?.Invoke(value1, value2, value3);
        }

        #endregion

        #region 字段

        /// <summary>已初始化</summary>
        private bool _inited = false;
        /// <summary>监听器表：事件类型 - 监听器列表</summary>
        private readonly Dictionary<T, List<Delegate>> _listenerDict = new Dictionary<T, List<Delegate>>();

        #endregion
    }
}