using XLib.Base.Ex;
using XLib.Base.ID;

namespace XLib.Base.VirtualDisk
{
    public class Harddisk
    {
        #region 属性

        /// <summary>根文件夹</summary>
        public Folder Root { get; private set; } = new Folder("根");

        /// <summary>文件表</summary>
        public Dictionary<int, File> FileDict { get; set; } = new Dictionary<int, File>();

        #endregion

        #region 事件

        /// <summary>文件夹已创建</summary>
        public Action<Folder>? Folder_Created { get; set; } = null;

        /// <summary>文件夹已移除</summary>
        public Action<Folder>? Folder_Removed { get; set; } = null;

        /// <summary>文件已创建</summary>
        public Action<File>? File_Created { get; set; } = null;

        /// <summary>文件已移除</summary>
        public Action<File>? File_Removed { get; set; } = null;

        #endregion

        #region 文件夹方法

        /// <summary>
        /// 创建文件夹
        /// </summary>
        public Folder CreateFolder(List<string> path) => CreateFolder(Root, path);

        /// <summary>
        /// 创建文件夹
        /// </summary>
        public Folder CreateFolder(Folder parent, List<string> path)
        {
            if (path.Count == 0) return parent;

            Folder? folder = FindSubFolder(parent, path[0]);
            if (folder == null)
            {
                // 创建文件夹
                folder = new Folder(path[0], parent);
                // 添加至父文件夹
                parent.FolderList.Add(folder);
                // 引发事件
                Folder_Created?.Invoke(folder);
            }

            path.RemoveAt(0);
            return CreateFolder(folder, path);
        }

        /// <summary>
        /// 移除文件夹
        /// </summary>
        public void RemoveFolder(List<string> nodeList)
        {
            // 根据路径查找文件夹
            Folder? folder = FindFolder(nodeList);
            if (folder == null) return;
            // 从文件夹移除文件夹
            folder.Parent.RemoveFolder(folder);
            // 触发事件
            Folder_Removed?.Invoke(folder);
        }

        /// <summary>
        /// 根据路径查找文件夹
        /// </summary>
        public Folder? FindFolder(List<string> nodeList)
        {
            // 当前文件夹
            Folder currentFolder = Root;
            // 有剩余节点
            while (nodeList.Count > 0)
            {
                // 从当前文件夹查找文件夹
                Folder? folder = currentFolder.FindFoder(nodeList[0]);
                // 未找到文件夹，返回空
                if (folder == null) return null;
                // 找到文件夹，更新当前文件夹并移除首节点
                currentFolder = folder;
                nodeList.RemoveAt(0);
            }
            // 返回当前文件夹
            return currentFolder;
        }

        #endregion

        #region 文件方法

        /// <summary>
        /// 在指定路径下创建文件
        /// </summary>
        public File CreateFile(List<string> path, string name, string extension, object? instance = null)
        {
            // 路径为空，在根文件夹创建
            if (path.Count == 0) return CreateFile(Root, name, extension, instance);
            // 创建文件夹，再创建文件
            return CreateFile(CreateFolder(Root, path), name, extension, instance);
        }

        /// <summary>
        /// 在指定文件夹中创建文件
        /// </summary>
        public File CreateFile(Folder parent, string name, string extension, object? instance)
        {
            if (parent.FileNameUsed(name)) throw new Exception($"已存在“{name}”");

            // 创建文件
            File file = new File(_fileIDBox.TakeID(), name, extension, parent) { Instance = instance };
            // 添加至文件表
            FileDict.Add(file.ID, file);
            // 添加至文件夹
            parent.FileList.Add(file);
            // 设置所在文件夹
            file.Parent = parent;
            // 引发事件
            File_Created?.Invoke(file);
            // 返回文件
            return file;
        }

        /// <summary>
        /// 创建空文件
        /// </summary>
        public File CreateEmptyFile(int id, string name, string extension)
        {
            File file = new File(id, name, extension, null);
            _fileIDBox.UseID(id);
            FileDict.Add(file.ID, file);
            return file;
        }

        /// <summary>
        /// 移除指定路径的文件
        /// </summary>
        public void RemoveFile(List<string> nodeList)
        {
            File? file = FindFile(nodeList);
            if (file != null) RemoveFile(file);
        }

        /// <summary>
        /// 移除指定文件
        /// </summary>
        public void RemoveFile(File file)
        {
            // 从文件夹移除文件
            file.Parent.RemoveFile(file);
            // 移除文件实例
            FileDict.Remove(file.ID);
            // 回收文件编号
            _fileIDBox.RecycleID(file.ID);
            // 触发事件
            File_Removed?.Invoke(file);
        }

        /// <summary>
        /// 查找文件
        /// </summary>
        public File? FindFile(int fileID)
        {
            if (FileDict.ContainsKey(fileID)) return FileDict[fileID];
            return null;
        }

        /// <summary>
        /// 根据路径查找文件
        /// </summary>
        public File? FindFile(List<string> nodeList)
        {
            if (nodeList.Count == 0) return null;

            // 引用最后一个节点并移除
            string fileName = nodeList.Last();
            nodeList.RemoveLast();
            // 先查找文件夹
            Folder? folder = FindFolder(nodeList);
            // 查找文件
            return folder?.FindFile(fileName);
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        public string GetFilePath(int fileID)
        {
            File? file = FindFile(fileID);
            if (file == null) return "";

            string path = file.Name;
            Folder? parent = file.Parent;
            while (parent != null)
            {
                path = parent.Name + "/" + path;
                parent = parent.Parent;
            }
            return path;
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 清空磁盘
        /// </summary>
        public void ClearDisk()
        {
            // 清空文件表
            FileDict.Clear();
            // 清空根文件夹
            Root.FolderList.Clear();
            Root.FileList.Clear();
            // 重置编号箱
            _fileIDBox.Reset();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 查找子文件夹
        /// </summary>
        protected Folder? FindSubFolder(Folder parent, string name)
        {
            foreach (var item in parent.FolderList)
                if (item.Name == name) return item;
            return null;
        }

        #endregion

        #region 字段

        /// <summary>文件编号箱</summary>
        private readonly IDBox _fileIDBox = new IDBox();

        #endregion
    }
}