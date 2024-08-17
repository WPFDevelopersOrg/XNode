using System.Windows;
using XNode.AppTool;
using XNode.SubSystem.WindowSystem;

namespace XNode
{
    public partial class App : Application
    {
        #region 构造方法

        public App()
        {
            Startup += App_Startup;
        }

        #endregion

        #region 应用程序事件

        private void App_Startup(object sender, StartupEventArgs e)
        {
            // 启动异常
            bool startException = false;
            try
            {
                Init();
            }
            catch (Exception ex)
            {
                startException = true;
                WM.ShowAppError("软件启动异常：" + ex.Message);
            }
            finally
            {
                if (startException) Shutdown();
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化应用程序
        /// </summary>
        private void Init()
        {
            // 初始化应用程序代理
            AppDelegate.Init();

            // 初始化系统数据
            SystemDataDelegate.Instance.Init();
            // 启动系统服务
            SystemServiceDelegate.Instance.Start();
            // 初始化系统工具
            SystemToolDelegate.Instance.Init();
        }

        #endregion
    }
}