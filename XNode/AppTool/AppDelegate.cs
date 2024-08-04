using System.Windows;
using XLib.Base;

namespace XNode.AppTool
{
    /// <summary>
    /// 应用程序代理
    /// </summary>
    public class AppDelegate
    {
        public static App Main { get; set; }

        public static string Version { get; private set; } = "1.0.0";

        /// <summary>名称比较器</summary>
        public static NaturalComparator NameComparator { get; set; } = new NaturalComparator();

        public static void Init()
        {
            if (Application.Current is App app) Main = app;
        }

        public static void Invoke(Action action)
        {
            Main.Dispatcher.Invoke(action);
        }

        public static void BeginInvoke(Action action)
        {
            Main.Dispatcher.BeginInvoke(action);
        }
    }
}