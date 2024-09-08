namespace XLib.Math.Easing
{
    /// <summary>
    /// 缓动函数
    /// </summary>
    public static class EF
    {
        #region 二次方

        public static double InQuad(double t) => t * t;
        public static double OutQuad(double t) => 1 - InQuad(1 - t);
        public static double InOutQuad(double t)
        {
            if (t < 0.5) return InQuad(t * 2) / 2;
            return 1 - InQuad((1 - t) * 2) / 2;
        }

        #endregion

        #region 三次方

        public static double InCubic(double t) => t * t * t;
        public static double OutCubic(double t) => 1 - InCubic(1 - t);
        public static double InOutCubic(double t)
        {
            if (t < 0.5) return InCubic(t * 2) / 2;
            return 1 - InCubic((1 - t) * 2) / 2;
        }

        #endregion

        #region 四次方

        public static double InQuart(double t) => t * t * t * t;
        public static double OutQuart(double t) => 1 - InQuart(1 - t);
        public static double InOutQuart(double t)
        {
            if (t < 0.5) return InQuart(t * 2) / 2;
            return 1 - InQuart((1 - t) * 2) / 2;
        }

        #endregion

        #region 五次方

        public static double InQuint(double t) => t * t * t * t * t;
        public static double OutQuint(double t) => 1 - InQuint(1 - t);
        public static double InOutQuint(double t)
        {
            if (t < 0.5) return InQuint(t * 2) / 2;
            return 1 - InQuint((1 - t) * 2) / 2;
        }

        #endregion

        #region 正弦

        public static double InSine(double t) => 1 - (double)System.Math.Cos(t * System.Math.PI / 2);
        public static double OutSine(double t) => (double)System.Math.Sin(t * System.Math.PI / 2);
        public static double InOutSine(double t) => (double)(System.Math.Cos(t * System.Math.PI) - 1) / -2;

        // 振荡

        public static double InElastic(double t) => 1 - OutElastic(1 - t);
        public static double OutElastic(double t)
        {
            double p = 0.3;
            return (double)System.Math.Pow(2, -10 * t) * (double)System.Math.Sin((t - p / 4) * (2 * System.Math.PI) / p) + 1;
        }
        public static double InOutElastic(double t)
        {
            if (t < 0.5) return InElastic(t * 2) / 2;
            return 1 - InElastic((1 - t) * 2) / 2;
        }

        #endregion

        #region 弹跳

        public static double InBounce(double t) => 1 - OutBounce(1 - t);
        public static double OutBounce(double t)
        {
            double div = 2.75;
            double mult = 7.5625;

            if (t < 1 / div)
            {
                return mult * t * t;
            }
            else if (t < 2 / div)
            {
                t -= 1.5 / div;
                return mult * t * t + 0.75;
            }
            else if (t < 2.5 / div)
            {
                t -= 2.25 / div;
                return mult * t * t + 0.9375;
            }
            else
            {
                t -= 2.625 / div;
                return mult * t * t + 0.984375;
            }
        }
        public static double InOutBounce(double t)
        {
            if (t < 0.5) return InBounce(t * 2) / 2;
            return 1 - InBounce((1 - t) * 2) / 2;
        }

        #endregion

        #region 后退

        public static double InBack(double t)
        {
            double s = 1.70158;
            return t * t * ((s + 1) * t - s);
        }
        public static double OutBack(double t) => 1 - InBack(1 - t);
        public static double InOutBack(double t)
        {
            if (t < 0.5) return InBack(t * 2) / 2;
            return 1 - InBack((1 - t) * 2) / 2;
        }

        #endregion

        #region 圆形

        public static double InCirc(double t) => -((double)System.Math.Sqrt(1 - t * t) - 1);
        public static double OutCirc(double t) => 1 - InCirc(1 - t);
        public static double InOutCirc(double t)
        {
            if (t < 0.5) return InCirc(t * 2) / 2;
            return 1 - InCirc((1 - t) * 2) / 2;
        }

        #endregion
    }
}