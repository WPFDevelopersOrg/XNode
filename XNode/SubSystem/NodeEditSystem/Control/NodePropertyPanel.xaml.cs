using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XLib.Node;
using XNode.SubSystem.NodeEditSystem.Control.PropertyBar;

namespace XNode.SubSystem.NodeEditSystem.Control
{
    public partial class NodePropertyPanel : UserControl
    {
        public NodePropertyPanel()
        {
            InitializeComponent();
        }

        /// <summary>节点实例</summary>
        public NodeBase? Instance { get; set; }

        /// <summary>
        /// 加载属性条
        /// </summary>
        public void LoadPropertyBar()
        {
            if (Instance == null) return;

            foreach (var property in Instance.PropertyList)
            {
                var bar = CreatePropertyBar(property);
                if (bar == null) continue;

                if (MainStackPanel.Children.Count > 0) bar.Margin = new Thickness(0, 10, 0, 0);
                MainStackPanel.Children.Add(bar);
            }
        }

        /// <summary>
        /// 清空属性条
        /// </summary>
        public void ClearPropertyBar() => MainStackPanel.Children.Clear();

        #region 控件事件

        private void MainGrid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MainScrollBar.Value -= e.Delta / 120 * 50;
        }

        private void MainStackPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (MainScrollBar != null)
            {
                MainScrollBar.ViewportSize = MainGrid.ActualHeight;
                MainScrollBar.Maximum = MainStackPanel.ActualHeight - MainGrid.ActualHeight;
            }
        }

        private void MainScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainStackPanel.Margin = new Thickness(0, -MainScrollBar.Value, 0, 0);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 创建属性条
        /// </summary>
        private PropertyBarBase? CreatePropertyBar(NodeProperty property)
        {
            PropertyBarBase? bar = null;

            switch (property.Type)
            {
                case "int":
                    bar = new TextInput();
                    bar.Title = property.Name;
                    bar.LoadProperty(property.Value);
                    ((TextInput)bar).TextChanged += text => property.Value = text;
                    break;
                case "label":
                    bar = new TextInput();
                    bar.Title = property.Name;
                    bar.LoadProperty(property.Value);
                    break;
                case "List":
                    ListSelecter selecter = new ListSelecter();
                    bar = selecter;
                    List<string> list = ((ListProperty)property).GetValueList();
                    selecter.Title = property.Name;
                    selecter.LoadProperty(list, property.Value);
                    selecter.SelectionChanged += index => ((ListProperty)property).Index = index;
                    break;
                case "CustomList":
                    ListEditer editer = new ListEditer();
                    editer.Instance = (CustomListProperty)property;
                    editer.Init();
                    bar = editer;

                    break;
            }

            return bar;
        }

        #endregion
    }
}