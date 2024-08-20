using XLib.Node;

namespace NodeLib.File.Define
{
    public abstract class FileNode : NodeBase
    {
        public FileNode() => NodeLibName = "File";

        public override Dictionary<string, string> GetParaDict() => new Dictionary<string, string>();
    }
}