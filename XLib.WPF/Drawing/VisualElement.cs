using System.Windows.Media;

namespace XLib.WPF.Drawing
{
    /// <summary>
    /// 可视元素
    /// </summary>
    public abstract class VisualElement : DrawingVisual
    {
        public void Update()
        {
            using DrawingContext context = RenderOpen();
            OnUpdate(context);
        }

        public void Clear() => RenderOpen().Close();

        protected abstract void OnUpdate(DrawingContext context);
    }
}