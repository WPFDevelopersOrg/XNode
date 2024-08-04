using XLib.Drawing;

namespace XNode.SubSystem.ResourceSystem
{
    /// <summary>
    /// 引脚类型
    /// </summary>
    public enum PinType
    {
        /// <summary>执行引脚</summary>
        Execute,
        /// <summary>数据引脚</summary>
        Data
    }

    /// <summary>
    /// 引脚样式
    /// </summary>
    public enum PinStyle
    {
        /// <summary>空心</summary>
        Hollow,
        /// <summary>实心</summary>
        Solid,
    }

    /// <summary>
    /// 引脚图标工具：用于生成引脚图标
    /// </summary>
    public class PinIconTool
    {
        #region 公开方法

        /// <summary>
        /// 创建引脚图标。返回引脚图标的像素点颜色数据
        /// </summary>
        public static byte[] CreatePinIcon(PinType type, byte r, byte g, byte b, PinStyle style = PinStyle.Hollow)
        {
            if (type == PinType.Execute)
            {
                if (style == PinStyle.Hollow) return DrawExecutePinIcon(r, g, b).GetPixelData();
                return DrawSolidExecutePinIcon(r, g, b).GetPixelData();
            }
            if (style == PinStyle.Hollow) return DrawDataPinIcon(r, g, b).GetPixelData();
            return DrawSolidDataPinIcon(r, g, b).GetPixelData();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 绘制执行引脚图标
        /// </summary>
        private static Bitmap DrawExecutePinIcon(byte r, byte g, byte b)
        {
            Pixel pixel = new Pixel(r, g, b);
            Pixel alphaPixel = new Pixel(r, g, b, 51);

            Bitmap bitmap = new Bitmap(11, 11);

            // 上边、下边、左边
            bitmap.DrawHorizontalLine(0, 0, 6, pixel);
            bitmap.DrawHorizontalLine(0, 10, 6, pixel);
            bitmap.DrawVerticalLine(0, 1, 9, pixel);
            // 右上斜线、右下斜线
            bitmap.DrawSlash(6, 1, 1, 1, 5, pixel);
            bitmap.DrawSlash(9, 6, -1, 1, 4, pixel);
            // 黑色区域
            bitmap.DrawRectangle(1, 1, 5, 9, Pixel.Black);
            bitmap.DrawVerticalLine(6, 2, 7, Pixel.Black);
            bitmap.DrawVerticalLine(7, 3, 5, Pixel.Black);
            bitmap.DrawVerticalLine(8, 4, 3, Pixel.Black);
            bitmap.DrawPixel(9, 5, Pixel.Black);
            // 半透明斜线
            bitmap.DrawSlash(6, 0, 1, 1, 5, alphaPixel);
            bitmap.DrawSlash(5, 1, 1, 1, 5, alphaPixel);
            bitmap.DrawSlash(8, 6, -1, 1, 4, alphaPixel);
            bitmap.DrawSlash(10, 6, -1, 1, 5, alphaPixel);

            return bitmap;
        }

        /// <summary>
        /// 绘制实心执行引脚图标
        /// </summary>
        private static Bitmap DrawSolidExecutePinIcon(byte r, byte g, byte b)
        {
            Pixel pixel = new Pixel(r, g, b);
            Pixel alphaPixel = new Pixel(r, g, b, 51);

            Bitmap bitmap = new Bitmap(11, 11);

            bitmap.DrawHorizontalLine(0, 0, 6, pixel);
            bitmap.DrawHorizontalLine(0, 1, 7, pixel);
            bitmap.DrawHorizontalLine(0, 2, 8, pixel);
            bitmap.DrawHorizontalLine(0, 3, 9, pixel);
            bitmap.DrawHorizontalLine(0, 4, 10, pixel);
            bitmap.DrawHorizontalLine(0, 5, 11, pixel);
            bitmap.DrawHorizontalLine(0, 6, 10, pixel);
            bitmap.DrawHorizontalLine(0, 7, 9, pixel);
            bitmap.DrawHorizontalLine(0, 8, 8, pixel);
            bitmap.DrawHorizontalLine(0, 9, 7, pixel);
            bitmap.DrawHorizontalLine(0, 10, 6, pixel);

            bitmap.DrawSlash(6, 0, 1, 1, 5, alphaPixel);
            bitmap.DrawSlash(10, 6, -1, 1, 5, alphaPixel);

            return bitmap;
        }

        /// <summary>
        /// 绘制数据引脚图标
        /// </summary>
        private static Bitmap DrawDataPinIcon(byte r, byte g, byte b)
        {
            Pixel pixel = new Pixel(r, g, b);
            Pixel alphaPixel = new Pixel(r, g, b, 51);

            Bitmap bitmap = new Bitmap(11, 11);

            bitmap.DrawSlash(5, 0, 1, 1, 6, pixel);
            bitmap.DrawSlash(9, 6, -1, 1, 5, pixel);
            bitmap.DrawSlash(4, 9, -1, -1, 5, pixel);
            bitmap.DrawSlash(1, 4, 1, -1, 4, pixel);

            bitmap.DrawPixel(5, 1, Pixel.Black);
            bitmap.DrawHorizontalLine(4, 2, 3, Pixel.Black);
            bitmap.DrawHorizontalLine(3, 3, 5, Pixel.Black);
            bitmap.DrawHorizontalLine(2, 4, 7, Pixel.Black);
            bitmap.DrawHorizontalLine(1, 5, 9, Pixel.Black);
            bitmap.DrawHorizontalLine(2, 6, 7, Pixel.Black);
            bitmap.DrawHorizontalLine(3, 7, 5, Pixel.Black);
            bitmap.DrawHorizontalLine(4, 8, 3, Pixel.Black);
            bitmap.DrawPixel(5, 9, Pixel.Black);

            bitmap.DrawSlash(6, 0, 1, 1, 5, alphaPixel);
            bitmap.DrawSlash(10, 6, -1, 1, 5, alphaPixel);
            bitmap.DrawSlash(4, 10, -1, -1, 5, alphaPixel);
            bitmap.DrawSlash(0, 4, 1, -1, 5, alphaPixel);

            bitmap.DrawSlash(5, 1, 1, 1, 5, alphaPixel);
            bitmap.DrawSlash(8, 6, -1, 1, 4, alphaPixel);
            bitmap.DrawSlash(4, 8, -1, -1, 4, alphaPixel);
            bitmap.DrawSlash(2, 4, 1, -1, 3, alphaPixel);

            return bitmap;
        }

        /// <summary>
        /// 绘制实心数据引脚图标
        /// </summary>
        private static Bitmap DrawSolidDataPinIcon(byte r, byte g, byte b)
        {
            Pixel pixel = new Pixel(r, g, b);
            Pixel alphaPixel = new Pixel(r, g, b, 51);

            Bitmap bitmap = new Bitmap(11, 11);

            bitmap.DrawHorizontalLine(5, 0, 1, pixel);
            bitmap.DrawHorizontalLine(4, 1, 3, pixel);
            bitmap.DrawHorizontalLine(3, 2, 5, pixel);
            bitmap.DrawHorizontalLine(2, 3, 7, pixel);
            bitmap.DrawHorizontalLine(1, 4, 9, pixel);
            bitmap.DrawHorizontalLine(0, 5, 11, pixel);
            bitmap.DrawHorizontalLine(1, 6, 9, pixel);
            bitmap.DrawHorizontalLine(2, 7, 7, pixel);
            bitmap.DrawHorizontalLine(3, 8, 5, pixel);
            bitmap.DrawHorizontalLine(4, 9, 3, pixel);
            bitmap.DrawHorizontalLine(5, 10, 1, pixel);

            bitmap.DrawSlash(6, 0, 1, 1, 5, alphaPixel);
            bitmap.DrawSlash(10, 6, -1, 1, 5, alphaPixel);
            bitmap.DrawSlash(4, 10, -1, -1, 5, alphaPixel);
            bitmap.DrawSlash(0, 4, 1, -1, 5, alphaPixel);

            return bitmap;
        }

        #endregion
    }
}