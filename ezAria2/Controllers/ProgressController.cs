using System;
using System.Drawing;
using System.Windows.Forms;

namespace ezAria2
{
    public class ProgressController//这个类控制托盘按钮
    {
        private NotifyIcon NotifyIcon;
        private void InitialTray()
        {
            //菜单项在这里
            ContextMenu ContextMenu = new ContextMenu();
            //ContextMenu.MenuItems.Add(new MenuItem("粘贴"));
            //ContextMenu.MenuItems.Add(new MenuItem("粘贴"));
            ContextMenu.MenuItems.Add(new MenuItem("退出", new EventHandler(Quit)));
            //设置托盘的各个属性
            NotifyIcon = new NotifyIcon
            {
                BalloonTipText = "服务运行中...",//托盘气泡显示内容
                BalloonTipTitle = "ezAria2",
                Text = "ezAria2",
                Visible = true,//托盘按钮是否可见
                Icon = Properties.Resources.cbuod_gwj75_004
                //SystemIcons.Question,//托盘中显示的图标
            };
            NotifyIcon.ShowBalloonTip(500);//托盘气泡显示时间
            NotifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
            NotifyIcon.ContextMenu = ContextMenu;
            NotifyIcon.DoubleClick += new EventHandler(ShowMainWindow);
        }

        private void ShowMainWindow(object sender, EventArgs e)
        {
            var MainWindow = System.Windows.Application.Current.MainWindow;
            if (MainWindow==null)
            {
                MainWindow = new MainWindow();
                MainWindow.Show();
            }
            else
            {
                //如果MainWindow存在，则将其置顶
            }
        }

        private void Quit(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
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
        public void Dispose()
        {
            NotifyIcon.Dispose();
        }
        ~ProgressController()
        {
            Dispose();
        }
    }
}
