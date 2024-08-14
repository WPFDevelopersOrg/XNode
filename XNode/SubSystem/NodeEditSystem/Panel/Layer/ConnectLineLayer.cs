using XLib.Node;
using XLib.WPF.Drawing;

namespace XNode.SubSystem.NodeEditSystem.Panel.Layer
{
    /// <summary>
    /// 连接线图层
    /// </summary>
    public class ConnectLineLayer : DrawingBoard
    {
        public List<VisualConnectLine> ConnectLineList => _connectLineList;

        public void AddConnectLine(VisualConnectLine connectLine)
        {
            _connectLineList.Add(connectLine);
            // 添加可视元素
            AddVisualElement(connectLine);
            // 绘制可视元素
            connectLine.Update();
        }

        public void RemoveConnectLine(PinBase start, PinBase end)
        {
            int lineIndex = -1;
            int index = 0;
            // 查找连接线
            foreach (var line in _connectLineList)
            {
                if (line.StartPin == start && line.EndPin == end)
                {
                    lineIndex = index;
                    break;
                }
                index++;
            }
            // 移除连接线
            if (lineIndex != -1)
            {
                RemoveVisualElement(_connectLineList[lineIndex]);
                _connectLineList.RemoveAt(lineIndex);
            }
        }

        public void ClearConnectLine()
        {
            _connectLineList.Clear();
            ClearVisualElement();
        }

        private readonly List<VisualConnectLine> _connectLineList = new List<VisualConnectLine>();
    }
}