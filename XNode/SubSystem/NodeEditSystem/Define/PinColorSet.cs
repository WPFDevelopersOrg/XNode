using System.Windows.Media;
using XLib.WPF.Ex;

namespace XNode.SubSystem.NodeEditSystem.Define
{
    /// <summary>
    /// 引脚颜色集
    /// </summary>
    public class PinColorSet
    {
        public static Color Execute => "C47EFF".ToColor();

        public static Color Bool => "A7C4B5".ToColor();

        public static Color Int => "B3D465".ToColor();

        public static Color Double => "E06C9F".ToColor();

        public static Color String => "F3B562".ToColor();

        public static Color ByteArray => "6CB891".ToColor();
    }
}