using XLib.WPF;
using XLib.WPF.Behavior;

namespace XLib.WPFControl.Tool
{
    public class SelectTool : ToolBase<TreeView>
    {
        public SelectTool(TreeView host) : base(host) { }

        public override void Init()
        {
            // 开始拖动 -> 松开
            NewTree("开始拖动", (_) =>
            {
                _host.BeginDragItem();
                _host.Capture();
            });
            NewNode(Behaviors.LeftUp, (_) =>
            {
                ResetTree();
                _host.ReleaseCapture();
            });
            // 开始拖动 -> 移动 -> 松开
            BackToRoot();
            NewNode(Behaviors.Move, (_) =>
            {
                switch (_host.GetMouseSide())
                {
                    case MouseSide.InSide:
                        _host.DragItem();
                        break;
                    case MouseSide.OutSide:
                        _host.DragItemTo();
                        break;
                }
            });
            NewNode(Behaviors.LeftUp, (_) =>
            {
                ResetTree();
                _host.ReleaseCapture();
                _host.EndDragItem();
            });
            Finish();
        }

        public override void OnLeftButtonDown()
        {
            if (_host.HasSelected())
            {
                // 只允许拖动文件
                if (_host.DragFileOnly)
                {
                    // 判断选中项是否包含文件夹
                    if (!FolderSelected()) Invoke("开始拖动", null);
                }
                else Invoke("开始拖动", null);
            }
        }

        private bool FolderSelected()
        {
            foreach (var item in _host.SelectedItemList)
                if (item.IsFolder) return true;
            return false;
        }
    }
}