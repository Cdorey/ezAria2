using System.Drawing;
using System.Windows.Forms;

namespace ezAria2
{
    class ProgressController
    {
        #region
        private void InitialTray()
        {
            //设置托盘的各个属性
            var _notifyIcon = new NotifyIcon();
            _notifyIcon.BalloonTipText = "服务运行中...";//托盘气泡显示内容
            _notifyIcon.Text = "EzAria2";
            _notifyIcon.Visible = true;//托盘按钮是否可见
            _notifyIcon.Icon = new Icon(@"C:\Program Files (x86)\Statistics17\BasicScriptFile.ico");//托盘中显示的图标
            _notifyIcon.ShowBalloonTip(500);//托盘气泡显示时间
            _notifyIcon.MouseDoubleClick += notifyIcon_MouseDoubleClick;
        }
        #endregion

        #region 托盘图标鼠标单击事件
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }
        #endregion
        public ProgressController()
        {
            InitialTray();
        }
    }
}
