namespace XLib.WPF.Behavior
{
    /// <summary>
    /// 行为树
    /// </summary>
    public class BehaviorTree
    {
        #region 构造方法

        public BehaviorTree(string name, Action<BehaviorArgs?>? action)
        {
            Name = name;
            Root = new BehaviorNode(name, action);
            _current = Root;
        }

        #endregion

        #region 属性

        public string Name { get; private set; } = "";

        public BehaviorNode Root { get; set; }

        #endregion

        #region 公开方法

        /// <summary>
        /// 转字符串
        /// </summary>
        public override string ToString() => Name;

        /// <summary>
        /// 添加节点
        /// </summary>
        public BehaviorNode AddNode(string name, Action<BehaviorArgs?>? action)
        {
            var node = new BehaviorNode(name, action);
            _current.SubNodeList.Add(node);
            _current = node;
            return node;
        }

        /// <summary>
        /// 回到根节点
        /// </summary>
        public void BackToRoot() => _current = Root;

        /// <summary>
        /// 引发行为
        /// </summary>
        public void Invoke(string name, BehaviorArgs? args)
        {
            // 如果当前节点能引发，则由当前节点引发
            if (_current.Name == name)
            {
                if (_current.Enabled)
                    _current.Action?.Invoke(args);
                else return;
            }
            // 否则，流转至下一个能引发的节点
            else
            {
                // 查找能引发行为的节点
                var nextNode = _current.FindSubNode(name);
                // 找到了，则引发；否则忽略
                if (nextNode != null && nextNode.Enabled)
                {
                    _current = nextNode;
                    _current.Action?.Invoke(args);
                }
            }
        }

        /// <summary>
        /// 处理键盘按下
        /// </summary>
        public void HandleKeyDown(string key)
        {
            if (!_current.Enabled) return;
            _current?.KeyDown?.Invoke(key);
        }

        /// <summary>
        /// 处理键盘松开
        /// </summary>
        public void HandleKeyUp(string key)
        {
            if (!_current.Enabled) return;
            _current?.KeyUp?.Invoke(key);
        }

        /// <summary>
        /// 处理鼠标滚轮
        /// </summary>
        public void HandleMouseWheel(int delta)
        {
            if (!_current.Enabled) return;
            _current?.MouseWheel?.Invoke(delta);
        }

        /// <summary>
        /// 设置启用
        /// </summary>
        public void SetEnable(List<string> nodeList, bool enable)
        {
            var node = FindNode(nodeList);
            if (node != null) node.Enabled = enable;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 查找节点
        /// </summary>
        private BehaviorNode? FindNode(List<string> nodeList)
        {
            if (nodeList.Count == 0) return null;
            return FindNode(Root, nodeList);
        }

        /// <summary>
        /// 查找节点
        /// </summary>
        private BehaviorNode? FindNode(BehaviorNode parent, List<string> nodeList)
        {
            if (nodeList.Count == 0) return parent;
            BehaviorNode? node = FindSubNode(parent, nodeList[0]);
            if (node != null)
            {
                nodeList.RemoveAt(0);
                return FindNode(node, nodeList);
            }
            return null;
        }

        /// <summary>
        /// 查找子节点
        /// </summary>
        private BehaviorNode? FindSubNode(BehaviorNode parent, string name)
        {
            foreach (var item in parent.SubNodeList)
                if (item.Name == name) return item;
            return null;
        }

        #endregion

        #region 字段

        /// <summary>当前节点</summary>
        private BehaviorNode _current;

        #endregion
    }
}