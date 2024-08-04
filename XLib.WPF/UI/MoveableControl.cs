using System.Windows;
using System.Windows.Controls;

namespace XLib.WPF.UI
{
    /// <summary>
    /// 表示可移动的用户控件
    /// </summary>
    public class MoveableControl : UserControl
    {
        #region 属性

        /// <summary>当前坐标</summary>
        public Point Point
        {
            get => new(_real.X + _offset.X, _real.Y + _offset.Y);
            set
            {
                _real = value;
                _offset = new Point();
            }
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 设置偏移
        /// </summary>
        public void SetOffset(Point offset)
        {
            _offset = offset;
        }

        /// <summary>
        /// 应用偏移
        /// </summary>
        public void ApplyOffset()
        {
            _real.X += _offset.X;
            _real.Y += _offset.Y;
            _offset = new Point();
            OnOffsetChanged();
        }

        #endregion

        protected virtual void OnOffsetChanged() { }

        #region 字段

        /// <summary>真实坐标</summary>
        private Point _real = new Point();
        /// <summary>偏移坐标</summary>
        private Point _offset = new Point();

        #endregion
    }
}