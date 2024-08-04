
using System.Windows;
using System.Windows.Media;

namespace XNode.SubSystem.NodeEditSystem.Define
{
    /// <summary>
    /// 连接线
    /// </summary>
    public class ConnectLine
    {
        public Point Start { get; set; }

        public Point End { get; set; }

        public Color Color { get; set; } = Colors.White;

        public bool IsData { get; set; } = false;

        public override string ToString() => $"{Start}-{End}";
    }
}