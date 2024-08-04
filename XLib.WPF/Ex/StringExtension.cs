using System.Windows.Media;

namespace XLib.WPF.Ex
{
    public static class StringExtension
    {
        /// <summary>
        /// 颜色代码转颜色。格式：FFFFFF
        /// </summary>
        public static Color ToColor(this string hexCode)
        {
            try
            {
                byte r = Convert.ToByte(hexCode.Substring(0, 2), 16);
                byte g = Convert.ToByte(hexCode.Substring(2, 2), 16);
                byte b = Convert.ToByte(hexCode.Substring(4, 2), 16);
                return Color.FromRgb(r, g, b);
            }
            catch (Exception) { return Colors.Black; }
        }
    }
}