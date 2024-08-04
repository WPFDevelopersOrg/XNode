using System.Windows.Controls;
using XLib.Base;
using XLib.Base.VirtualDisk;
using XLib.WPFControl;
using XNode.SubSystem.CacheSystem;
using XNode.SubSystem.ResourceSystem;

namespace XNode.SubSystem.NodeLibSystem
{
    public partial class NodeLibPanel : UserControl
    {
        public NodeLibPanel()
        {
            InitializeComponent();
        }

        public void Init()
        {
            NodePresetTree.Init();
            NodePresetTree.SetTreeRoot(new TreeItem(NodeLibManager.Instance.Root));
            LoadNodeLib(NodeLibManager.Instance.Root, null);
        }

        #region 私有方法

        /// <summary>
        /// 加载节点库：将节点库构建为树视图
        /// </summary>
        private void LoadNodeLib(Folder folder, TreeItem? parentItem)
        {
            // 加载文件夹
            foreach (var currentFolder in folder.FolderList)
            {
                // 确定文件夹图标
                string icon = ContainsChild(currentFolder) ? "Folder" : "EmptyFolder";
                if (parentItem == null) icon = "Lib";
                // 添加文件夹项
                TreeItem folderTreeItem = AddTreeItem(parentItem, currentFolder, icon);
                // 恢复折叠状态
                folderTreeItem.IsExpanded = CacheManager.Instance.Cache.NodeLib.IsExpanded(folderTreeItem.GetFullPath());
                folderTreeItem.ItemExpanded = TreeItem_Expanded;
                folderTreeItem.ItemCollapsed = TreeItem_Collapsed;
                // 加载文件
                if (currentFolder.FolderList.Count > 0 || currentFolder.FileList.Count > 0)
                    LoadNodeLib(currentFolder, folderTreeItem);
            }
            // 加载文件
            foreach (var currentFile in folder.FileList)
            {
                AddTreeItem(parentItem, currentFile, "NodePreset");
            }
        }

        /// <summary>
        /// 树项展开
        /// </summary>
        private void TreeItem_Expanded(TreeItem treeItem) => CacheManager.Instance.Cache.NodeLib.Expand(treeItem.GetFullPath());

        /// <summary>
        /// 树项折叠
        /// </summary>
        private void TreeItem_Collapsed(TreeItem treeItem) => CacheManager.Instance.Cache.NodeLib.Fold(treeItem.GetFullPath());

        /// <summary>
        /// 包含子项
        /// </summary>
        private bool ContainsChild(Folder folder)
        {
            if (folder.FolderList.Count > 0) return true;
            if (folder.FileList.Count > 0) return true;
            return false;
        }

        /// <summary>
        /// 添加树项
        /// </summary>
        private TreeItem AddTreeItem(TreeItem? parent, ITreeItem content, string icon)
        {
            TreeItem treeItem = new TreeItem(content)
            {
                Icon = ImageResManager.Instance.GetIcon15(icon + ".png"),
            };
            if (parent == null) NodePresetTree.TreeRoot.AddItem(treeItem);
            else parent.AddItem(treeItem);
            NodePresetTree.Update();
            return treeItem;
        }

        #endregion
    }
}