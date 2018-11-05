using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ezAria2
{
    public class Aria2cTask : INotifyPropertyChanged
    {
        public string Gid { get; set; }//任务的GID

        /// <summary>
        /// 枚举文件名的来源
        /// </summary>
        protected enum FileNameSources
        {
            Path,
            Uri
        }

        /// <summary>
        /// 当前任务的文件名的来源，可能是通过URL推测，也可能是来自ARIA2C返回的文件路径
        /// </summary>
        protected FileNameSources FileNameSource;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public enum TaskType
        {
            Http,
            BitTorrent,
            MetaLink,
            Ed2k
        }

        /// <summary>
        /// 指示该任务的来源，可能是URL，可能是BT或MetaLink
        /// </summary>
        public TaskType Type { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 尝试获取当前任务的文件名
        /// </summary>
        protected virtual async void GetFileInfo()
        {
            JRCtler.JsonRpcRes x = await Aria2Methords.GetFiles(Gid);
            if (x.Result.Count == 1)
            {
                string filepath = x.Result[0].path;
                if (filepath != null)
                {
                    FileName = filepath.Substring(filepath.LastIndexOf(@"/") + 1);
                    FileNameSource = FileNameSources.Path;
                }
                else
                {
                    string uri = x.Result[0].uris[0].uri;
                    FileName = uri.Substring(uri.LastIndexOf(@"/") + 1);
                    FileNameSource = FileNameSources.Uri;
                }
            }
            else
            {
                string path = x.Result[0].path;
                int count = x.Result.Count;
                FileName = path.Substring(path.LastIndexOf(@"/") + 1) + "等，共计" + count.ToString() + "个文件";
            }
            OnPropertyChanged("FileName");
        }

        public Aria2cTask()
        {

        }
    }

    /// <summary>
    /// 小型任务对象,用于任务列表
    /// </summary>
    public class TaskLite : Aria2cTask
    {
        protected string Completed;//已完成的尺寸

        protected string Total;//任务的尺寸

        /// <summary>
        /// aria2c返回的任务状态，包含更多信息
        /// </summary>
        protected string Status;

        /// <summary>
        /// 归零时刷新
        /// </summary>
        protected int OughtToRefresh = 0;

        public delegate void NotifyStatusChanged(TaskLite Sender, string e);

        public event NotifyStatusChanged OnStatusChanged;//当前任务的status变成completed时触发

        public string State { get; set; }//任务的状态

        /// <summary>
        /// 下载文件的图标
        /// </summary>
        public string Icon
        {
            get
            {
                switch (Type)
                {
                    case TaskType.Http:
                        return "Resources/Icon70/Download.png";
                    case TaskType.BitTorrent:
                        return "Resources/Icon70/bt.png";
                    case TaskType.MetaLink:
                        return "Resources/Icon70/link.png";
                    default:
                        return "Resources/Icon70/web.png";
                }
            }
        }

        /// <summary>
        /// 当前速度
        /// </summary>
        public string Speed { get; set; }

        public double Progress
        {
            get
            {
                if (Total==null)
                {
                    return 0;
                }
                else if(long.Parse(Total) == 0)
                {
                    return 0;
                }
                else
                {
                    double i = long.Parse(Completed) * 100 / long.Parse(Total);
                    return i;
                }
            }
        }

        /// <summary>
        /// 解析一个RPC调用结果以更新当前任务
        /// downloadSpeed
        /// status
        /// completedLength
        /// totalLength
        /// gid
        /// </summary>
        /// <param name="e"></param>
        protected virtual void InformationRefresh(JRCtler.JsonRpcRes e)
        {
            if (e.Result!=null)
            {
                //计算当前下载速度
                double SpeedLong = e.Result.downloadSpeed;
                if (SpeedLong / 1024 == 0)
                    Speed = Math.Round(SpeedLong, 2).ToString() + "B/S";
                else if (SpeedLong / 1048576 == 0)
                    Speed = Math.Round((SpeedLong / 1024), 2).ToString() + "KB/S";
                else
                    Speed = Math.Round((SpeedLong / 1048578), 2).ToString() + "MB/S";
                OnPropertyChanged("Speed");

                //更新状态
                string NewStatus = e.Result.status;
                if (Status != NewStatus)
                {
                    Status = NewStatus;
                    switch (Status)
                    {
                        case "active":
                            State = "none";
                            break;
                        case "waiting":
                            State = "wait";
                            OughtToRefresh = 5;
                            break;
                        case "paused":
                            State = "error";
                            OughtToRefresh = 5;
                            break;
                        case "error":
                            State = "error";
                            OughtToRefresh = 10;
                            break;
                        case "complete":
                            State = "none";
                            OnStatusChanged?.Invoke(this, "complete");
                            OughtToRefresh = 10;
                            break;
                        case "removed":
                            State = "error";
                            OnStatusChanged?.Invoke(this, "removed");
                            OughtToRefresh = 10;
                            break;
                        default:
                            State = "wait";
                            OughtToRefresh = 5;
                            break;
                    }
                    OnPropertyChanged("State");
                }

                //更新进度
                string CompletedNew = e.Result.completedLength;
                Total = e.Result.totalLength;
                if (CompletedNew != Completed)
                {
                    Completed = CompletedNew;
                    OnPropertyChanged("Progress");
                }
                else if (CompletedNew == "0")
                {
                    State = "wait";
                    OnPropertyChanged("State");
                }

                Gid = e.Result.gid;
                if (FileName == null || FileNameSource != FileNameSources.Path)
                    GetFileInfo();
            }
            else
            {
                OnStatusChanged?.Invoke(this, "null");
            }
        }

        /// <summary>
        /// 刷新任务状态
        /// </summary>
        /// <returns></returns>
        public virtual async Task RefreshAsync()
        {
            if (OughtToRefresh <= 0)
            {
                JRCtler.JsonRpcRes e = await Aria2Methords.TellStatus(Gid);
                InformationRefresh(e);
            }
            else
            {
                OughtToRefresh--;
            }
        }

        protected virtual async void Refresh()
        {
            await Task.Run(async () =>
            {
                while(true)
                {
                    await RefreshAsync();
                    System.Threading.Thread.Sleep(1000);
                }
            });
        }

        /// <summary>
        /// 这个方法用于暂停和开始任务，已知问题：如果一个磁力任务当前没有速度，则无法正确暂停
        /// </summary>
        public async void StateChangeFunction()
        {
            if (Status == "active")
            {
                await Aria2Methords.Pause(Gid);
                //State = "error";
            }
            else
            {
                await Aria2Methords.UpPause(Gid);
                //State = "none";
            }
            OughtToRefresh = 0;
            OnPropertyChanged("State");
        }

        /// <summary>
        /// 移除当前任务
        /// </summary>
        /// <returns></returns>
        public async Task Remove()
        {
            await Aria2Methords.Remove(Gid);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="e">使用该Json Rpc请求结果初始化任务对象</param>
        public TaskLite(JRCtler.JsonRpcRes e)
        {
            InformationRefresh(e);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gid">使用该Gid初始化任务对象并拉取状态</param>
        public TaskLite(string gid)
        {
            Gid = gid;
            Refresh();
        }

        public TaskLite()
        {

        }
    }

    public sealed class TaskList : ObservableCollection<TaskLite>//任务列表
    {
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public delegate void TaskFinish(TaskLite e);

        public event TaskFinish TaskFinished;//有任务状态为completed时触发

        private Queue<TaskLite> FinishedTaskList = new Queue<TaskLite>();//该队列内容为已完成待移除的任务

        private List<string> GidList = new List<string>();//该列表用于存储已经添加过的任务GID，避免重复添加

        private void StatusChangedHandle(TaskLite Sender, string e)//当任务状态有变化时的处理程序
        {
            FinishedTaskList.Enqueue(Sender);
            if (e=="complete")
            {
                TaskFinished(Sender);
            }
        }

        private void CheckRefreshResult(JRCtler.JsonRpcRes e)//检查Refresh方法中异步返回的结果
        {
            foreach (dynamic s in e.Result)
            {
                JRCtler.JsonRpcRes a = new JRCtler.JsonRpcRes
                {
                    Result = s
                };
                string g = a.Result.gid;
                if (GidList.Contains(g) != true)
                {
                    GidList.Add(g);
                    string Status = a.Result.status;
                    if (Status != "complete")
                    {
                        Add(new TaskLite(a));
                    }
                    else
                    {
                        TaskFinished?.Invoke(new TaskLite(a));
                    }
                }

            }
        }

        private async void Refresh()//更新任务列表
        {
            JRCtler.JsonRpcRes x = await Aria2Methords.TellActive();
            CheckRefreshResult(x);
            JRCtler.JsonRpcRes y = await Aria2Methords.TellWaiting();
            CheckRefreshResult(y);
            JRCtler.JsonRpcRes z = await Aria2Methords.TellStopped();
            CheckRefreshResult(z);
        }

        public async Task Update()//历遍单个任务，调用其刷新方法
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].OnStatusChanged += StatusChangedHandle;
                await this[i].RefreshAsync();
                this[i].OnStatusChanged -= StatusChangedHandle;
            }
            while (FinishedTaskList.Count != 0)
            {
                Remove(FinishedTaskList.Dequeue());//直接在for或foreach循环中进行remove操作会导致异常
            }
            Refresh();
        }

        /// <summary>
        /// 获得一个当前任务列表对象的拷贝
        /// </summary>
        /// <returns></returns>
        public TaskList Clone()
        {
            return (TaskList)MemberwiseClone();
        }

        public TaskList()
        {
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
    }

    /// <summary>
    /// 一个完整版本的任务信息
    /// </summary>
    public class TaskInformation : TaskLite
    {
        protected override void InformationRefresh(JRCtler.JsonRpcRes e)
        {
            if (e.Result != null)
            {
                //计算当前下载速度
                double SpeedLong = e.Result.downloadSpeed;
                if (SpeedLong / 1024 == 0)
                    Speed = Math.Round(SpeedLong, 2).ToString() + "B/S";
                else if (SpeedLong / 1048576 == 0)
                    Speed = Math.Round((SpeedLong / 1024), 2).ToString() + "KB/S";
                else
                    Speed = Math.Round((SpeedLong / 1048578), 2).ToString() + "MB/S";
                OnPropertyChanged("Speed");

                //更新状态
                string NewStatus = e.Result.status;
                if (Status != NewStatus)
                {
                    Status = NewStatus;
                    switch (Status)
                    {
                        case "active":
                            State = "正在下载";
                            break;
                        case "waiting":
                            State = "等待下载";
                            OughtToRefresh = 5;
                            break;
                        case "paused":
                            State = "任务暂停";
                            OughtToRefresh = 5;
                            break;
                        case "error":
                            State = "下载错误";
                            OughtToRefresh = 10;
                            break;
                        case "complete":
                            State = "下载完成";
                            OughtToRefresh = 10;
                            break;
                        case "removed":
                            State = "已经删除";
                            OughtToRefresh = 10;
                            break;
                        default:
                            State = "状态未知";
                            OughtToRefresh = 5;
                            break;
                    }
                    OnPropertyChanged("State");
                }
                //更新进度
                string CompletedNew = e.Result.completedLength;
                Total = e.Result.totalLength;
                if (CompletedNew != Completed)
                {
                    Completed = CompletedNew;
                    OnPropertyChanged("Progress");
                }
                else if (CompletedNew == "0")
                {
                    State = "等待下载";
                    OnPropertyChanged("State");
                }

                Gid = e.Result.gid;
                if (FileName == null || FileNameSource != FileNameSources.Path)
                    GetFileInfo();
            }
        }

        public override async Task RefreshAsync()
        {
            JRCtler.JsonRpcRes e = await Aria2Methords.TellStatus(Gid);
            InformationRefresh(e);
        }

        protected override async void Refresh()
        {
            await RefreshAsync();
        }

        public ObservableCollection<string> FilesList { get; set; }

        public string FileCount
        {
            get
            {
                return FilesList.Count.ToString()+"个文件";
            }
        }

        public ObservableCollection<string> Peers { get; set; }

        public string EstimatedRemainingTime
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// 尝试获取当前任务的文件名
        /// </summary>
        protected override async void GetFileInfo()
        {
            JRCtler.JsonRpcRes x = await Aria2Methords.GetFiles(Gid);
            if (x.Result.Count != 0)
            {
                string filepath = x.Result[0].path;
                if (filepath != null)
                {
                    foreach (dynamic a in x.Result)
                    {
                        string path = a.path;
                        string s = path.Substring(path.LastIndexOf(@"/") + 1);
                        FilesList.Add(s);
                        OnPropertyChanged("FileCount");
                    }
                }
                else
                {
                    string uri = x.Result[0].uris[0].uri;
                    FileName = uri.Substring(uri.LastIndexOf(@"/") + 1);
                    FileNameSource = FileNameSources.Uri;
                }
            }
            else
            {
                string FirstFilePath = x.Result[0].path;
                int count = x.Result.Count;
                FileName = FirstFilePath.Substring(FirstFilePath.LastIndexOf(@"/") + 1) + "等，共计" + count.ToString() + "个文件";
            }
            OnPropertyChanged("FileName");
            OnPropertyChanged("FilesList");
        }

        /// <summary>
        /// 获取速度信息
        /// </summary>
        /// <returns>x[0]下载速度，x[1]上传速度</returns>
        public async Task<long[]> GetSpeed()
        {
            JRCtler.JsonRpcRes x = await Aria2Methords.TellStatus(Gid, new string[2]
            {
                "downloadSpeed",
                "uploadSpeed"
            });
            return new long[]
            {
                x.Result.downloadSpeed,
                x.Result.uploadSpeed
            };
        }

        public TaskInformation(TaskLite e)
        {
            Gid = e.Gid;
            Refresh();
            FilesList = new ObservableCollection<string>();
            Peers = new ObservableCollection<string>();
        }

        //public TaskInformation(string e)
        //{
        //    Gid = e;
        //    Refresh();
        //    FilesList = new ObservableCollection<string>();
        //    Peers = new ObservableCollection<string>();
        //}
    }

    public class FinishedTask : Aria2cTask//已完成任务
    {

        private BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            BitmapImage image = new BitmapImage();
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bytes = ms.GetBuffer();
            ms.Close();
            image.BeginInit();
            image.StreamSource = new MemoryStream(bytes);
            image.EndInit();
            return image;
        }

        public BitmapImage FileIcon
        {
            get
            {
                Bitmap picture = Icon.ExtractAssociatedIcon(Path).ToBitmap();
                picture.MakeTransparent(picture.GetPixel(1, 1));
                return BitmapToBitmapImage(picture);
            }
        }

        public string Path { get; set; }//文件路径

        public string FileSize //文件尺寸
        {
            get
            {
                return filesize;
            }
            set
            {
                double FileSizeValue = double.Parse(value);
                if (FileSizeValue < 1024)
                {
                    filesize = Math.Round(FileSizeValue, 2).ToString() + "B";
                }
                else if (FileSizeValue < 1024 * 1024)
                {
                    filesize = Math.Round((FileSizeValue / 1024), 2).ToString() + "KB";
                }
                else if (FileSizeValue < (1024 * 1024 * 1024))
                {
                    filesize = Math.Round((FileSizeValue / 1024 / 1024), 2).ToString() + "MB";
                }
                else
                {
                    filesize = Math.Round((FileSizeValue / (1024 * 1024 * 1024)), 2).ToString() + "GB";
                }
            }
        }
        private string filesize;
    }

    public sealed class HistoryList : ObservableCollection<FinishedTask>//历史任务列表
    {
        /// <summary>
        /// 归零时保存
        /// </summary>
        private int KickCount = 0;

        private static readonly string HistorayListIsSaving = "HistorayListIsSaving";

        private List<string> FinishedGidList = new List<string>();//该列表用于存储已经添加过的任务GID，避免重复添加

        private void Save(Object Sender, EventArgs e)//读写代码有待优化
        {
            if (KickCount <= 60)
            {
                KickCount++;
            }
            else
            {
                lock (HistorayListIsSaving)
                {
                    IList<FinishedTask> Historys = Items;
                    File.WriteAllText(@"HistoryList.log", JsonConvert.SerializeObject(Historys));
                    KickCount = 0;
                }
            }
        }

        private void Load()
        {
            //Clear();
            //List<FinishedTask> Historys = null;
            //Historys = JsonConvert.DeserializeObject<List<FinishedTask>>(File.ReadAllText(@"HistoryList.log"));
            //if (Historys != null)
            //{
            //    foreach (FinishedTask a in Historys)
            //    {
            //        Add(a);
            //    }
            //}
            //try
            //{
            //    Historys = JsonConvert.DeserializeObject<List<FinishedTask>>(File.ReadAllText(@"HistoryList.log"));
            //    if(Historys!=null)
            //    {
            //        foreach (FinishedTask a in Historys)
            //        {
            //            Add(a);
            //        }
            //    }
            //}
            //catch (IOException)
            //{
            //    Thread.Sleep(15);
            //    Load();
            //}
        }

        public async void TaskCompleted(TaskLite e)
        {
            if (FinishedGidList.Contains(e.Gid) == false)
            {
                var y = await Aria2Methords.GetFiles(e.Gid);
                Newtonsoft.Json.Linq.JArray results = y.Result;
                foreach (dynamic x in results)
                {
                    FinishedTask a = new FinishedTask
                    {
                        Path = x.path,
                        FileSize = x.length,
                        Gid = e.Gid
                    };
                    a.FileName = a.Path.Substring(a.Path.LastIndexOf(@"/") + 1);
                    Add(a);
                }
                FinishedGidList.Add(e.Gid);
            }

        }

        public HistoryList()//任务列表的构造函数，实际使用时应当修改
        {
            Load();
            //CollectionChanged += Save;
            //dispatcherTimer.Tick += new EventHandler(Save);
        }
    }

}