using System.Runtime.CompilerServices;

namespace XLib.Drawing
{
    /// <summary>
    /// 位图
    /// </summary>
    public class Bitmap
    {
        #region 构造方法

        public Bitmap(int width, int height)
        {
            Width = width;
            Height = height;

            if (Width < 1 || Height < 1) throw new Exception("图片太小");
            _pixelArray = new Pixel[Width, Height];
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    _pixelArray[x, y] = new Pixel();
        }

        #endregion

        #region 属性

        /// <summary>宽度</summary>
        public int Width { get; set; } = 1;

        /// <summary>高度</summary>
        public int Height { get; set; } = 1;

        #endregion

        #region 绘图方法

        /// <summary>
        /// 填充画布
        /// </summary>
        public void Fill(byte r = 255, byte g = 255, byte b = 255, byte a = 255)
        {
            foreach (var pixel in _pixelArray)
            {
                pixel.R = r;
                pixel.G = g;
                pixel.B = b;
                pixel.A = a;
            }
        }

        /// <summary>
        /// 绘制像素点
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawPixel(int x, int y, Pixel pixel) => _pixelArray[x, y].Blend(pixel);

        /// <summary>
        /// 绘制水平线：从左至右
        /// </summary>
        public void DrawHorizontalLine(int x, int y, int length, Pixel pixel)
        {
            // 防止越界
            if (x < 0) x = 0;
            int end_x = x + length;
            if (end_x > Width) end_x = Width;
            // 绘制像素点
            for (int col = x; col < end_x; col++) DrawPixel(col, y, pixel);
        }

        /// <summary>
        /// 绘制垂直线：从上至下
        /// </summary>
        public void DrawVerticalLine(int x, int y, int length, Pixel pixel)
        {
            // 防止越界
            if (y < 0) y = 0;
            int end_y = y + length;
            if (end_y > Height) end_y = Height;
            // 绘制像素点
            for (int row = y; row < end_y; row++) DrawPixel(x, row, pixel);
        }

        /// <summary>
        /// 绘制正斜线
        /// </summary>
        public void DrawSlash(int x, int y, int delta_x, int delta_y, int length, Pixel pixel)
        {
            // 当前像素点坐标
            int current_x = x;
            int current_y = y;
            for (int counter = 0; counter < length; counter++)
            {
                // 绘制像素点
                DrawPixel(current_x, current_y, pixel);
                // 更新坐标
                current_x += delta_x;
                current_y += delta_y;
            }
        }

        /// <summary>
        /// 绘制两点直线
        /// </summary>
        public void DrawLine(int x1, int y1, int x2, int y2, Pixel pixel)
        {
            int xDelta = x2 - x1;
            int yDelta = y2 - y1;
            float steps;

            if (Math.Abs(xDelta) > Math.Abs(yDelta)) steps = Math.Abs(xDelta);
            else steps = Math.Abs(yDelta);

            float xIncrement = xDelta / steps;
            float yIncrement = yDelta / steps;

            float x = x1;
            float y = y1;
            for (int step = 0; step <= steps; step++)
            {
                DrawPixel((int)Math.Round(x), (int)Math.Round(y), pixel);
                x += xIncrement;
                y += yIncrement;
            }
        }

        /// <summary>
        /// 绘制矩形：左上至右下
        /// </summary>
        public void DrawRectangle(int x, int y, int width, int height, Pixel pixel)
        {
            for (int row = y; row <= height; row++)
                for (int col = x; col <= width; col++)
                    DrawPixel(col, row, pixel);
        }

        /// <summary>
        /// 清除画布
        /// </summary>
        public void Clear()
        {
            foreach (var pixel in _pixelArray) pixel.Reset();
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 获取像素点数据
        /// </summary>
        public byte[] GetPixelData()
        {
            byte[] result = new byte[Width * Height * 4];
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    int index = (row * Width + col) * 4;
                    result[index + 0] = _pixelArray[col, row].B;
                    result[index + 1] = _pixelArray[col, row].G;
                    result[index + 2] = _pixelArray[col, row].R;
                    result[index + 3] = _pixelArray[col, row].A;
                }
            }
            return result;
        }

        #endregion

        #region 字段

        /// <summary>像素点数组</summary>
        private readonly Pixel[,] _pixelArray = new Pixel[1, 1];

        #endregion
    }
}