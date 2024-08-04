namespace XLib.Base.Ex
{
    public static class IntExtension
    {
        /// <summary>
        /// 将整数限制在指定范围内
        /// </summary>
        public static int Limit(this int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        /// <summary>
        /// 将毫秒转为时间字符串
        /// </summary>
        public static string ToTimeString(this int ms) => TimeSpan.FromMilliseconds(ms).ToString(StringEx.TimeFormat3);

        public static string ToShortTimeString(this int ms) => TimeSpan.FromSeconds(ms).ToString(StringEx.ShortTime);
    }
}