namespace XLib.Base.VirtualDisk
{
    public class File : ITreeItem, IComparable<File>
    {
        #region 构造方法

        public File(int id, string name, string extension, Folder? parent = null)
        {
            if (id == -1) throw new Exception("无效的文件编号");
            if (name == "") throw new Exception("文件名不能为空");
            if (extension == "") throw new Exception("扩展名不能为空");

            ID = id;
            Name = name;
            Extension = extension;
            Parent = parent;
        }

        #endregion

        #region 属性

        /// <summary>编号</summary>
        public int ID { get; set; } = -1;

        /// <summary>名称</summary>
        public string Name { get; set; } = "未命名文件";

        /// <summary>扩展名</summary>
        public string Extension { get; set; } = "";

        /// <summary>实例：文件真实数据</summary>
        public object? Instance { get; set; } = null;

        /// <summary>所在文件夹</summary>
        public Folder? Parent { get; set; }

        /// <summary>状态</summary>
        public string State { get; set; } = "";

        public bool IsFolder => false;

        #endregion

        #region Object 方法

        public override string ToString() => Name;

        #endregion

        #region ITreeItem 方法

        public void Rename(string newName)
        {
            Name = newName;
            Parent.FileList.Sort();
        }

        public bool CanMoveTo(ITreeItem target, out string reason)
        {
            // 如果目标文件夹已存在同名文件
            if (target is Folder folder && folder.FileNameUsed(Name))
            {
                reason = "已存在同名文件";
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
                Parent.FileList.Remove(this);
                // 添加至目标文件夹
                targetFolder.FileList.Add(this);
                // 排序
                targetFolder.FileList.Sort();
                // 更新所在文件夹
                Parent = targetFolder;
            }
        }

        #endregion

        #region IComparable 方法

        public int CompareTo(File? other) => NaturalComparator.Compare(Name, other.Name);

        #endregion

        #region 公开方法

        public T GetInstance<T>()
        {
            if (Instance == null) throw new Exception("文件实例为空");
            return Instance is T result ? result : throw new Exception("实例类型不匹配");
        }

        public File Copy()
        {
            File result = new File(ID, Name, Extension, null)
            {
                Instance = Instance
            };
            return result;
        }

        #endregion
    }
}