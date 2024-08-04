using System.Windows.Input;
using XLib.WPF.Behavior;

namespace XLib.WPF
{
    /// <summary>
    /// 鼠标滚轮行为参数
    /// </summary>
    public class MouseWheelBehaviorArgs : BehaviorArgs
    {
        public MouseWheelEventArgs? WheelArgs { get; set; }
    }

    /// <summary>
    /// 工具基类
    /// </summary>
    public abstract class ToolBase
    {
        /// <summary>当前行为树</summary>
        public BehaviorTree? CurrentTree => _handler.CurrentTree;

        /// <summary>重置完成</summary>
        public Action<ToolBase>? ResetFinish { get; set; } = null;

        /// <summary>
        /// 初始化
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// 重置
        /// </summary>
        public virtual void Reset() => ResetTree();

        /// <summary>
        /// 清理
        /// </summary>
        public virtual void Clear() { }

        #region 输入事件

        /// <summary>
        /// 鼠标按下
        /// </summary>
        public virtual void OnMouseDown(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    OnLeftButtonDown();
                    break;
                case MouseButton.Middle:
                    _handler.Invoke(Behaviors.MiddleDown, null);
                    break;
                case MouseButton.Right:
                    OnRightButtonDown();
                    break;
            }
        }

        /// <summary>
        /// 鼠标左键双击
        /// </summary>
        public virtual void OnDoubleClick()
        {
            _handler.Invoke(Behaviors.DoubleClick, null);
        }

        /// <summary>
        /// 鼠标左键按下
        /// </summary>
        public virtual void OnLeftButtonDown()
        {
            _handler.Invoke(Behaviors.LeftDown, null);
        }

        /// <summary>
        /// 鼠标右键按下
        /// </summary>
        public virtual void OnRightButtonDown()
        {
            _handler.Invoke(Behaviors.RightDown, null);
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        public virtual void OnMouseMove()
        {
            _handler.Invoke(Behaviors.Move, null);
        }

        /// <summary>
        /// 鼠标松开
        /// </summary>
        public virtual void OnMouseUp(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    _handler.Invoke(Behaviors.LeftUp, null);
                    break;
                case MouseButton.Middle:
                    _handler.Invoke(Behaviors.MiddleUp, null);
                    break;
                case MouseButton.Right:
                    _handler.Invoke(Behaviors.RightUp, null);
                    break;
            }
        }

        /// <summary>
        /// 鼠标滚轮
        /// </summary>
        public virtual void OnMouseWheel(MouseWheelEventArgs e)
        {
            _handler.Invoke(Behaviors.Wheel, new MouseWheelBehaviorArgs { WheelArgs = e });
            _handler.HandleMouseWheel(e.Delta / 120);
        }

        /// <summary>
        /// 键盘按下
        /// </summary>
        public virtual void OnKeyDown(Key key) => _handler.HandleKeyDown(key.ToString());

        /// <summary>
        /// 键盘松开
        /// </summary>
        public virtual void OnKeyUp(Key key) => _handler.HandleKeyUp(key.ToString());

        /// <summary>
        /// 鼠标进入
        /// </summary>
        public virtual void OnMouseEnter() => _handler.Invoke(Behaviors.Enter, null);

        /// <summary>
        /// 鼠标离开
        /// </summary>
        public virtual void OnMouseLeave() => _handler.Invoke(Behaviors.Leave, null);

        #endregion

        #region 行为处理

        protected BehaviorNode NewTree(string name, Action<BehaviorArgs?>? action) => _handler.NewBehaviorTree(name, action);

        protected BehaviorNode NewNode(string name, Action<BehaviorArgs?>? action) => _handler.AddBehaviorNode(name, action);

        protected void BackToRoot() => _handler.BackToRoot();

        protected void Finish() => _handler.FnishAdd();

        public void Invoke(string name, BehaviorArgs? args = null) => _handler.Invoke(name, args);

        public void SetEnable(List<string> nodeList, bool enable) => _handler.SetEnable(nodeList, enable);

        protected void ResetTree()
        {
            _handler.Reset();
            ResetFinish?.Invoke(this);
        }

        /// <summary>行为处理器</summary>
        private readonly BehaviorHandler _handler = new BehaviorHandler();

        #endregion
    }

    public abstract class ToolBase<THost> : ToolBase where THost : class
    {
        public ToolBase(THost host)
        {
            Host = host;
            Init();
        }

        public THost Host { get => _host; set => _host = value; }

        protected THost _host;
    }
}