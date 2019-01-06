using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace FlyingAria2c.Aria2Lib
{
    internal class Aria2cProgressHandle
    {
        private Process aria2cProcess;

        /// <summary>
        /// RPC接口
        /// </summary>
        internal int Port { get; private set; }

        /// <summary>
        /// RPC令牌
        /// </summary>
        internal string Token { get; private set; }

        /// <summary>
        /// 当进程对象被销毁时发生
        /// </summary>
        internal event Action ProgressHandleDisposing;

        /// <summary>
        /// 获取一个随机可用的端口
        /// </summary>
        /// <returns></returns>
        private int GetRandomUnusedPort()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        private void Start(string Aria2cPath, string DefaultDownloadPath)
        {
            string TempPath = Path.GetTempPath();
            if (!System.IO.File.Exists(TempPath + "aria2.session"))
            {
                System.IO.File.Create(TempPath + "aria2.session").Close();
            }
            aria2cProcess = new Process();
            aria2cProcess.StartInfo.FileName = Aria2cPath;
            aria2cProcess.StartInfo.CreateNoWindow = true;
            //拼接启动参数
            aria2cProcess.StartInfo.Arguments = string.Format("--enable-rpc true --rpc-listen-port={0} --rpc-secret={1} -d {2} -i {3} -c true --save-session={3}", Port, Token, DefaultDownloadPath, TempPath + "aria2.session");
            aria2cProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            aria2cProcess.Start();
        }

        /// <summary>
        /// 主程序崩溃时关闭aria2c.exe进程
        /// </summary>
        public void Crash(object sender, EventArgs e)
        {
            if (!aria2cProcess.HasExited)
            {
                aria2cProcess.Kill();
            }
        }

        public Aria2cProgressHandle(string Aria2cPath, string DefaultDownloadPath)
        {
            //Creating random token for aria2c rpc secret
            Random creator = new Random();
            string charsToUse = "AzByCxDwEvFuGtHsIrJqKpLoMnNmOlPkQjRiShTgUfVeWdXcYbZa1234567890";
            int i = 1;
            Token = "";
            while (i <= 16)
            {
                int a = creator.Next(0, 61);
                string b = charsToUse.Substring(a, 1);
                Token = Token + b;
                i = i + 1;
            }
            //Confirming that download path is existed
            if (!Directory.Exists(DefaultDownloadPath))
            {
                Directory.CreateDirectory(DefaultDownloadPath);
            }

            Port = GetRandomUnusedPort();
            Start(Aria2cPath, DefaultDownloadPath);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Crash);
            //AppDomain.CurrentDomain.
        }

        ~Aria2cProgressHandle()
        {
            ProgressHandleDisposing?.Invoke();
            if (!aria2cProcess.HasExited)
            {
                aria2cProcess.Kill();
            }
        }
    }
}