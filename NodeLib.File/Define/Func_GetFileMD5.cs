using System.Security.Cryptography;
using System.Text;
using XLib.Node;

namespace NodeLib.File.Define
{
    public class Func_GetFileMD5 : FileNode
    {
        public override void Init()
        {
            SetViewProperty(NodeColorSet.Function, "Function", "计算文件摘要");

            PinGroupList.Add(new ExecutePinGroup(this, "计算指定文件的MD5"));
            PinGroupList.Add(new DataPinGroup(this, "string", "文件路径", "")
            {
                BoxWidth = 300,
                Readable = false,
                Writeable = false
            });
            PinGroupList.Add(new DataPinGroup(this, "string", "摘要", "")
            {
                BoxWidth = 220,
                Writeable = false
            });

            InitPinGroup();
        }

        protected override void ExecuteNode()
        {
            // 获取文件路径
            string filePath = GetData(1);
            if (filePath == "") return;

            try
            {
                // 打开文件
                using FileStream fileStream = System.IO.File.OpenRead(filePath);
                // 计算摘要  
                using MD5 md5 = MD5.Create();
                byte[] hashBytes = md5.ComputeHash(fileStream);
                // 将字节数组转换为十六进制字符串
                StringBuilder builder = new StringBuilder();
                foreach (byte item in hashBytes) builder.Append(item.ToString("x2"));
                // 设置结果
                SetData(2, builder.ToString());

                GetPinGroup<ExecutePinGroup>().Execute();
            }
            catch (Exception ex) { InvokeExecuteError(ex); }
        }

        public override string GetTypeString() => nameof(Func_GetFileMD5);

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

        protected override NodeBase CloneNode() => new Func_GetFileMD5();
    }
}