using System.Windows.Media.Imaging;
using XLib.Base;

namespace XLib.WPFControl
{
    public class TreeItem : IComparable<TreeItem>
    {
        #region 构造方法

        public TreeItem(ITreeItem content) => _content = content;

        #endregion

        #region 属性

        /// <summary>所属树视图</summary>
        public TreeView? Tree { get; set; }

        /// <summary>父级</summary>
        public TreeItem? Parent { get; set; } = null;

        /// <summary>子项</summary>
        public List<TreeItem> ItemList { get; set; } = new List<TreeItem>();

        /// <summary>索引</summary>
        public int Index => Parent == null ? -1 : Parent.ItemList.IndexOf(this);

        /// <summary>深度</summary>
        public int Deep => Parent == null ? -1 : Parent.Deep + 1;

        /// <summary>已展开</summary>
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                if (_isExpanded) ItemExpanded?.Invoke(this);
                else ItemCollapsed?.Invoke(this);
            }
        }

        /// <summary>图标</summary>
        public BitmapImage? Icon { get; set; } = null;

        /// <summary>内容</summary>
        public ITreeItem Content => _content;

        /// <summary>是否为文件夹</summary>
        public bool IsFolder => Content.IsFolder;

        /// <summary>已选中</summary>
        public bool Selected
        {
            get => _selected;
            set => SetSelected(value, true);
        }

        #endregion

        #region 事件

        /// <summary>子项列表已改变</summary>
        public Action<TreeItem>? ItemListChanged { get; set; } = null;

        /// <summary>项展开</summary>
        public Action<TreeItem>? ItemExpanded { get; set; } = null;

        /// <summary>项折叠</summary>
        public Action<TreeItem>? ItemCollapsed { get; set; } = null;

        #endregion

        #region “IComparable”方法

        public int CompareTo(TreeItem? other) => NaturalComparator.Compare(ToString(), other.ToString());

        #endregion

        #region 公开方法

        public override string ToString() => Content.ToString();

        /// <summary>
        /// 重命名
        /// </summary>
        public void Rename(string newName)
        {
            Content.Rename(newName);
            Parent?.SortItemList();
        }

        /// <summary>
        /// 设置是否选中
        /// </summary>
        public void SetSelected(bool value, bool notify = false)
        {
            if (_selected == value) return;
            _selected = value;
            // 添加选中项
            if (_selected) Tree.AddSelectedItem(this, notify);
            // 移除选中项
            else Tree.RemoveSelectedItem(this, notify);
        }

        /// <summary>
        /// 添加项
        /// </summary>
        public void AddItem(TreeItem item)
        {
            // 添加项并排序
            ItemList.Add(item);
            item.Parent = this;
            item.Tree = Tree;
            SortItemList();
            ItemListChanged?.Invoke(this);
        }

        /// <summary>
        /// 移除项
        /// </summary>
        public void RemoveItem(TreeItem item)
        {
            item.Parent = null;
            ItemList.Remove(item);
            ItemListChanged?.Invoke(this);
        }

        /// <summary>
        /// 清空项
        /// </summary>
        public void ClearItem()
        {
            foreach (var item in ItemList) item.Parent = null;
            ItemList.Clear();
            ItemListChanged?.Invoke(this);
        }

        /// <summary>
        /// 能否移动至
        /// </summary>
        public bool CanMoveTo(TreeItem target, out string reason)
        {
            bool result = Content.CanMoveTo(target.Content, out reason);
            return result;
        }

        /// <summary>
        /// 移动至目标项
        /// </summary>
        public void MoveTo(TreeItem target)
        {
            // 排序
            target.SortItemList();
            // 通知内容移动
            Content.NotifyMoveTo(target.Content);
        }

        /// <summary>
        /// 排序子项
        /// </summary>
        public void SortItemList()
        {
            if (ItemList.Count < 2) return;

            // 筛选文件夹项与文件项
            List<TreeItem> folderItemList = new List<TreeItem>();
            List<TreeItem> fileItemList = new List<TreeItem>();
            foreach (var item in ItemList)
            {
                if (item.IsFolder) folderItemList.Add(item);
                else fileItemList.Add(item);
            }
            // 排序文件夹项与文件项
            folderItemList.Sort();
            fileItemList.Sort();
            // 清空项
            ItemList.Clear();
            // 添加排序后的项
            foreach (var folderItem in folderItemList) ItemList.Add(folderItem);
            foreach (var fileItem in fileItemList) ItemList.Add(fileItem);
        }

        /// <summary>
        /// 获取完整路径
        /// </summary>
        public string GetFullPath()
        {
            if (Parent != null) return Parent.GetFullPath() + $"\\{Content}";
            return Content != null ? Content.ToString() : "";
        }

        #endregion

        #region 字段

        private readonly ITreeItem _content;
        private bool _selected = false;
        private bool _isExpanded = false;

        #endregion
    }
}