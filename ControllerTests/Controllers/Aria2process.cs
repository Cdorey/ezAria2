using System.Diagnostics;
using System.IO;

namespace ezAria2
{

    class Aria2Controller //Aria2.exe控制器，这里还有许多代码需要修改
    {
        static Process Aria2Process;
        static Aria2Controller()
        {
            ConfigController Start = new ConfigController();

            //判断几个关键的文件和路径是否存在
            if (!File.Exists(ConfigController.GlobalConfigInformation.save_session))
            {
                File.Create(ConfigController.GlobalConfigInformation.save_session);
            }
            if (!File.Exists(ConfigController.GlobalConfigInformation.input_file))
            {
                File.Create(ConfigController.GlobalConfigInformation.input_file);
            }
            if (!Directory.Exists(ConfigController.GlobalConfigInformation.dir))
            {
                Directory.CreateDirectory(ConfigController.GlobalConfigInformation.dir);
            }
            Start.Make();
            Aria2Process = new Process();
            Aria2Process.StartInfo.FileName = @"aria2c.exe";
            Aria2Process.StartInfo.CreateNoWindow = true;
            Aria2Process.StartInfo.Arguments = @"--conf-path=aria2.conf";
            Aria2Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Aria2Process.Start();
        }
        ~Aria2Controller()
        {
            Aria2Process.Kill();
        }
    }

}
