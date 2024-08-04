using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Resources;
using XLib.Base;
using XLib.WPFControl.Tool;

namespace XLib.WPFControl
{
    /// <summary>
    /// 鼠标所处位置
    /// </summary>
    public enum MouseSide
    {
        /// <summary>内部</summary>
        InSide,
        /// <summary>外部</summary>
        OutSide,
    }

    public partial class TreeView : UserControl
    {
        #region 构造方法

        public TreeView() => InitializeComponent();

        #endregion

        #region 属性

        /// <summary>树根</summary>
        public TreeItem TreeRoot { get; private set; } = new TreeItem(null);

        /// <summary>选中项列表</summary>
        public List<TreeItem> SelectedItemList => _selectedItemList.ToList();

        #endregion

        #region 开关

        /// <summary>移动文件</summary>
        public bool MoveItem { get; set; } = true;

        /// <summary>多选</summary>
        public bool MultiSelect { get; set; } = true;

        /// <summary>只允许拖动文件</summary>
        public bool DragFileOnly { get; set; } = false;

        #endregion

        #region 事件

        /// <summary>显示错误提示</summary>
        public Action<string>? ShowError { get; set; } = null;

        /// <summary>选中项已改变</summary>
        public Action? SelectedChanged { get; set; } = null;

        /// <summary>添加选择</summary>
        public Action<ITreeItem>? AddSelect { get; set; } = null;

        /// <summary>移除选择</summary>
        public Action<ITreeItem>? RemoveSelect { get; set; } = null;

        /// <summary>清空选择</summary>
        public Action? ClearSelect { get; set; } = null;

        /// <summary>双击项</summary>
        public Action<ITreeItem>? ItemDoubleClick { get; set; } = null;

        /// <summary>选中文件夹</summary>
        public Action<ITreeItem>? FolderSelected { get; set; } = null;

        /// <summary>项已移动</summary>
        public Action ItemMoved { get; set; } = null;

        #endregion

        #region 光标

        /// <summary>选择</summary>
        public Cursor? Select { get; set; }

        /// <summary>移动选中项</summary>
        public Cursor? MoveSelected { get; set; }

        /// <summary>无法移动</summary>
        public Cursor? CanNotMove { get; set; }

        /// <summary>拖动对象</summary>
        public Cursor? DragObject { get; set; }

        #endregion

        #region 公开方法

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            Select = LoadCursor("Assets/Cursor/Select.cur");
            MoveSelected = LoadCursor("Assets/Cursor/MoveSelected.cur");
            CanNotMove = LoadCursor("Assets/Cursor/Disable.cur");
            DragObject = LoadCursor("Assets/Cursor/DragObject.cur");
            MatchControlList();
            SizeChanged += TreeView_SizeChanged;
            _selectTool = new SelectTool(this);
            _selectTool.Init();
        }

        /// <summary>
        /// 设置树根
        /// </summary>
        public void SetTreeRoot(TreeItem root)
        {
            TreeRoot = root;
            TreeRoot.Tree = this;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            // 更新可视项
            UpdateDataSource();
            // 更新视图
            UpdateVisibleView();
            // 更新滚动条
            UpdateScrollBar();
        }

        /// <summary>
        /// 根据路径查找树项
        /// </summary>
        public TreeItem? FindItemByPath(List<string> path)
        {
            if (path.Count == 0) return null;
            return FindTreeItem(null, path);
        }

        /// <summary>
        /// 添加选中项
        /// </summary>
        public void AddSelectedItem(TreeItem treeItem, bool notify = false)
        {
            if (!_selectedItemList.Contains(treeItem))
            {
                _selectedItemList.Add(treeItem);
                if (_selectedItemList.Count == 1) _firstSelectedItem = treeItem;
                if (notify)
                {
                    AddSelect?.Invoke(treeItem.Content);
                    SelectedChanged?.Invoke();
                    if (treeItem.IsFolder) FolderSelected?.Invoke(treeItem.Content);
                }
                UpdateVisibleView();
            }
        }

        /// <summary>
        /// 移除选中项
        /// </summary>
        public void RemoveSelectedItem(TreeItem treeItem, bool notify = false)
        {
            if (_selectedItemList.Remove(treeItem))
            {
                if (_selectedItemList.Count == 0) _firstSelectedItem = null;
                if (notify)
                {
                    RemoveSelect?.Invoke(treeItem.Content);
                    SelectedChanged?.Invoke();
                }
                UpdateVisibleView();
            }
        }

