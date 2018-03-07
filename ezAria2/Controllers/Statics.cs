using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace ezAria2
{
    public class Stc//保存所有的全局资源
    {
        public static JRCtler Line;
        public static ConfigInformation GloConf;
        public static ProgressController ProCtl;
        static Process Aria2Process;
        public static void Quit()
        {
            Task.Factory.StartNew(async () =>
            {
                await Aria2Methords.ShutDown();
            });
            Aria2Process.Kill();
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
            Start.Make();
            Aria2Process = new Process();
            Aria2Process.StartInfo.FileName = @"aria2c.exe";
            Aria2Process.StartInfo.CreateNoWindow = true;
            Aria2Process.StartInfo.Arguments = @"--conf-path=aria2.conf";
            Aria2Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Aria2Process.Start();
            Stc.GloConf = Start.Configs;//给全局配置变量赋值，同时初始化所有其他全局变量

            Line = new JRCtler(string.Format("ws://127.0.0.1:{0}/jsonrpc", GloConf.rpc_listen_port));
            ProCtl = new ProgressController();

        }
    }

}
