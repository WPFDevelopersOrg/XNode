namespace XLib.Base.VirtualDisk
{
    public class Folder : ITreeItem, IComparable<Folder>
    {
        #region 构造方法

        public Folder(string name, Folder? parent = null)
        {
            Name = name;
            Parent = parent;
        }

        #endregion

        #region 属性

        /// <summary>名称</summary>
        public string Name { get; set; } = "未命名文件夹";

        /// <summary>所在文件夹</summary>
        public Folder? Parent { get; set; } = null;

        /// <summary>文件夹列表</summary>
        public List<Folder> FolderList { get; set; } = new List<Folder>();

        /// <summary>文件列表</summary>
        public List<File> FileList { get; set; } = new List<File>();

        /// <summary>路径</summary>
        public List<string> Path
        {
            get
            {
                if (Parent == null) return new List<string>();
                List<string> result = Parent.Path;
                result.Add(Name);
                return result;
            }
        }

        public bool IsFolder => true;

        #endregion

        #region Object 方法

        public override string ToString() => Name;

        #endregion

        #region ITreeItem 方法

        public void Rename(string newName)
        {
            Name = newName;
            Parent.FolderList.Sort();
        }

        public bool CanMoveTo(ITreeItem target, out string reason)
        {
            if (target is Folder folder && folder.FolderNameUsed(Name))
            {
                reason = "已存在同名文件夹";
                return false;
            }
            reason = "";
            return true;
        }

        public void NotifyMoveTo(ITreeItem target)
        {
            if (target is Folder targetFolder)
            {
                // 从当前所在文件夹移除
                Parent.FolderList.Remove(this);
                // 添加至目标文件夹
                targetFolder.FolderList.Add(this);
                // 排序
                targetFolder.FolderList.Sort();
                // 更新所在文件夹
                Parent = targetFolder;
            }
        }

        #endregion

        #region IComparable 方法

        public int CompareTo(Folder? other) => NaturalComparator.Compare(Name, other.Name);

        #endregion

        #region 公开方法

        /// <summary>
        /// 获取一个能使用的文件夹名称
        /// </summary>
        public string GetCanUseFolderName()
        {
            List<string> nameList = new List<string>();
            foreach (var folder in FolderList) nameList.Add(folder.Name);

            int nameID = 1;
            while (true)
            {
                string name = $"新建文件夹_{nameID:00}";
                if (nameList.Contains(name))
                {
                    nameID++;
                    continue;
                }
                return name;
            }
        }

        /// <summary>
        /// 获取一个能使用的文件名称
        /// </summary>
        public string GetCanUseFileName(string baseName)
        {
            List<string> nameList = new List<string>();
            foreach (var folder in FileList) nameList.Add(folder.Name);

            int nameID = 1;
            while (true)
            {
                string name = $"{baseName}_{nameID:00}";
                if (nameList.Contains(name))
                {
                    nameID++;
                    continue;
                }
                return name;
            }
        }

        /// <summary>
        /// 文件夹名称已使用
        /// </summary>
        public bool FolderNameUsed(string name)
        {
            foreach (var folder in FolderList)
                if (folder.Name == name) return true;
            return false;
        }

        /// <summary>
        /// 文件名称已使用
        /// </summary>
        public bool FileNameUsed(string name)
        {
            foreach (var file in FileList)
                if (file.Name == name) return true;
            return false;
        }

        /// <summary>
        /// 加载文件夹
        /// </summary>
        public void LoadFolder(Folder folder)
        {
            folder.Parent = this;
            FolderList.Add(folder);
        }

        /// <summary>
        /// 加载文件
        /// </summary>
        public void LoadFile(File file)
        {
            file.Parent = this;
            FileList.Add(file);
        }

        /// <summary>
        /// 移除文件夹
        /// </summary>
        public void RemoveFolder(Folder folder)
        {
            // 置空所在文件夹
            folder.Parent = null;
            // 移除文件夹
            FolderList.Remove(folder);
        }

        /// <summary>
        /// 移除文件
        /// </summary>
        public void RemoveFile(File file)
        {
            // 置空所在文件夹
            file.Parent = null;
            // 移除文件
            FileList.Remove(file);
        }

        /// <summary>
        /// 查找文件夹
        /// </summary>
        public Folder? FindFoder(string name)
        {
            foreach (var folder in FolderList)
                if (folder.Name == name) return folder;
            return null;
        }

        /// <summary>
        /// 查找文件
        /// </summary>
        public File? FindFile(string name)
        {
            foreach (var file in FileList)
                if (file.Name == name) return file;
            return null;
        }

        #endregion
    }
}