        /// <summary>
        /// 清空选中项
        /// </summary>
        public void ClearSelectedItem(bool notify = false)
        {
            var tempList = new List<TreeItem>(_selectedItemList);
            foreach (var item in tempList) item.SetSelected(false);
            _selectedItemList.Clear();
            _firstSelectedItem = null;
            if (notify)
            {
                ClearSelect?.Invoke();
                SelectedChanged?.Invoke();
            }
            UpdateVisibleView();
        }

        /// <summary>
        /// 清空项
        /// </summary>
        public void ClearItem()
        {
            // 清空选中项
            ClearSelectedItem(true);
            // 清空全部项
            TreeRoot.ClearItem();
            _dataWindow.Reset();
            // 更新视图
            UpdateVisibleView();
            // 更新滚动条
            UpdateScrollBar();
        }

        /// <summary>
        /// 获取悬停项
        /// </summary>
        public TreeItem GetHoverTreeItem()
        {
            int offset = (int)Mouse.GetPosition(ItemBox).Y;
            int itemIndex = offset > 0 ? offset / _controlHeight : offset / _controlHeight - 1;
            if (itemIndex >= 0 && itemIndex < _visibleData.Count) return _visibleData[itemIndex];
            else return TreeRoot;
        }

        /// <summary>
        /// 展开全部
        /// </summary>
        public void ExpandAll()
        {
            ExpandFolderItem(TreeRoot);
            Update();
        }

        /// <summary>
        /// 折叠全部
        /// </summary>
        public void FurlAll()
        {
            FurlFolderItem(TreeRoot);
            Update();
        }

        #endregion

        #region 工具方法

        public void Capture() => CaptureMouse();

        public void ReleaseCapture() => ReleaseMouseCapture();

        /// <summary>
        /// 是否有选中项
        /// </summary>
        public bool HasSelected() => _selectedItemList.Count > 0;

        /// <summary>
        /// 开始拖动项
        /// </summary>
        public void BeginDragItem()
        {
            // 清空拖动源
            _dragSourceSet.Clear();
            // 遍历选中项
            foreach (var treeItem in _selectedItemList)
                // 添加实际选中项
                _dragSourceSet.Add(GetRealSelected(treeItem));
            _beginDrag = Mouse.GetPosition(this);
        }

        /// <summary>
        /// 拖动项
        /// </summary>
        public void DragItem()
        {
            // 当前坐标不等于拖动起始坐标
            if (Mouse.GetPosition(this) != _beginDrag)
            {
                if (MoveItem)
                {
                    // 更新拖动目标
                    UpdateDragTarget();
                    Cursor = CanDrag() ? MoveSelected : CanNotMove;
                }
                else Cursor = CanNotMove;
            }
        }

        /// <summary>
        /// 结束拖动项
        /// </summary>
        public void EndDragItem()
        {
            switch (GetMouseSide())
            {
                case MouseSide.InSide:
                    {
                        // 执行拖动
                        if (_dragSourceSet.Count > 0 && _dragTargetItem != null)
                            foreach (var source in _dragSourceSet) DoDrag(source, _dragTargetItem);
                        // 置空拖动目标
                        _dragTargetItem = null;
                        // 隐藏目标框
                        TargetBorder.Visibility = Visibility.Hidden;
                        // 更新视图
                        Update();
                    }
                    break;
                case MouseSide.OutSide:
                    {
                        if (GetHitedIDropable() is IDropable dropable)
                        {
                            try
                            {
                                dropable.OnDrop(_dragSourceList);
                            }
                            catch (Exception ex)
                            {
                                ShowError?.Invoke("拖动失败：" + ex.Message + ex.StackTrace);
                            }
                        }
                    }
                    break;
            }
            // 还原光标
            Cursor = Select;
            _dragSourceSeted = false;
        }

        /// <summary>
        /// 获取鼠标所在位置
        /// </summary>
        public MouseSide GetMouseSide()
        {
            if (_mouseMove.X < 0 || _mouseMove.Y < 0 || _mouseMove.X > ActualWidth || _mouseMove.Y > ActualHeight)
                return MouseSide.OutSide;
            return MouseSide.InSide;
        }

