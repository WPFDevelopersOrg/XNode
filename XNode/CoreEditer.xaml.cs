using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XNode
{
    public partial class CoreEditer : UserControl
    {
        public CoreEditer()
        {
            InitializeComponent();
            Loaded += CoreEditer_Loaded;
        }

        #region 控件事件

        private void CoreEditer_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化核心编辑器
        /// </summary>
        private void Init()
        {
            // 初始化编辑器面板
            Panel_NodeEditer.Init();
            // 初始化节点库面板
            Panel_NodeLib.Init();
        }

        #endregion

        #region 字段



        #endregion
    }
}