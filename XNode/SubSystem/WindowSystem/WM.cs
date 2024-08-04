using System.Media;
using System.Windows;
using XLib.WPF.WindowDefine;

namespace XNode.SubSystem.WindowSystem
{
    /// <summary>
    /// 窗口管理器
    /// </summary>
    public class WM
    {
        /// <summary>主窗口</summary>
        public static MainWindow Main { get; set; }

        /// <summary>
        /// 显示应用程序错误
        /// </summary>
        public static void ShowAppError(string tip)
        {
            TipDialog dialog = new TipDialog
            {
                Message = tip,
                Level = TipLevel.Error
            };
            SystemSounds.Hand.Play();
            dialog.ShowDialog();
        }

        public static void ShowTip(string tip, Window? owner = null, TipLevel level = TipLevel.Info)
        {
            TipDialog dialog = new TipDialog
            {
                Message = tip,
                Level = level,
            };
            try
            {
                dialog.Owner = owner ?? Main;
            }
            catch (Exception)
            {
                dialog.Owner = null;
            }
            if (level is TipLevel.Info or TipLevel.Warning) SystemSounds.Asterisk.Play();
            else SystemSounds.Hand.Play();

            _tipDialogList.Add(dialog);
            dialog.Closed += (_, _) => _tipDialogList.Remove(dialog);
            dialog.Show();
        }

        /// <summary>
        /// 显示错误提示
        /// </summary>
        public static void ShowError(string tip, Window? owner = null)
        {
            ShowTip(tip, owner, TipLevel.Error);
        }

        /// <summary>
        /// 显示询问框
        /// </summary>
        public static bool? ShowAsk(string message, string yesText = "是", bool useCancel = true, TipLevel level = TipLevel.Info)
        {
            AskDialog dialog = new AskDialog
            {
                Message = message,
                Owner = Main,
                YesText = yesText,
                Level = level,
                UseCancel = useCancel
            };
            if (level is TipLevel.Info or TipLevel.Warning) SystemSounds.Asterisk.Play();
            else SystemSounds.Hand.Play();
            dialog.ShowDialog();

            bool? result = null;
            if (dialog.Result == AskResult.Yes) result = true;
            else if (dialog.Result == AskResult.No) result = false;
            return result;
        }

        /// <summary>提示框列表</summary>
        private static readonly List<XDialog> _tipDialogList = new List<XDialog>();
    }
}