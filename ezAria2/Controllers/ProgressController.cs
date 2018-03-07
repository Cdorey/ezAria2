using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace ezAria2
{
    public class ProgressController
    {
        private NotifyIcon NotifyIcon;
        private void InitialTray()
        {
            //设置托盘的各个属性
            NotifyIcon = new NotifyIcon
            {
                BalloonTipText = "服务运行中...",//托盘气泡显示内容
                BalloonTipTitle = "EzAria2",
                Text = "EzAria2",
                Visible = true,//托盘按钮是否可见
                Icon = SystemIcons.Question,//托盘中显示的图标
            };
            NotifyIcon.ShowBalloonTip(500);//托盘气泡显示时间
            NotifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MainWindow MainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            if (MainWindow==null)
            {
                MainWindow = new MainWindow();
                MainWindow.Show();
            }

        }
        public ProgressController()
        {
            InitialTray();
        }
        ~ProgressController()
        {
            NotifyIcon.Dispose();
        }
    }
}
