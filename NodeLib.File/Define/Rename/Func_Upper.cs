using XLib.Node;

namespace NodeLib.File.Define.Rename
{
    /// <summary>
    /// 全大写
    /// </summary>
    public class Func_Upper : FileNode
    {
        public override void Init()
        {
            SetViewProperty(NodeColorSet.Function, "Function", "全大写");

            PinGroupList.Add(new ExecutePinGroup(this, "将文件名改为全大写，忽略扩展名"));
            PinGroupList.Add(new DataPinGroup(this, "string", "文件路径", "")
            {
                BoxWidth = 300,
                Readable = false,
                Writeable = true
            });

            InitPinGroup();
        }

        protected override void ExecuteNode()
        {
            // 更新并获取文件路径
            UpdateData(1);
            string filePath = GetData(1);
            if (filePath == "") return;

            try
            {
                // 获取文件名
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                // 获取扩展名
                string extension = Path.GetExtension(filePath);
                // 修改文件名
                string newFileName = fileName.ToUpper() + extension;
                // 获取文件夹路径
                string folderPath = Path.GetDirectoryName(filePath);
                // 获取新文件路径
                string newFilePath = Path.Combine(folderPath, newFileName);
                // 修改文件名
                System.IO.File.Move(filePath, newFilePath);

                GetPinGroup<ExecutePinGroup>().Execute();
            }
            catch (Exception ex) { InvokeExecuteError(ex); }
        }

        public override string GetTypeString() => nameof(Func_Upper);

        public override Dictionary<string, string> GetParaDict()
        {
            Dictionary<string, string> result = new Dictionary<string, string>
            {
                { "FilePath", GetData(1) },
            };
            return result;
        }

        public override void LoadParaDict(string version, Dictionary<string, string> paraDict)
        {
            SetData(1, paraDict["FilePath"]);
        }

        protected override NodeBase CloneNode() => new Func_Upper();
    }
}