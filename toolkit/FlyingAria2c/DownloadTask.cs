using FlyingAria2c.Aria2Lib;
using System;
using System.Threading.Tasks;

namespace FlyingAria2c
{
    public class DownloadTask
    {
        private string Gid;

        private long totalLength = 0;

        private long completedLength = 0;

        public event Action<DownloadTask> Complete;

        public enum TaskAction
        {
            Active,
            Waiting,
            Paused,
            Error,
            Complete,
            Removed
        }

        public TaskAction Status
        {
            get
            {
                switch (status)
                {
                    case "active":
                        return TaskAction.Active;
                    case "waiting":
                        return TaskAction.Waiting;
                    case "paused":
                        return TaskAction.Paused;
                    case "error":
                        return TaskAction.Error;
                    case "complete":
                        return TaskAction.Complete;
                    case "removed":
                        return TaskAction.Removed;
                    default:
                        return TaskAction.Error;
                }
            }
        }
        private string status;

        public double Progress
        {
            get
            {
                if (totalLength == 0)
                {
                    return 0;
                }
                else
                {
                    double i = completedLength * 100 / totalLength;
                    return i;
                }
            }
        }

        public string DownloadSpeed
        {
            get
            {
                double SpeedLong = downloadSpeed;
                if (SpeedLong / 1024 == 0)
                {
                    return Math.Round(SpeedLong, 2).ToString() + "B/S";
                }
                else if (SpeedLong / 1048576 == 0)
                {
                    return Math.Round((SpeedLong / 1024), 2).ToString() + "KB/S";
                }
                else
                {
                    return Math.Round((SpeedLong / 1048578), 2).ToString() + "MB/S";
                }
            }
        }
        private long downloadSpeed = 0;

        protected async Task RefreshStatus()
        {
            System.Collections.Generic.Dictionary<string, string> x = await Aria2Methords.TellStatus(Downloader.RpcConnection, Gid);
            status = x["status"];
            totalLength = long.Parse(x["totalLength"]);
            completedLength = long.Parse(x["completedLength"]);
            downloadSpeed = long.Parse(x["downloadSpeed"]);
            if (Status == TaskAction.Complete)
            {
                Complete?.Invoke(this);
                Complete = null;
            }
        }

        public async Task<string> GetFilePath()
        {
            File[] x = await Aria2Methords.GetFiles(Downloader.RpcConnection, Gid);
            return x[0].Path;
        }

        private async void Create(string DownloadAddress)
        {
            Gid = await Aria2Methords.AddUri(Downloader.RpcConnection, DownloadAddress);
            await RefreshStatus();
        }

        public DownloadTask(string DownloadAddress, Action<DownloadTask> CompletedEventHandle = null)
        {
            Create(DownloadAddress);
            Complete += CompletedEventHandle;
        }
    }
}