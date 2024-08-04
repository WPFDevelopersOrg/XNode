using XLib.Base.Ex;

namespace XLib.Base
{
    /// <summary>
    /// 表示树中某个节点的路径
    /// </summary>
    public class TreeNodePath
    {
        public TreeNodePath() { }

        public TreeNodePath(string path, string split = "/")
        {
            _split = split;
            if (path != "")
            {
                // 移除前后空格
                path = path.Trim();
                // 移除前后路径分割符
                if (path.StartsWith(_split)) path = path[1..];
                if (path.EndsWith(_split)) path = path[..^1];
                // 分割路径
                if (path.Contains(_split)) NodeList = path.Split(_split).ToList();
                else NodeList = new List<string> { path };
            }
        }

        /// <summary>路径分割符</summary>
        public string PathSplit { get => _split; set => _split = value; }

        /// <summary>节点列表</summary>
        public List<string> NodeList { get; set; } = new List<string>();

        public TreeNodePath Append(string node)
        {
            NodeList.Add(node);
            return this;
        }

        /// <summary>
        /// 移除第一个节点
        /// </summary>
        public void RemoveFirst() => NodeList.RemoveAt(0);

        /// <summary>
        /// 移除最后一个节点
        /// </summary>
        public void RemoveLast() => NodeList.RemoveLast();

        public override string ToString() => NodeList.ToListString(_split);

        private string _split = "/";
    }
}