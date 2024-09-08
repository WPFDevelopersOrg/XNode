using System.Windows;
using System.Windows.Input;
using XLib.Sample.Layer;

namespace XLib.Sample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadingLayer loadingLayer = new LoadingLayer { Margin = new Thickness(50) };
            loadingLayer.Init();
            MainGrid.Children.Add(loadingLayer);
            loadingLayer.Start();
        }
    }
}