        /// <summary>
        /// 拖动项至
        /// </summary>
        public void DragItemTo()
        {
            // 置空拖动目标
            _dragTargetItem = null;
            // 隐藏目标框
            TargetBorder.Visibility = Visibility.Hidden;
            // 设置光标
            Cursor = DragObject;
            // 设置拖动源
            if (!_dragSourceSeted)
            {
                _dragSourceList.Clear();
                foreach (var item in _dragSourceSet)
                {
                    // 跳过文件夹
                    if (item.IsFolder) continue;
                    _dragSourceList.Add(item.Content);
                }
                _dragSourceSeted = true;
            }
        }

        #endregion

        #region 控件事件

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            _mouseMove = Mouse.GetPosition(this);
            _selectTool?.OnMouseMove();
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _selectTool?.OnMouseUp(e.ChangedButton);
        }

        private void TreeView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.HeightChanged)
            {
                MatchControlList();
                UpdateVisibleView();
                UpdateScrollBar();
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 清空选中项
            if (Keyboard.Modifiers != ModifierKeys.Control && Keyboard.Modifiers != ModifierKeys.Shift)
            {
                ClearSelectedItem(true);
                UpdateVisibleView();
            }
        }

        private void OnItemHited(TreeItem treeItem)
        {
            bool selectChanged = false;

            if (MultiSelect)
            {
                // 未按下“Ctrl”与“Shift”键
                if (Keyboard.Modifiers != ModifierKeys.Control && Keyboard.Modifiers != ModifierKeys.Shift &&
                    Keyboard.Modifiers != (ModifierKeys.Control | ModifierKeys.Shift))
                {
                    if (!treeItem.Selected)
                    {
                        selectChanged = true;
                        // 清空已选中项
                        ClearSelectedItem(true);
                        // 选中命中项
                        treeItem.Selected = true;
                        if (_selectedItemList.Count == 1) _firstSelectedItem = treeItem;
                    }
                }
                // 加选或减选
                else if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    selectChanged = true;
                    // 命中项未选中
                    if (!treeItem.Selected)
                    {
                        treeItem.Selected = true;
                        if (_selectedItemList.Count == 1) _firstSelectedItem = treeItem;
                    }
                    // 命中项已选中
                    else
                    {
                        treeItem.Selected = false;
                        if (_selectedItemList.Count == 0) _firstSelectedItem = null;
                    }
                }
                // 范围选择
                else if (Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    selectChanged = true;
                    // 第一个选中项不为空
                    if (_firstSelectedItem != null)
                    {
                        // 与第一个选中项为同一层级
                        if (treeItem.Parent == _firstSelectedItem.Parent)
                        {
                            // 移除除第一个选中项之外的选中项
                            var tempList = new List<TreeItem>(_selectedItemList);
                            foreach (var item in tempList)
                                if (item != _firstSelectedItem) item.Selected = false;
                            // 确定范围
                            int index_first = _firstSelectedItem.Index;
                            int index_second = treeItem.Index;
                            if (index_second > index_first)
                            {
                                for (int index = index_first + 1; index <= index_second; index++)
                                    _firstSelectedItem.Parent.ItemList[index].Selected = true;
                            }
                            else if (index_first > index_second)
                            {
                                for (int index = index_first - 1; index >= index_second; index--)
                                    _firstSelectedItem.Parent.ItemList[index].Selected = true;
                            }
                        }
                    }
                    else
                    {
                        treeItem.Selected = true;
                        if (_selectedItemList.Count == 1) _firstSelectedItem = treeItem;
                    }
                }
            }
            else
            {
                selectChanged = true;
                // 清空已选中项
                ClearSelectedItem(true);
                // 选中命中项
                treeItem.Selected = true;
                if (_selectedItemList.Count == 1) _firstSelectedItem = treeItem;
            }

            // 更新视图
            if (selectChanged) UpdateVisibleView();
            // 触发鼠标按下
            _selectTool.OnMouseDown(MouseButton.Left);
        }

        private void OnDoubleClick(TreeItem treeItem) => ItemDoubleClick?.Invoke(treeItem.Content);

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MainScrollBar.Value -= e.Delta / 120;
            _selectTool.OnMouseMove();
        }

        private void MainScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // 更新滚动条当前值
            var scrollBar = (ScrollBar)sender;
            var newValue = Math.Round(e.NewValue, 0);
            if (newValue > scrollBar.Maximum) newValue = scrollBar.Maximum;
            scrollBar.Value = newValue;
            // 更新起始索引
            _dataWindow.Top = (int)newValue;
            // 更新视图
            UpdateVisibleView();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 加载光标
        /// </summary>
        private Cursor LoadCursor(string cursorPath)
        {
            StreamResourceInfo resourceInfo = Application.GetResourceStream(new Uri(cursorPath, UriKind.Relative));
            return new Cursor(resourceInfo.Stream);
        }

        /// <summary>
        /// 匹配控件列表
        /// </summary>
        private void MatchControlList()
        {
            _dataWindow.WindowHeight = (int)(MainGrid.ActualHeight - 20) / _controlHeight + 1;
            while (_controlList.Count < _dataWindow.WindowHeight)
            {
                TreeItemView control = new TreeItemView();
                _controlList.Add(control);
                ItemBox.Children.Add(control);
                control.OnItemExpand = Update;
                control.OnItemFurl = Update;
                control.OnItemHited = OnItemHited;
                control.OnDoubleClick = OnDoubleClick;
            }
        }

        /// <summary>
        /// 更新数据源
        /// </summary>
        private void UpdateDataSource()
        {
            // 清空数据源
            _dataWindow.Data.Clear();
            // 加载树项
            LoadSubItem(TreeRoot);
        }

        /// <summary>
        /// 加载子项
        /// </summary>
        private void LoadSubItem(TreeItem parent)
        {
            // 遍历子项
            foreach (var subItem in parent.ItemList)
            {
                // 加载子项
                _dataWindow.Data.Add(subItem);
                // 子项已展开，加载子项的子项
                if (subItem.IsExpanded) LoadSubItem(subItem);
            }
        }

        /// <summary>
        /// 更新可视视图
        /// </summary>
        private void UpdateVisibleView()
        {
            // 获取可视数据
            _visibleData = _dataWindow.GetVisibleDataNew();
            // 刷新列表
            for (int index = 0; index < _controlList.Count; index++)
            {
                _controlList[index].Instance = index < _visibleData.Count ? _visibleData[index] : null;
                _controlList[index].Update();
            }
        }

        /// <summary>
        /// 更新滚动条
        /// </summary>
        private void UpdateScrollBar()
        {
            if (MainGrid.ActualHeight == 0) return;

            MainScrollBar.ViewportSize = ((int)MainGrid.ActualHeight - 20) / _controlHeight;
            // 数据量 > 可视数据量
            if (_dataWindow.DataCount > MainScrollBar.ViewportSize)
                MainScrollBar.Maximum = _dataWindow.DataCount - MainScrollBar.ViewportSize;
            else MainScrollBar.Maximum = 0;
        }

        /// <summary>
        /// 查找树项
        /// </summary>
        private TreeItem? FindTreeItem(TreeItem? parent, List<string> pathNodeList)
        {
            TreeItem? firstItem = FindSubItem(parent, pathNodeList[0]);
            if (firstItem == null) return null;
            pathNodeList.RemoveAt(0);
            if (pathNodeList.Count == 0) return firstItem;
            return FindTreeItem(firstItem, pathNodeList);
        }

        /// <summary>
        /// 查找子项
        /// </summary>
        private TreeItem? FindSubItem(TreeItem? parent, string nodeName)
        {
            if (parent == null)
            {
                foreach (var item in TreeRoot.ItemList)
                    if (item.Content.ToString() == nodeName) return item;
            }
            else
            {
                foreach (var item in parent.ItemList)
                    if (item.Content.ToString() == nodeName) return item;
            }
            return null;
        }

        /// <summary>
        /// 获取实际选中项
        /// </summary>
        private TreeItem GetRealSelected(TreeItem treeItem)
        {
            // 实际选中项
            TreeItem realSelected = treeItem;
            // 向上查找选中项
            while (treeItem.Parent != TreeRoot)
            {
                if (treeItem.Parent.Selected) realSelected = treeItem.Parent;
                treeItem = treeItem.Parent;
            }
            // 返回实际选中项
            return realSelected;
        }

        /// <summary>
        /// 更新拖动目标
        /// </summary>
        private void UpdateDragTarget()
        {
            // 计算鼠标位置与第一项的偏移
            int offset = (int)Mouse.GetPosition(ItemBox).Y;
            // 项索引
            int itemIndex = offset > 0 ? offset / _controlHeight : offset / _controlHeight - 1;
            if (itemIndex >= 0 && itemIndex < _visibleData.Count)
            {
                TreeItem item = _visibleData[itemIndex];
                // 更新目标
                _dragTargetItem = item.IsFolder && !item.Selected ? item : null;
                TargetBorder.Visibility = _dragTargetItem == null ? Visibility.Hidden : Visibility.Visible;
            }
            else
            {
                _dragTargetItem = TreeRoot;
                TargetBorder.Visibility = Visibility.Hidden;
            }
            TargetBorder.Margin = new Thickness(0, itemIndex * _controlHeight, 0, 0);
        }

        /// <summary>
        /// 能否拖动
        /// </summary>
        private bool CanDrag()
        {
            if (_dragSourceSet.Count > 0 && _dragTargetItem != null)
            {
                // 有一个能拖动就返回真
                foreach (var source in _dragSourceSet)
                    if (CanDrag(source, _dragTargetItem)) return true;
            }
            return false;
        }

        /// <summary>
        /// 能否拖动
        /// </summary>
        private bool CanDrag(TreeItem source, TreeItem target)
        {
            if (source.IsFolder && target == source) return false;
            if (target == source.Parent) return false;
            if (GetAllParent(target).Contains(source)) return false;
            return true;
        }

        /// <summary>
        /// 执行拖动
        /// </summary>
        private void DoDrag(TreeItem source, TreeItem target)
        {
            // 检测非法拖动目标
            if (!CanDrag(source, target)) return;
            // 检测能否移动至目标
            if (source.CanMoveTo(target, out string reason))
            {
                // 移除对象
                source.Parent.RemoveItem(source);
                // 添加至目标并展开目标项
                target.AddItem(source);
                source.MoveTo(target);
                // 触发事件
                ItemMoved?.Invoke();
            }
            else ShowError?.Invoke($"移动“{source}”失败：" + reason);
        }

        /// <summary>
        /// 获取项的所有父级
        /// </summary>
        private List<TreeItem> GetAllParent(TreeItem treeItem)
        {
            List<TreeItem> parentList = new List<TreeItem>();
            while (treeItem.Parent != null && treeItem.Parent != TreeRoot)
            {
                parentList.Add(treeItem.Parent);
                treeItem = treeItem.Parent;
            }
            return parentList;
        }

        /// <summary>
        /// 获取命中的“IDropable”对象
        /// </summary>
        public IDropable? GetHitedIDropable()
        {
            // 获取鼠标命中的界面元素
            UIElement? element = Mouse.DirectlyOver as UIElement;
            while (element != null)
            {
                // 如果为“IDropable”对象，则返回
                if (element is IDropable dropable) return dropable;
                // 否则继续向上查找
                element = VisualTreeHelper.GetParent(element) as UIElement;
            }
            return null;
        }

        /// <summary>
        /// 展开文件夹项
        /// </summary>
        private void ExpandFolderItem(TreeItem treeItem)
        {
            foreach (var item in treeItem.ItemList)
            {
                if (item.IsFolder && item.ItemList.Count > 0)
                {
                    item.IsExpanded = true;
                    ExpandFolderItem(item);
                }
            }
        }

        /// <summary>
        /// 折叠文件夹项
        /// </summary>
        private void FurlFolderItem(TreeItem treeItem)
        {
            foreach (var item in treeItem.ItemList)
            {
                if (item.IsFolder)
                {
                    item.IsExpanded = false;
                    FurlFolderItem(item);
                }
            }
        }

        #endregion

        #region 字段

        /// <summary>控件列表</summary>
        private readonly List<TreeItemView> _controlList = new List<TreeItemView>();
        /// <summary>控件高度</summary>
        private readonly int _controlHeight = 23;

        /// <summary>树项数据窗口</summary>
        private readonly DataWindow<TreeItem> _dataWindow = new DataWindow<TreeItem>();
        /// <summary>可视数据</summary>
        private List<TreeItem> _visibleData = new List<TreeItem>();

        /// <summary>选中项列表</summary>
        private readonly List<TreeItem> _selectedItemList = new List<TreeItem>();
        /// <summary>第一个选中项</summary>
        private TreeItem? _firstSelectedItem = null;

        /// <summary>选择工具</summary>
        private SelectTool _selectTool;
        /// <summary>拖动源集</summary>
        private readonly HashSet<TreeItem> _dragSourceSet = new HashSet<TreeItem>();
        /// <summary>拖动目标项</summary>
        private TreeItem? _dragTargetItem = null;

        private Point _beginDrag = new Point();
        private Point _mouseMove = new Point();

        /// <summary>拖动源列表</summary>
        private readonly List<ITreeItem> _dragSourceList = new List<ITreeItem>();
        /// <summary>已设置拖动源。防止在拖动时重复设置拖动源列表</summary>
        private bool _dragSourceSeted = false;

        #endregion
    }
}