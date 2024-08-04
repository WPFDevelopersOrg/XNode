using System.Windows.Media;

namespace XLib.WPF.Ex
{
    public static class StructExtension
    {
        /// <summary>
        /// Drawing.Color -> Media.Color
        /// </summary>
        public static Color ToMediaColor(this System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Media.Color -> Drawing.Color
        /// </summary>
        public static System.Drawing.Color ToDrawingColor(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}