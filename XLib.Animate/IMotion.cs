namespace XLib.Animate
{
    /// <summary>
    /// 表示可运动的对象
    /// </summary>
    public interface IMotion
    {
        /// <summary>
        /// 获取运动属性值
        /// </summary>
        public double GetMotionProperty(string propertyName);

        /// <summary>
        /// 设置运动属性值
        /// </summary>
        public void SetMotionProperty(string propertyName, double value);
    }
}