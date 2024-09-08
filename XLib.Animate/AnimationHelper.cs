using XLib.Math.Easing;

namespace XLib.Animate
{
    public static class AnimationHelper
    {
        /// <summary>
        /// 创建动画
        /// </summary>
        public static Animation CreateAnimation(this IMotion source, string property, double begin, double end, double duration,
            EasingType type = EasingType.LinearEase, EasingMode mode = EasingMode.EaseIn, double delay = 0, double wait = 0, bool loop = false)
        {
            Animation animation = new Animation(source)
            {
                MotionProperty = property,
                BeginValue = begin,
                EndValue = end,
                Duration = duration,
                EasingFunction = GetEasingFunction(type, mode),
                StartDelay = delay,
                EndDelay = wait,
                Loop = loop
            };
            animation.Init();
            return animation;
        }

        /// <summary>
        /// 添加动画
        /// </summary>
        public static void Motion(this IMotion source, string property, double begin, double end, double duration,
            EasingType type = EasingType.LinearEase, EasingMode mode = EasingMode.EaseIn, double delay = 0, bool loop = false)
        {
            // 创建动画
            Animation animation = new Animation(source)
            {
                MotionProperty = property,
                BeginValue = begin,
                EndValue = end,
                Duration = duration,
                EasingFunction = GetEasingFunction(type, mode),
                StartDelay = delay,
                Loop = loop
            };
            animation.Init();
            // 添加动画
            AnimationEngine.Instance.AddAnimation(animation);
        }

        /// <summary>
        /// 从当前值运动至
        /// </summary>
        public static void MotoinTo(this IMotion source, string property, double end, double duration,
            EasingType type = EasingType.LinearEase, EasingMode mode = EasingMode.EaseIn)
        {
            double begin = source.GetMotionProperty(property);
            source.Motion(property, begin, end, duration, type, mode);
        }

        private static IEasingFunction GetEasingFunction(EasingType type, EasingMode mode)
        {
            return type switch
            {
                EasingType.LinearEase => new LinearEase { Mode = mode },
                EasingType.QuadraticEase => new QuadraticEase { Mode = mode },
                EasingType.CubicEase => new CubicEase { Mode = mode },
                EasingType.QuarticEase => new QuarticEase { Mode = mode },
                EasingType.QuinticEase => new QuinticEase { Mode = mode },
                EasingType.SineEase => new SineEase { Mode = mode },
                EasingType.ElasticEase => new ElasticEase { Mode = mode },
                EasingType.BounceEase => new BounceEase { Mode = mode },
                EasingType.BackEase => new BackEase { Mode = mode },
                EasingType.CircleEase => new CircleEase { Mode = mode },
                _ => throw new Exception("创建缓动函数失败"),
            };
        }
    }
}