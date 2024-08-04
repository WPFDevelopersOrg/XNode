using System.Windows;
using System.Windows.Media;
using XLib.WPF.Drawing;
using XNode.SubSystem.NodeEditSystem.Define;

namespace XNode.SubSystem.NodeEditSystem.Panel.Layer
{
    /// <summary>
    /// 悬停框图层
    /// </summary>
    public class HoverBoxLayer : SingleBoard
    {
        /// <summary>悬停框</summary>
        public TargetBox? Box { get; set; } = null;

        protected override void OnUpdate()
        {
            if (Box == null) return;

            List<Point> pointList = Box.GetPointList(15);
            for (int index = 0; index < pointList.Count / 2; index++)
                _dc.DrawLine(_pen, pointList[index * 2], pointList[index * 2 + 1]);
        }

        private readonly Pen _pen = new Pen(Brushes.White, 1);
    }
}