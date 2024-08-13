using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using XLib.Base.ArchiveFrame;
using XLib.Base.Ex;
using XNode.SubSystem.ArchiveSystem;
using XNode.SubSystem.EventSystem;
using XNode.SubSystem.WindowSystem;

namespace XNode.SubSystem.ProjectSystem
{
    /// <summary>
    /// 项目管理器
    /// </summary>
    public class ProjectManager
    {
        #region 单例

        private ProjectManager() { }
        public static ProjectManager Instance { get; } = new ProjectManager();

        #endregion

        #region 属性

        /// <summary>当前项目</summary>
        public NodeProject? CurrentProject { get; set; } = null;

        /// <summary>已保存</summary>
        public bool Saved
        {
            get => _saved;
            set
            {
                _saved = value;
                EM.Instance.Invoke(EventType.Project_Changed);
            }
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 打开项目
        /// </summary>
        public void OpenProject()
        {
            // 当前项目未保存
            if (!Saved)
            {
                bool? result = WM.ShowAsk("当前项目未保存，是否保存？");
                // 取消操作
                if (result == null) return;
                // 保存项目
                else if (result == true) SaveProject();
            }
            // 选择项目文件
            string filePath = FileTool.Instance.OpenReadProjectDialog();
            if (filePath == "") return;
            // 读取存档文件
            ArchiveFile? file = ArchiveManager.Instance.ReadArchiveFile(filePath);
            if (file == null)
            {
                WM.ShowError($"项目文件“{filePath}”读取失败：无效的存档文件");
                return;
            }
            // 关闭当前项目
            CloseProject();
            // 加载项目
            bool success = ArchiveManager.Instance.LoadArchive(file, filePath, (JObject)file.Data);
            if (success)
            {
                SwitchProject(filePath);
                Saved = true;
                EM.Instance.Invoke(EventType.Project_Loaded);
            }
        }

        /// <summary>
        /// 保存项目
        /// </summary>
        public void SaveProject()
        {
            // 没有当前项目，则选择一个路径已创建当前项目
            if (CurrentProject == null)
            {
                // 选择项目保存路径
                string projectPath = FileTool.Instance.OpenSaveProjectDialog();
                // 未选择，取消保存
                if (projectPath == "") return;
                // 设置为当前项目
                SwitchProject(projectPath);
                // 创建空文本文件
                File.WriteAllText(projectPath, "", Encoding.UTF8);
            }
            // 执行保存
            ExecuteSave();
        }

        /// <summary>
        /// 关闭项目
        /// </summary>
        public void CloseProject()
        {
            // 重置核心编辑器
            WM.Main.Editer.ResetEditer();
            // 置空当前项目
            CurrentProject = null;
            Saved = true;
        }

        /// <summary>
        /// 切换项目
        /// </summary>
        public void SwitchProject(string fullPath)
        {
            // 解析文件路径与名称
            (string, string) pathInfo = fullPath.ParsePath("\\");
            // 设置当前项目
            CurrentProject = new NodeProject
            {
                ProjectPath = pathInfo.Item1,
                ProjectName = pathInfo.Item2.RemoveExtension()
            };
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 执行保存
        /// </summary>
        private void ExecuteSave()
        {
            if (!File.Exists(CurrentProject.ProjectFilePath)) return;
            if (ProjectReadonly()) return;

            try
            {
                // 备份项目
                string backupPath = BackupProject();

                // 生成存档数据
                ArchiveFile file = ArchiveManager.Instance.GenerateArchive();
                // 序列化存档数据
                string jsonData = JsonConvert.SerializeObject(file, Formatting.Indented);
                // 创建文件并写入数据，文件已存在则覆盖
                File.WriteAllText(CurrentProject.ProjectFilePath, jsonData, Encoding.UTF8);
                // 设置为已保存
                Saved = true;

                // 删除备份
                if (backupPath != "" && File.Exists(backupPath)) File.Delete(backupPath);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 项目为只读
        /// </summary>
        private bool ProjectReadonly()
        {
            // 获取文件的属性
            FileAttributes attributes = File.GetAttributes(CurrentProject.ProjectFilePath);
            // 返回文件是否为只读
            return (attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
        }

        /// <summary>
        /// 备份项目
        /// </summary>
        private string BackupProject()
        {
            if (CurrentProject == null) return "";

            NodeProject backup = CurrentProject.Clone();
            backup.ProjectFileName += "_Backup";
            File.Copy(CurrentProject.ProjectFilePath, backup.ProjectFilePath, true);

            return backup.ProjectFilePath;
        }

        #endregion

        #region 属性字段

        private bool _saved = true;

        #endregion
    }
}