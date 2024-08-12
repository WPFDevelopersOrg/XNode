namespace XNode.SubSystem.ProjectSystem
{
    /// <summary>
    /// 节点项目
    /// </summary>
    public class NodeProject
    {
        /// <summary>项目路径。仅文件夹</summary>
        public string ProjectPath { get; set; } = "";

        /// <summary>项目名称。不包含扩展名</summary>
        public string ProjectName { get; set; } = "";

        public string ProjectFileName
        {
            get
            {
                if (_projectFileName == "") return ProjectName;
                return _projectFileName;
            }
            set => _projectFileName = value;
        }

        /// <summary>文件扩展名</summary>
        public static string FileExtension { get; private set; } = ".xnode";

        /// <summary>项目文件路径</summary>
        public string ProjectFilePath => $"{ProjectPath}\\{ProjectFileName}{FileExtension}";

        public NodeProject Clone()
        {
            return new NodeProject
            {
                ProjectPath = ProjectPath,
                ProjectName = ProjectName,
            };
        }

        private string _projectFileName = "";
    }
}