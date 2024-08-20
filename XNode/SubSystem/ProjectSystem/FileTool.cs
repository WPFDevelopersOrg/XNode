using Microsoft.Win32;
using XLib.Base;
using XNode.SubSystem.OptionSystem;

namespace XNode.SubSystem.ProjectSystem
{
    /// <summary>
    /// 文件工具
    /// </summary>
    public class FileTool
    {
        private FileTool()
        {
            _projectFilter.TypeList.Add(new TypeInfo("节点项目", "xnode"));
            _projectFilter.TypeList.Add(new TypeInfo("节点项目", "json"));
        }
        public static FileTool Instance { get; } = new FileTool();

        /// <summary>
        /// 打开读取项目对话框
        /// </summary>
        public string OpenReadProjectDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = OptionManager.Instance.ProjectPath,
                Filter = _projectFilter.ToString(),
            };
            return dialog.ShowDialog() == true ? dialog.FileName : "";
        }

        /// <summary>
        /// 打开保存项目对话框
        /// </summary>
        public string OpenSaveProjectDialog(string fileName)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                InitialDirectory = OptionManager.Instance.ProjectPath,
                FileName = $"{fileName}.xnode",
                Filter = _projectFilter.ToString(),
            };
            return dialog.ShowDialog() == true ? dialog.FileName : "";
        }

        private readonly FileFilter _projectFilter = new FileFilter();
    }
}