using NodeLib.File.Define;
using XLib.Base.VirtualDisk;
using XLib.Node;

namespace NodeLib.File
{
    public class NodeLibManager : INodeLib
    {
        #region 单例

        private NodeLibManager() { }
        public static NodeLibManager Instance { get; } = new NodeLibManager();

        #endregion

        #region INodeLib 属性

        public string Name { get; set; } = "File";

        public string Title { get; set; } = "文件处理";

        public Harddisk LibHarddisk { get; set; } = new Harddisk();

        #endregion

        #region INodeLib 方法

        public void Init()
        {
            LibHarddisk.CreateFile(LibHarddisk.Root, "计算文件摘要", "nt", new NodeType<Func_GetFileMD5>());
        }

        public void Clear() { }

        public NodeBase? CreateNode(string typeString)
        {
            return typeString switch
            {
                nameof(Func_GetFileMD5) => new Func_GetFileMD5(),

                _ => null,
            };
        }

        #endregion
    }
}