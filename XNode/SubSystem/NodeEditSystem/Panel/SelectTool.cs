using System.Windows.Input;
using XLib.WPF;
using XLib.WPF.Behavior;
using XNode.SubSystem.NodeEditSystem.Panel.Component;

namespace XNode.SubSystem.NodeEditSystem.Panel
{
    public class SelectTool : ToolBase<InteractionComponent>
    {
        public SelectTool(InteractionComponent host) : base(host) { }

        /// <summary>光标</summary>
        public Cursor Cursor { get; set; }

        public override void Init()
        {
            移动();

            命中空白();
            命中节点();
            命中引脚();
            双击节点();

            中键按下();

            右键引脚();
        }

        public override void OnLeftButtonDown()
        {
            switch (_host.GetHitedArea())
            {
                case Define.MouseHitedArea.Space:
                    Invoke(Behaviors.HitedSpace);
                    break;
                case Define.MouseHitedArea.Node:
                    Invoke("命中节点");
                    break;
                case Define.MouseHitedArea.Pin:
                    Invoke("命中引脚");
                    break;
            }
        }

        public override void OnDoubleClick()
        {
            if (_host.GetHitedArea() == Define.MouseHitedArea.Space)
                Invoke("双击空白处");
            else if (_host.GetHitedArea() == Define.MouseHitedArea.Node)
                Invoke("双击节点");
        }

        public override void OnRightButtonDown()
        {
            if (_host.GetHitedArea() == Define.MouseHitedArea.Pin)
                Invoke("右键引脚");
        }

        private void 移动()
        {
            NewTree(Behaviors.Move, (_) =>
            {
                _host.HandleMouseMove();
                ResetTree();
            });
            Finish();
        }

        private void 命中空白()
        {
            NewTree(Behaviors.HitedSpace, (_) =>
            {
                _host.RemoveNodeFocus();
                if (Keyboard.Modifiers != ModifierKeys.Control) _host.ClearSelect();
                _host.BeginDrawSelectBox();
                _host.CaptureOperationLayer();
            });
            NewNode(Behaviors.LeftUp, (_) =>
            {
                _host.CancelDrawSelectBox();
                _host.ReleaseOperationLayer();
                ResetTree();
            });
            BackToRoot();
            NewNode(Behaviors.Move, (_) =>
            {
                _host.DrawSelectBox();
            });
            NewNode(Behaviors.LeftUp, (_) =>
            {
                _host.EndDrawSelectBox();
                _host.HandleMouseMove();
                _host.ReleaseOperationLayer();
                ResetTree();
            });
            Finish();
        }

        private void 命中节点()
        {
            NewTree("命中节点", (_) =>
            {
                // 清除悬停框
                _host.ClearHoverBox();
                // 按下了“Ctrl”键，加选或减选
                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    if (_host.CurrentNodeSelected()) _host.RemoveSelect();
                    else _host.AddSelect();
                }
                // 当前节点未选中
                else if (!_host.CurrentNodeSelected())
                {
                    _host.ClearSelect();
                    _host.SetTop();
                    _host.AddSelect();
                }
                // 开始拖动节点
                _host.BeginDragNode();
                _host.CaptureOperationLayer();
            });
            NewNode(Behaviors.LeftUp, (_) =>
            {
                _host.CancelDragNode();
                _host.HandleMouseMove();
                _host.ReleaseOperationLayer();
                ResetTree();
            });
            BackToRoot();
            NewNode(Behaviors.Move, (_) => _host.DragNode());
            NewNode(Behaviors.LeftUp, (_) =>
            {
                _host.EndDragNode();
                _host.HandleMouseMove();
                _host.ReleaseOperationLayer();
                ResetTree();
            });
            Finish();
        }

        private void 命中引脚()
        {
            NewTree("命中引脚", (_) =>
            {
                _host.ClearHoverBox();
                // 开始绘制连接线
                _host.BeginDrawConnectLine();
            });
            NewNode(Behaviors.LeftUp, (_) =>
            {
                // 取消绘制连接线
                _host.CancelDrawConnectLine();
                _host.HandleMouseMove();
                ResetTree();
            });
            BackToRoot();
            NewNode(Behaviors.Move, (_) =>
            {
                // 绘制连接线
                _host.DrawConnectLine();
            });
            NewNode(Behaviors.LeftUp, (_) =>
            {
                // 结束绘制连接线
                _host.EndDrawConnectLine();
                _host.HandleMouseMove();
                ResetTree();
            });
            Finish();
        }

        private void 双击节点()
        {
            NewTree("双击节点", (_) =>
            {
                _host.StartAndExecute();
                ResetTree();
            });
            Finish();
        }

        private void 中键按下()
        {
            NewTree(Behaviors.MiddleDown, (_) =>
            {
                _host.ClearHoverBox();
                _host.BeginDragViewport();
                _host.CaptureOperationLayer();
            });
            NewNode(Behaviors.MiddleUp, (_) =>
            {
                _host.ReleaseOperationLayer();
                _host.CancelDragViewport();
                _host.HandleMouseMove();
                ResetTree();
            });
            BackToRoot();
            NewNode(Behaviors.Move, (_) =>
            {
                _host.DragViewport();
            });
            NewNode(Behaviors.MiddleUp, (_) =>
            {
                _host.ReleaseOperationLayer();
                _host.EndDragViewport();
                _host.HandleMouseMove();
                ResetTree();
            });
            Finish();
        }

        private void 右键引脚()
        {
            NewTree("右键引脚", (_) =>
            {
                _host.ClearHoverBox();
                _host.BeginBreakPin();
            });
            NewNode(Behaviors.RightUp, (_) =>
            {
                _host.EndBreakPin();
                _host.HandleMouseMove();
                ResetTree();
            });
            BackToRoot();
            NewNode(Behaviors.Move, (_) =>
            {
                _host.CancelBreakPin();
            });
            NewNode(Behaviors.RightUp, (_) =>
            {
                _host.HandleMouseMove();
                ResetTree();
            });
            Finish();
        }
    }
}