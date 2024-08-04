namespace XLib.Base.Ex
{
    public static class DoubleExtension
    {
        /// <summary>
        /// 将双精度浮点数限制在指定范围内
        /// </summary>
        public static double Limit(this double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static double Round(this double doubleValue) => Math.Round(doubleValue);

        /// <summary>
        /// 将双精度浮点型四舍五入至整型
        /// </summary>
        public static int RoundInt(this double value) => (int)Math.Round(value);

        public static byte RoundByte(this double doubleValue)
        {
            // 限定范围
            doubleValue = doubleValue.Limit(0, 256);
            // 0 - 256 >> 0 - 255
            doubleValue = doubleValue / 256 * 255;
            return (byte)Math.Round(doubleValue);
        }

        /// <summary>
        /// 四舍五入至指定精度
        /// </summary>
        public static double RoundWithDigits(this double value, int digits = 1) => Math.Round(value, digits);
    }
}