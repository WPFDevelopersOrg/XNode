namespace XLib.Drawing
{
    /// <summary>
    /// 像素点
    /// </summary>
    public class Pixel
    {
        #region 构造方法

        public Pixel() { }

        public Pixel(byte r, byte g, byte b, byte a = 255)
        {
            _r = r;
            _g = g;
            _b = b;
            _a = a;
        }

        #endregion

        #region 属性

        public byte R { get => _r; set => _r = value; }

        public byte G { get => _g; set => _g = value; }

        public byte B { get => _b; set => _b = value; }

        public byte A { get => _a; set => _a = value; }

        #endregion

        #region 颜色

        /// <summary>黑色</summary>
        public static Pixel Black => new Pixel { _r = 0, _g = 0, _b = 0, _a = 255 };

        /// <summary>白色</summary>
        public static Pixel White => new Pixel { _r = 255, _g = 255, _b = 255, _a = 255 };

        #endregion

        #region 公开方法

        /// <summary>
        /// 混合像素点：无预乘
        /// 算法链接：https://zh.wikipedia.org/wiki/Alpha%E5%90%88%E6%88%90
        /// </summary>
        public void Blend(Pixel pixel)
        {
            double alpha_overlay = pixel._a / 255.0;
            double factor = _a / 255.0 * (1 - alpha_overlay);
            double alpha_result = alpha_overlay + factor;

            _r = (byte)Math.Round((pixel._r / 255.0 * alpha_overlay + _r / 255.0 * factor) / alpha_result * 255);
            _g = (byte)Math.Round((pixel._g / 255.0 * alpha_overlay + _g / 255.0 * factor) / alpha_result * 255);
            _b = (byte)Math.Round((pixel._b / 255.0 * alpha_overlay + _b / 255.0 * factor) / alpha_result * 255);
            _a = (byte)Math.Round(alpha_result * 255);
        }

        /// <summary>
        /// 覆盖像素点
        /// </summary>
        public void Cover(Pixel pixel)
        {
            _r = pixel._r;
            _g = pixel._g;
            _b = pixel._b;
            _a = pixel._a;
        }

        /// <summary>
        /// 判断颜色是否相同
        /// </summary>
        public bool SameColor(Pixel otherPixel)
        {
            if (_r != otherPixel._r) return false;
            if (_g != otherPixel._g) return false;
            if (_b != otherPixel._b) return false;
            if (_a != otherPixel._a) return false;
            return true;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            _r = 0;
            _g = 0;
            _b = 0;
            _a = 0;
        }

        #endregion

        #region 属性字段

        private byte _r = 0;
        private byte _g = 0;
        private byte _b = 0;
        private byte _a = 0;

        #endregion
    }
}