namespace XLib.Math.Easing
{
    /// <summary>
    /// 缓动函数接口
    /// </summary>
    public interface IEasingFunction
    {
        public EasingMode Mode { get; set; }

        public double Ease(double t);
    }

    /// <summary>
    /// 缓动函数基类
    /// </summary>
    public abstract class EaseBase : IEasingFunction
    {
        public EasingMode Mode { get; set; }

        public abstract double Ease(double t);
    }

    /// <summary>
    /// 直线。即无缓动
    /// </summary>
    public class LinearEase : EaseBase
    {
        public override double Ease(double t) => t;
    }

    /// <summary>
    /// 二次方
    /// </summary>
    public class QuadraticEase : EaseBase
    {
        public override double Ease(double t)
        {
            return Mode switch
            {
                EasingMode.EaseIn => EF.InQuad(t),
                EasingMode.EaseOut => EF.OutQuad(t),
                EasingMode.EaseInOut => EF.InOutQuad(t),
                _ => t,
            };
        }
    }

    /// <summary>
    /// 三次方
    /// </summary>
    public class CubicEase : EaseBase
    {
        public override double Ease(double t)
        {
            return Mode switch
            {
                EasingMode.EaseIn => EF.InCubic(t),
                EasingMode.EaseOut => EF.OutCubic(t),
                EasingMode.EaseInOut => EF.InOutCubic(t),
                _ => t,
            };
        }
    }

    /// <summary>
    /// 四次方
    /// </summary>
    public class QuarticEase : EaseBase
    {
        public override double Ease(double t)
        {
            return Mode switch
            {
                EasingMode.EaseIn => EF.InQuart(t),
                EasingMode.EaseOut => EF.OutQuart(t),
                EasingMode.EaseInOut => EF.InOutQuart(t),
                _ => t,
            };
        }
    }

    /// <summary>
    /// 五次方
    /// </summary>
    public class QuinticEase : EaseBase
    {
        public override double Ease(double t)
        {
            return Mode switch
            {
                EasingMode.EaseIn => EF.InQuint(t),
                EasingMode.EaseOut => EF.OutQuint(t),
                EasingMode.EaseInOut => EF.InOutQuint(t),
                _ => t,
            };
        }
    }

    /// <summary>
    /// 正弦
    /// </summary>
    public class SineEase : EaseBase
    {
        public override double Ease(double t)
        {
            return Mode switch
            {
                EasingMode.EaseIn => EF.InSine(t),
                EasingMode.EaseOut => EF.OutSine(t),
                EasingMode.EaseInOut => EF.InOutSine(t),
                _ => t,
            };
        }
    }

    /// <summary>
    /// 振荡
    /// </summary>
    public class ElasticEase : EaseBase
    {
        public override double Ease(double t)
        {
            return Mode switch
            {
                EasingMode.EaseIn => EF.InElastic(t),
                EasingMode.EaseOut => EF.OutElastic(t),
                EasingMode.EaseInOut => EF.InOutElastic(t),
                _ => t,
            };
        }
    }

    /// <summary>
    /// 弹跳
    /// </summary>
    public class BounceEase : EaseBase
    {
        public override double Ease(double t)
        {
            return Mode switch
            {
                EasingMode.EaseIn => EF.InBounce(t),
                EasingMode.EaseOut => EF.OutBounce(t),
                EasingMode.EaseInOut => EF.InOutBounce(t),
                _ => t,
            };
        }
    }

    /// <summary>
    /// 后退
    /// </summary>
    public class BackEase : EaseBase
    {
        public override double Ease(double t)
        {
            return Mode switch
            {
                EasingMode.EaseIn => EF.InBack(t),
                EasingMode.EaseOut => EF.OutBack(t),
                EasingMode.EaseInOut => EF.InOutBack(t),
                _ => t,
            };
        }
    }

    /// <summary>
    /// 圆形
    /// </summary>
    public class CircleEase : EaseBase
    {
        public override double Ease(double t)
        {
            return Mode switch
            {
                EasingMode.EaseIn => EF.InCirc(t),
                EasingMode.EaseOut => EF.OutCirc(t),
                EasingMode.EaseInOut => EF.InOutCirc(t),
                _ => t,
            };
        }
    }
}