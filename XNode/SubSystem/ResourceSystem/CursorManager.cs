using System.Windows;
using System.Windows.Input;
using System.Windows.Resources;

namespace XNode.SubSystem.ResourceSystem
{
    /// <summary>
    /// 光标管理器
    /// </summary>
    public class CursorManager
    {
        #region 单例

        private CursorManager() { }
        public static CursorManager Instance { get; } = new CursorManager();

        #endregion

        #region 光标

        /// <summary>选择</summary>
        public Cursor? Select { get; set; }

        /// <summary>选择并移动</summary>
        public Cursor? SelectAndMove { get; set; }

        /// <summary>移动</summary>
        public Cursor? Move { get; set; }

        /// <summary>水平移动</summary>
        public Cursor? MoveX { get; set; }

        /// <summary>垂直移动</summary>
        public Cursor? MoveY { get; set; }

        /// <summary>十字</summary>
        public Cursor? Cross { get; set; }

        /// <summary>插入</summary>
        public Cursor? Insert { get; set; }

        /// <summary>绘制</summary>
        public Cursor? Draw { get; set; }

        /// <summary>禁止</summary>
        public Cursor? Disable { get; set; }

        /// <summary>移至顶端</summary>
        public Cursor? MoveTop { get; set; }

        /// <summary>移至底端</summary>
        public Cursor? MoveBottom { get; set; }

        /// <summary>缩放：左上至右下</summary>
        public Cursor? ResizeUpDown { get; set; }

        /// <summary>缩放：左下至右上</summary>
        public Cursor? ResizeDownUp { get; set; }

        /// <summary>开关</summary>
        public Cursor? OnOff { get; set; }

        #endregion

        #region 管理器接口

        public void Init()
        {
            Select = LoadCursor("Assets/Cursor/Select.cur");
            SelectAndMove = LoadCursor("Assets/Cursor/MoveSelected.cur");
            Move = LoadCursor("Assets/Cursor/Move.cur");
            MoveX = LoadCursor("Assets/Cursor/MoveX.cur");
            MoveY = LoadCursor("Assets/Cursor/MoveY.cur");
            Cross = LoadCursor("Assets/Cursor/Cross.cur");
            Insert = LoadCursor("Assets/Cursor/Insert.cur");
            Draw = LoadCursor("Assets/Cursor/Draw.cur");
            Disable = LoadCursor("Assets/Cursor/Disable.cur");
            MoveTop = LoadCursor("Assets/Cursor/MoveTop.cur");
            MoveBottom = LoadCursor("Assets/Cursor/MoveBottom.cur");
            ResizeUpDown = LoadCursor("Assets/Cursor/ResizeUpDown.cur");
            ResizeDownUp = LoadCursor("Assets/Cursor/ResizeDownUp.cur");
            OnOff = LoadCursor("Assets/Cursor/OnOff.cur");
        }

        #endregion

        #region 私有方法

        private Cursor LoadCursor(string cursorPath)
        {
            StreamResourceInfo resourceInfo = Application.GetResourceStream(new Uri(cursorPath, UriKind.Relative));
            return new Cursor(resourceInfo.Stream);
        }

        #endregion
    }
}