using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ezAria2
{
    /// <summary>
    /// 保存所有的全局资源
    /// </summary>
    public class Stc
    {
        private static Process Aria2Process;//aria2c的进程

        public static JRCtler Line;
        public static ConfigInformation GloConf;
        public static ProgressController ProCtl;//托盘
        public static DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public static TaskList TaskData;
        public static HistoryList HistoryData;
        public static ApplicationConfig Config=new ApplicationConfig();

        private static void Quit(object sender, ExitEventArgs e)//程序关闭事件
        {
            Line.Quit();
            ProCtl.Dispose();
            Aria2Methords.ShutDown();
        }

        private static void Crash(object sender, DispatcherUnhandledExceptionEventArgs e)//程序崩溃事件
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
                File.Create(Start.Configs.save_session).Close();
            }
            if (!File.Exists(Start.Configs.input_file))
            {
                File.Create(Start.Configs.input_file).Close();
            }
            if (!Directory.Exists(Start.Configs.dir))
            {
                Directory.CreateDirectory(Start.Configs.dir);
            }
            if (!File.Exists(@"HistoryList.log"))
            {
                File.Create(@"HistoryList.log").Close();
            }
            Start.Make();
            Aria2Process = new Process();
            Aria2Process.StartInfo.FileName = Config.Aria2cPath;
            Aria2Process.StartInfo.CreateNoWindow = true;
            Aria2Process.StartInfo.Arguments = @"--conf-path="+ Config.Aria2cConfigPath;
            Aria2Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Aria2Process.Start();
            Stc.GloConf = Start.Configs;//给全局配置变量赋值，同时初始化所有其他全局变量
            Line = new JRCtler(string.Format("ws://127.0.0.1:{0}/jsonrpc", GloConf.rpc_listen_port));//这里的逻辑重新梳理后，可以允许ezAria2作为客户端，控制其他主机的下载服务
            ProCtl = new ProgressController();
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            Application.Current.Exit += new ExitEventHandler(Quit);
            Application.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(Crash);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            TaskData = new TaskList();
            HistoryData = new HistoryList();
        }
    }

}
