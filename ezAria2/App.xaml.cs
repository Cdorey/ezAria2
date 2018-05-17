using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace ezAria2
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        static Mutex mutex;
        public App()
        {
            mutex = new Mutex(true, "ezAria2 GUI",out bool IsSuccessful);
            if (!IsSuccessful)
            {
                mutex.Close();
                var instance = RunningInstance();
                //将实例放置到前台
                //HandleRunningInstance(instance);
                Shutdown();
            }
        }

        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //Loop through the running processes in with the same name
            foreach (Process process in processes)
            {
                //Ignore the current process
                if (process.Id != current.Id)
                {
                    //Make sure that the process is running from the exe file.
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", @"\") == current.MainModule.FileName)
                    {
                        //Return the other process instance.
                        return process;
                    }
                }
            }
            //No other instance was found, return null.
            return null;
        }

        //public static void HandleRunningInstance(Process instance)
        //{
        //    ShowWindowAsync(instance.MainWindowHandle, WS_SHOWNORMAL); //显示，可以注释掉
        //    SetForegroundWindow(instance.MainWindowHandle);            //放到前端
        //}

        //[DllImport("User32.dll")]
        //private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        //[DllImport("User32.dll")]
        //internal static extern bool SetForegroundWindow(IntPtr hWnd);
        //private const int WS_SHOWNORMAL = 1;

    }
}
