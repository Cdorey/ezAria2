using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ezAria2
{
    public class Stc//保存所有的全局资源
    {
        public static JRCtler Line;
        public static ConfigInformation GloConf;
        public static ProgressController ProCtl;
        static Process Aria2Process;
        public static DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public static TaskList TaskData;
        public static HistoryList HistoryData;
        public static void Quit(object sender,ExitEventArgs e)//程序关闭事件
        {
            Aria2Process.Kill();
            ProCtl.Dispose();
        }

        static Stc()
        {
            ConfigController Start = new ConfigController();
            //判断几个关键的文件和路径是否存在
            if (!File.Exists(Start.Configs.save_session))
            {
                File.Create(Start.Configs.save_session);
            }
            if (!File.Exists(Start.Configs.input_file))
            {
                File.Create(Start.Configs.input_file);
            }
            if (!Directory.Exists(Start.Configs.dir))
            {
                Directory.CreateDirectory(Start.Configs.dir);
            }
            if (!File.Exists(@"HistoryList.log"))
            {
                File.Create(@"HistoryList.log");
            }
            Start.Make();
            Aria2Process = new Process();
            Aria2Process.StartInfo.FileName = @"aria2c.exe";
            Aria2Process.StartInfo.CreateNoWindow = true;
            Aria2Process.StartInfo.Arguments = @"--conf-path=aria2.conf";
            Aria2Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Aria2Process.Start();
            Stc.GloConf = Start.Configs;//给全局配置变量赋值，同时初始化所有其他全局变量
            Line = new JRCtler(string.Format("ws://127.0.0.1:{0}/jsonrpc", GloConf.rpc_listen_port));//这里的逻辑重新梳理后，可以允许ezAria2作为客户端，控制其他主机的下载服务
            ProCtl = new ProgressController();
            Application.Current.Exit += new ExitEventHandler(Quit);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            TaskData = new TaskList();
            HistoryData = new HistoryList();
        }
        ~Stc()
        {
            Aria2Process.Kill();
        }
    }

}
