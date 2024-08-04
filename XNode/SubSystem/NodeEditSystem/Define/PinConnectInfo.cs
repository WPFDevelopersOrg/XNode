using XLib.Node;

namespace XNode.SubSystem.NodeEditSystem.Define
{
    /// <summary>
    /// 引脚连接信息
    /// </summary>
    public class PinConnectInfo
    {
        public string Title { get; set; } = "";

        public PinFlow Flow { get; set; } = PinFlow.Input;

        public List<PinPath> PathList { get; set; } = new List<PinPath>();
    }
}