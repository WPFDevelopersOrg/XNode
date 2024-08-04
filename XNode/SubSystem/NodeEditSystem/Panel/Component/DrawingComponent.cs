using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using XLib.Base.UIComponent;
using XLib.Node;
using XNode.AppTool;
using XNode.SubSystem.NodeEditSystem.Control;
using XNode.SubSystem.NodeEditSystem.Define;
using XNode.SubSystem.NodeEditSystem.Panel.Layer;

namespace XNode.SubSystem.NodeEditSystem.Panel.Component
{
    public class DrawingComponent : Component<EditPanel>
    {
        #region 属性

        /// <summary>世界中心</summary>
        public Point WorldCenter => _gridLayer.GridCenter;

        /// <summary>悬停框</summary>
        public TargetBox? HoverBox
        {
            get => _hoverBoxLayer?.Box;
            set { if (_hoverBoxLayer != null) _hoverBoxLayer.Box = value; }
        }

        #endregion

        #region 生命周期

        protected override void Init()
        {
            EnableLayer();
            _host.OperateArea.SizeChanged += OperateArea_SizeChanged;
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 屏幕坐标转世界坐标
        /// </summary>
        public Point ScreenToWorld(Point screenPoint)
        {
            // 转世界坐标
            Point worldPoint = new Point(screenPoint.X - _gridLayer.GridCenter.X, screenPoint.Y - _gridLayer.GridCenter.Y);
            // 对齐至网格
            double x = Math.Round(worldPoint.X / _gridLayer.CellWidth) * _gridLayer.CellWidth;
            double y = Math.Round(worldPoint.Y / _gridLayer.CellHeight) * _gridLayer.CellHeight;
            // 返回坐标
            return new Point(x, y);
        }

        /// <summary>
        /// 更新悬停框
        /// </summary>
        public void UpdateHoverBox() => _hoverBoxLayer.Update();

        /// <summary>
        /// 清空选框
        /// </summary>
        public void ClearSelectBox() => _selectBoxLayer.Clear();

        /// <summary>
        /// 更新选框
        /// </summary>
        public void UpdateSelectBox(Point start, Point end)
        {
            _selectBoxLayer.Start = start;
            _selectBoxLayer.End = end;
            _selectBoxLayer.Update();
        }

        /// <summary>
        /// 获取选框区域
        /// </summary>
        public Rect GetSelectBoxRect() => new Rect(_selectBoxLayer.Start, _selectBoxLayer.End);

        /// <summary>
        /// 获取选择方式
        /// </summary>
        public SelectType GetSelectType() => _selectBoxLayer.End.X < _selectBoxLayer.Start.X ? SelectType.Cross : SelectType.Box;

        /// <summary>
        /// 更新选中框
        /// </summary>
        public void UpdateSelectedBox()
        {
            _selectedBoxLayer.BoxList.Clear();
            foreach (var card in GetComponent<CardComponent>().SelectedCardList)
            {
                TargetBox box = new TargetBox
                {
                    ScreenPoint = new Point(Canvas.GetLeft(card) + 9, Canvas.GetTop(card) - 2),
                    Width = card.ActualWidth - 18,
                    Height = card.ActualHeight + 4
                };
                _selectedBoxLayer.BoxList.Add(box);
            }
            _selectedBoxLayer.Update();
        }

        /// <summary>
        /// 开始绘制临时连接线
        /// </summary>
        public void BeginDrawTempConnectLine(Point start)
        {
            _tempLineLayer.Line = new ConnectLine
            {
                Start = start,
                End = start,
            };
            _tempLineLayer.Update();
        }

        /// <summary>
        /// 更新临时连接线起点
        /// </summary>
        public void UpdateTempLineStart(Point start)
        {
            if (_tempLineLayer.Line != null)
                _tempLineLayer.Line.Start = new Point(start.X, start.Y);
            _tempLineLayer.Update();
        }

        /// <summary>
        /// 设置临时连接线终点
        /// </summary>
        public void UpdateTempLineEnd(Point end)
        {
            if (_tempLineLayer.Line != null)
                _tempLineLayer.Line.End = new Point(end.X, end.Y);
            _tempLineLayer.Update();
        }

        /// <summary>
        /// 清除临时连接线
        /// </summary>
        public void ClearTempLine()
        {
            _tempLineLayer.Line = null;
            _tempLineLayer.Clear();
        }

        /// <summary>
        /// 更新连接线
        /// </summary>
        public void UpdateConnectLine()
        {
            // 清空连接线
            _connectLineLayer.ConnectLineList.Clear();
            // 遍历连接信息
            foreach (var pair in GetComponent<InteractionComponent>().ConnectInfo)
            {
                // 起点
                Point start = GetPinPoint(pair.Key);
                // 遍历目标引脚
                foreach (var targetPin in pair.Value)
                {
                    // 终点
                    Point end = GetPinPoint(targetPin);
                    // 创建连接线
                    ConnectLine connectLine = new ConnectLine
                    {
                        Start = new Point(start.X, start.Y),
                        End = new Point(end.X, end.Y),
                        IsData = pair.Key is DataPin,
                    };
                    if (connectLine.IsData) connectLine.Color = GetPinColor((DataPin)pair.Key);
                    // 添加连接线
                    _connectLineLayer.ConnectLineList.Add(connectLine);
                }
            }
            // 更新连接线图层
            _connectLineLayer.Update();
        }

        /// <summary>
        /// 拖动视口
        /// </summary>
        public void DragViewport(Point offset)
        {
            // 移动网格
            _gridLayer.MoveLayer(offset);
            // 更新节点视图
            GetComponent<CardComponent>().UpdateNodeCard();
            // 更新选中框与连接线
            UpdateSelectedBox();
            UpdateConnectLine();
        }

        /// <summary>
        /// 结束拖动
        /// </summary>
        public void EndDrag() => _gridLayer.ApplyOffset();

        #endregion

        #region 控件事件

        private void OperateArea_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateLayerSize();
            UpdateGrid();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 启用图层
        /// </summary>
        private void EnableLayer()
        {
            // 创建图层
            _gridLayer = new GridLayer();
            _connectLineLayer = new ConnectLineLayer();
            _hoverBoxLayer = new HoverBoxLayer();
            _selectedBoxLayer = new SelectedBoxLayer();
            _selectBoxLayer = new SelectBoxLayer();
            _tempLineLayer = new TempConnectLineLayer();
            // 添加图层
            Host.Layer_Base.Children.Add(_gridLayer);
            Host.Layer_Base.Children.Add(_connectLineLayer);
            Host.Layer_Box.Children.Add(_hoverBoxLayer);
            Host.Layer_Box.Children.Add(_selectedBoxLayer);
            Host.Layer_Box.Children.Add(_selectBoxLayer);
            Host.Layer_Temp.Children.Add(_tempLineLayer);
            // 更新图层尺寸
            UpdateLayerSize();
            // 更新网格
            UpdateGrid();
        }

        /// <summary>
        /// 重置图层
        /// </summary>
        private void ResetLayer()
        {
            // 重置网格
            _gridLayer.Reset();
            // 清空连接线
            _connectLineLayer.ConnectLineList.Clear();
            _connectLineLayer.Clear();
            // 清空悬停框、选框
            _hoverBoxLayer.Box = null;
            _hoverBoxLayer.Clear();
            _selectBoxLayer.Clear();
            // 清空选中框
            _selectedBoxLayer.Clear();
            _selectedBoxLayer.BoxList.Clear();
        }

        /// <summary>
        /// 更新图层尺寸
        /// </summary>
        private void UpdateLayerSize()
        {
            double width = Host.OperateArea.ActualWidth;
            double height = Host.OperateArea.ActualHeight;

            _gridLayer.Width = width;
            _gridLayer.Height = height;
            _connectLineLayer.Width = width;
            _connectLineLayer.Height = height;
            _hoverBoxLayer.Width = width;
            _hoverBoxLayer.Height = height;
            _selectBoxLayer.Width = width;
            _selectBoxLayer.Height = height;
            _selectedBoxLayer.Width = width;
            _selectedBoxLayer.Height = height;
            _tempLineLayer.Width = width;
            _tempLineLayer.Height = height;
        }

        /// <summary>
        /// 更新网格
        /// </summary>
        private void UpdateGrid() => _gridLayer.Update();

        /// <summary>
        /// 获取引脚相对于图层的坐标
        /// </summary>
        private Point GetPinPoint(PinBase pin)
        {
            // 获取引脚路径
            PinPath path = pin.GetPinPath();
            // 获取节点卡片
            NodeView card = GetComponent<CardComponent>().GetNodeCard(path.NodeID);
            // 获取引脚坐标（相对于节点）
            Point pinOffset = card.GetPinOffset(path);
            // 获取节点坐标
            Point nodePoint = new Point(Canvas.GetLeft(card), Canvas.GetTop(card));
            // 返回偏移
            return new Point(nodePoint.X + pinOffset.X, nodePoint.Y + pinOffset.Y);
        }

        /// <summary>
        /// 获取引脚颜色
        /// </summary>
        private Color GetPinColor(DataPin pin)
        {
            return ((DataPinGroup)pin.OwnerGroup).Type switch
            {
                "bool" => PinColorSet.Bool,
                "int" => PinColorSet.Int,
                "double" => PinColorSet.Double,
                "string" => PinColorSet.String,
                "byte[]" => PinColorSet.ByteArray,
                _ => Colors.White,
            };
        }

        #endregion

        #region 图层

        /// <summary>网格图层</summary>
        private GridLayer? _gridLayer;
        /// <summary>连接线图层</summary>
        private ConnectLineLayer? _connectLineLayer;
        /// <summary>悬停框图层</summary>
        private HoverBoxLayer? _hoverBoxLayer;
        /// <summary>选框图层</summary>
        private SelectBoxLayer? _selectBoxLayer;
        /// <summary>选中框图层</summary>
        private SelectedBoxLayer? _selectedBoxLayer;
        /// <summary>临时连接线图层</summary>
        private TempConnectLineLayer? _tempLineLayer;

        #endregion
    }
}