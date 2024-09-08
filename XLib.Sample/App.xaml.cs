using System.Windows;
using XLib.Animate;

namespace XLib.Sample
{
    /// <summary>
    /// 此应用程序用于演示 XLib 的功能
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            AnimationEngine.Instance.Start();
        }
    }
}