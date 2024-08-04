namespace XLib.WPF.Behavior
{
    /// <summary>
    /// 行为处理器
    /// </summary>
    public class BehaviorHandler
    {
        /// <summary>当前行为树</summary>
        public BehaviorTree? CurrentTree => _currentTree;

        /// <summary>行为树列表</summary>
        public List<BehaviorTree> TreeList { get; set; } = new List<BehaviorTree>();

        /// <summary>异常处理器</summary>
        public static Action<Exception>? ExceptionHandler { get; set; } = null;

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            foreach (var tree in TreeList) tree.BackToRoot();
            _currentTree = null;
        }

        /// <summary>
        /// 新建行为树
        /// </summary>
        public BehaviorNode NewBehaviorTree(string name, Action<BehaviorArgs?>? action)
        {
            var tree = new BehaviorTree(name, action);
            TreeList.Add(tree);
            _currentTree = tree;
            return tree.Root;
        }

        /// <summary>
        /// 添加行为节点
        /// </summary>
        public BehaviorNode AddBehaviorNode(string name, Action<BehaviorArgs?>? action)
        {
            if (_currentTree == null) throw new Exception("请先添加行为树");
            return _currentTree.AddNode(name, action);
        }

        /// <summary>
        /// 回到根节点
        /// </summary>
        public void BackToRoot() => _currentTree?.BackToRoot();

        /// <summary>
        /// 完成添加
        /// </summary>
        public void FnishAdd()
        {
            _currentTree?.BackToRoot();
            _currentTree = null;
        }

        /// <summary>
        /// 引发行为
        /// </summary>
        public void Invoke(string name, BehaviorArgs? args)
        {
            try
            {
                // 查找一棵匹配的行为树
                _currentTree ??= TreeList.Find(tree => tree.Name == name);
                // 引发行为
                _currentTree?.Invoke(name, args);
            }
            catch (Exception ex)
            {
                // 重置行为树
                Reset();
                // 处理异常
                ExceptionHandler?.Invoke(ex);
            }
        }

        public void HandleKeyDown(string key) => _currentTree?.HandleKeyDown(key);

        public void HandleKeyUp(string key) => _currentTree?.HandleKeyUp(key);

        public void HandleMouseWheel(int delta) => _currentTree?.HandleMouseWheel(delta);

        public void SetEnable(List<string> nodeList, bool enable)
        {
            if (nodeList.Count == 0) return;
            BehaviorTree? tree = TreeList.Find(item => item.Name == nodeList[0]);
            if (tree != null)
            {
                nodeList.RemoveAt(0);
                tree.SetEnable(nodeList, enable);
            }
        }

        /// <summary>当前行为树</summary>
        private BehaviorTree? _currentTree;
    }
}