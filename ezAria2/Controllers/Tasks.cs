using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace ezAria2
{
    /// <summary>
    /// 小型任务对象,用于任务列表
    /// </summary>
    public class TaskLite : INotifyPropertyChanged
    {
        private string Completed;//已完成的尺寸

        private string Total;//任务的尺寸

        /// <summary>
        /// 归零时刷新
        /// </summary>
        private int OughtToRefresh = 0;

        public delegate void TaskFinish(TaskLite e);

        public event TaskFinish TaskFinished;//当前任务的status变成completed时触发

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
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

        public TaskType Type { get; set; }

        public string State { get; set; }//任务的状态

        /// <summary>
        /// aria2c返回的任务状态，包含更多信息
        /// </summary>
        private string Status;

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

        public string Speed { get; set; }//当前速度

        public double Progress
        {
            get
            {
                if (long.Parse(Total) == 0)
                {
                    return 0;
                }
                else
                {
                    double i = long.Parse(Completed) * 100 / long.Parse(Total);
                    return 0;
                }
            }
        }

        public string FileName { get; set; }//下载的文件名

        public string Gid { get; set; }//任务的GID

        public async void GetFileInfo()//获得文件信息
        {
            JRCtler.JsonRpcRes x = await Aria2Methords.GetFiles(Gid);
            if (x.Result.Count == 1)
            {
                string uri = x.Result[0].uris[0].uri;
                if (uri != null)
                {
                    FileName = uri.Substring(uri.LastIndexOf(@"/") + 1);
                }
                else
                {
                    string path = x.Result[0].path;
                    FileName = path.Substring(path.LastIndexOf(@"/") + 1);
                }
            }
            else
            {
                string path = x.Result[0].path;
                int count = x.Result.Count;
                FileName = path.Substring(path.IndexOf(@"/") + 1, path.LastIndexOf(@"/")) + "，共计" + count.ToString() + "个文件";
            }
            OnPropertyChanged("FileName");
        }

        public async Task Refresh()//刷新任务状态
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

        public async void StateChangeFunction()
        {
            if (State == "none")
            {
                await Aria2Methords.Pause(Gid);
                State = "error";
            }
            else
            {
                await Aria2Methords.UpPause(Gid);
                State = "none";
            }
            OughtToRefresh = 0;
            OnPropertyChanged("State");
        }

        public async Task Remove()
        {
            await Aria2Methords.Remove(Gid);
        }

        private void InformationRefresh(JRCtler.JsonRpcRes e)
        {
            //计算当前下载速度
            double SpeedLong = e.Result.downloadSpeed;
            if (SpeedLong / 1024 == 0)
            {
                Speed = Math.Round(SpeedLong, 2).ToString() + "B/S";
            }
            else if (SpeedLong / 1048576 == 0)
            {
                Speed = Math.Round((SpeedLong / 1024), 2).ToString() + "KB/S";
            }
            else
            {
                Speed = Math.Round((SpeedLong / 1048578), 2).ToString() + "MB/S";
            }
            OnPropertyChanged("Speed");

            //更新状态
            Status = e.Result.status;
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
                    State = "wait";
                    OughtToRefresh = 5;
                    break;
                case "error":
                    State = "error";
                    OughtToRefresh = 10;
                    break;
                case "complete":
                    State = "none";
                    TaskFinished?.Invoke(this);
                    OughtToRefresh = 10;
                    break;
                case "removed":
                    State = "error";
                    OughtToRefresh = 10;
                    break;
                default:
                    State = "wait";
                    OughtToRefresh = 5;
                    break;
            }

            //更新进度
            string CompletedNew = e.Result.completedLength;
            Total = e.Result.totalLength;
            if (CompletedNew != Completed)
            {
                Completed = CompletedNew;
                OnPropertyChanged("Progress");
            }

            Gid = e.Result.gid;
            GetFileInfo();
            e = null;
        }

        public TaskLite(JRCtler.JsonRpcRes e)//构造函数
        {
            InformationRefresh(e);
        }
    }

    public class TaskList : ObservableCollection<TaskLite>//任务列表
    {
        public delegate void TaskFinish(TaskLite e);

        public event TaskFinish TaskFinished;//有任务状态为completed时触发

        private Queue<TaskLite> FinishedTaskList = new Queue<TaskLite>();//该队列内容为已完成待移除的任务

        private List<string> GidList = new List<string>();//该列表用于存储已经添加过的任务GID，避免重复添加

        private void RemoveTask(TaskLite e)//当任务完成后移除并触发任务列表的TaskFinish事件
        {
            FinishedTaskList.Enqueue(e);
            TaskFinished(e);
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
                this[i].TaskFinished += RemoveTask;
                await this[i].Refresh();
                this[i].TaskFinished -= RemoveTask;
            }
            while (FinishedTaskList.Count != 0)
            {
                Remove(FinishedTaskList.Dequeue());//直接在for或foreach循环中进行remove操作会导致异常
            }
            Refresh();
        }

        public TaskList()
        {

        }

    }

    public class FinishedTask//已完成任务
    {

        public System.Drawing.Icon Icon { get; set; }//下载文件的图标

        public string FileName { get; set; }//下载的文件名

        public string Path { get; set; }//文件路径

        public string FileSize { get; set; }//文件尺寸

        public string FromGid { get; set; }//指示该下载文件来自哪个GID


    }

    public class HistoryList : ObservableCollection<FinishedTask>//历史任务列表
    {
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
            Clear();
            List<FinishedTask> Historys = null;
            Historys = JsonConvert.DeserializeObject<List<FinishedTask>>(File.ReadAllText(@"HistoryList.log"));
            if (Historys != null)
            {
                foreach (FinishedTask a in Historys)
                {
                    Add(a);
                }
            }
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
                        FromGid = e.Gid
                    };
                    a.FileName = a.Path.Substring(a.Path.LastIndexOf(@"/") + 1);
                    a.Icon = System.Drawing.Icon.ExtractAssociatedIcon(a.Path);
                    Add(a);
                }
                FinishedGidList.Add(e.Gid);
            }

        }

        public HistoryList()//任务列表的构造函数，实际使用时应当修改
        {
            Load();
            //CollectionChanged += Save;
            Stc.dispatcherTimer.Tick += new EventHandler(Save);
        }
    }

    /// <summary>
    /// JsonRpc控制器，基于WebSocket
    /// </summary>
    public class JRCtler
    {
        private class JsonRpcReq//一个请求消息对象
        {
            [JsonProperty(PropertyName = "jsonrpc")]
            public string Version { get { return "2.0"; } }
            [JsonProperty(PropertyName = "method")]
            public string Method { get; set; }
            [JsonProperty(PropertyName = "params")]
            public ArrayList Params { get; set; }
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }
            public JsonRpcReq(string n, int i, ArrayList e)
            {
                Method = n;
                Id = i;
                Params = e;
            }
        }

        public class JsonRpcRes//一个回复消息对象
        {
            [JsonProperty(PropertyName = "jsonrpc")]
            public string Version { get; set; }
            [JsonProperty(PropertyName = "method")]
            public string Method { get; set; }
            [JsonProperty(PropertyName = "result")]
            public dynamic Result { get; set; }
            [JsonProperty(PropertyName = "error")]
            public ErrorInfo Error { get; set; }
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }
            public class ErrorInfo
            {
                [JsonProperty(PropertyName = "code")]
                public string Code { get; set; }
                [JsonProperty(PropertyName = "message")]
                public string Message { get; set; }
                [JsonProperty(PropertyName = "data")]
                public string Data { get; set; }
            }
            public JsonRpcRes()
            {
                Id = -1;
            }
        }

        //资源列表
        private int Idlist = 1;//请求的ID序列号

        private Dictionary<int, JsonRpcRes> RespondList = new Dictionary<int, JsonRpcRes>();//ID-JsonRpcRes字典

        private WebSocket ws;//JsonRpc调用所使用的WebSocket链路

        public Queue<JsonRpcRes> NoticeList = new Queue<JsonRpcRes>();//通知列表

        //私有方法
        private void Send(JsonRpcReq SendMessage)//发消息
        {
            ws.Send(JsonConvert.SerializeObject(SendMessage));
        }

        private void JsonRpcMessage(string Message)//收消息
        {
            JsonRpcRes NewMessage = JsonConvert.DeserializeObject<JsonRpcRes>(Message);
            if (NewMessage.Id == -1)
            {
                NoticeList.Enqueue(NewMessage);
            }
            else
            {
                RespondList.Add(NewMessage.Id, NewMessage);
            }
            if (!File.Exists(@"log.txt"))
            {
                File.CreateText(@"log.txt").Close();
            }
            string Logs = File.ReadAllText(@"log.txt") + "Message:" + Message + Environment.NewLine;
            File.WriteAllText(@"log.txt", Logs);
        }

        //公开方法
        /// <summary>
        /// 异步开始一个远程调用
        /// </summary>
        /// <param name="methord">方法</param>
        /// <param name="Params">参数数组</param>
        /// <returns></returns>
        public async Task<JsonRpcRes> JsonRpcAsync(string methord, ArrayList Params)
        {
            JsonRpcReq NewTask = new JsonRpcReq(methord, Idlist++, Params);
            Send(NewTask);//这一行应该调用控制器的方法执行NewTask请求
            await Task.Run(() =>
            {
                while (!RespondList.ContainsKey(NewTask.Id))
                {
                    Thread.Sleep(500);
                }
            });
            return RespondList[NewTask.Id];
        }

        /// <summary>
        /// 同步开始一个远程调用
        /// </summary>
        /// <param name="methord">方法</param>
        /// <param name="Params">参数数组</param>
        /// <returns></returns>
        public JsonRpcRes JsonRpc(string methord, ArrayList Params)
        {
            JsonRpcReq NewTask = new JsonRpcReq(methord, Idlist++, Params);
            Send(NewTask);//这一行应该调用控制器的方法执行NewTask请求

            while (!RespondList.ContainsKey(NewTask.Id))
            {
                Thread.Sleep(500);
            }
            var s = RespondList[NewTask.Id];
            RespondList.Remove(NewTask.Id);
            return s;
        }

        public void Quit()
        {
            ws.Close();
        }

        //构造函数
        public JRCtler(string Uri)
        {
            ws = new WebSocket(Uri);
            ws.Connect();
            ws.OnOpen += (sender, e) =>
            {

            };
            ws.OnMessage += (sender, e) =>
            {
                JsonRpcMessage(e.Data);
            };
        }

    }

    /// <summary>
    /// Aria2 Rpc接口的方法库
    /// </summary>
    public static class Aria2Methords
    {
        private static string Base64Encode(string Path)
        {
            try
            {
                return Convert.ToBase64String(File.ReadAllBytes(Path));
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 新建一个任务
        /// </summary>
        /// <param name="Uri">下载链接的地址</param>
        /// <returns></returns>
        public static async Task<string> AddUri(string Uri)
        {
            string[] Uris = new string[] { Uri };
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.Rpc_secret,
                Uris
            };
            string Gid = (await Stc.Line.JsonRpcAsync("aria2.addUri", Params)).Result;
            return Gid;
        }

        /// <summary>
        /// 新建一个任务，包含多个源
        /// </summary>
        /// <param name="Uris">这个包含多个下载链接的数组指向同一个文件</param>
        /// <returns></returns>
        public static async Task<string> AddUri(string[] Uris)
        {
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.Rpc_secret,
                Uris
            };
            string Gid = (await Stc.Line.JsonRpcAsync("aria2.addUri", Params)).Result;
            return Gid;
        }

        /// <summary>
        /// 添加种子
        /// </summary>
        /// <param name="Path">种子文件在计算机上的位置</param>
        /// <returns></returns>
        public static async Task<string> AddTorrent(string Path)
        {
            string TorrentBase64 = Base64Encode(Path);
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.Rpc_secret,
                TorrentBase64
            };
            string Gid = (await Stc.Line.JsonRpcAsync("aria2.addTorrent", Params)).Result;
            return Gid;
        }

        /// <summary>
        /// 添加MetaLink
        /// </summary>
        /// <param name="Path">MetaLink文件在计算机上的位置</param>
        /// <returns></returns>
        public static async Task<string> AddMetalink(string Path)
        {
            string MetalinkBase64 = Base64Encode(Path);
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.Rpc_secret,
                MetalinkBase64
            };
            string Gid = (await Stc.Line.JsonRpcAsync("aria2.addMetalink", Params)).Result;
            return Gid;
        }

        public static async Task<string> Remove(string Gid)
        {
            string[] Gids = new string[] { Gid };
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.Rpc_secret,
                Gids
            };
            string Result = (await Stc.Line.JsonRpcAsync("aria2.remove", Params)).Result;
            return Result;
        }

        public static async Task<string> Pause(string Gid)
        {
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.Rpc_secret,
                Gid
            };
            string Result = (await Stc.Line.JsonRpcAsync("aria2.pause", Params)).Result;
            return Result;
        }

        public static async Task PauseAll()
        {
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.Rpc_secret,
            };
            string Result = (await Stc.Line.JsonRpcAsync("aria2.pauseAll", Params)).Result;
        }

        public static async Task<string> UpPause(string Gid)
        {
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.Rpc_secret,
                Gid
            };
            string Result = (await Stc.Line.JsonRpcAsync("aria2.unpause", Params)).Result;
            return Result;
        }

        public static async Task UpPauseAll()
        {
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.Rpc_secret,
            };
            string Result = (await Stc.Line.JsonRpcAsync("aria2.unpauseAll", Params)).Result;
        }

        public static async Task<JRCtler.JsonRpcRes> TellStatus(string Gid)
        {
            string[] Keys = new string[] { "status", "totalLength", "completedLength", "downloadSpeed", "gid" };
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.Rpc_secret,
                Gid,
                Keys
            };
            JRCtler.JsonRpcRes Result = await Stc.Line.JsonRpcAsync("aria2.tellStatus", Params);
            return Result;
        }

        public static async Task<JRCtler.JsonRpcRes> TellStatus(string Gid, string[] Keys)
        {
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.Rpc_secret,
                Gid,
                Keys
            };
            JRCtler.JsonRpcRes Result = await Stc.Line.JsonRpcAsync("aria2.tellStatus", Params);
            return Result;
        }

        public static async Task<JRCtler.JsonRpcRes> TellActive()
        {
            string[] Keys = new string[] { "status", "totalLength", "completedLength", "downloadSpeed", "gid" };
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.Rpc_secret,
                Keys
            };
            JRCtler.JsonRpcRes Result = await Stc.Line.JsonRpcAsync("aria2.tellActive", Params);
            return Result;
        }

        public static async Task<JRCtler.JsonRpcRes> TellWaiting()
        {
            string[] Keys = new string[] { "status", "totalLength", "completedLength", "downloadSpeed", "gid" };
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.Rpc_secret,
                0,
                50,
                Keys
            };
            JRCtler.JsonRpcRes Result = await Stc.Line.JsonRpcAsync("aria2.tellWaiting", Params);
            return Result;
        }

        /// <summary>
        /// 查询已停止的任务
        /// </summary>
        /// <returns>返回最近50个结果</returns>
        public static async Task<JRCtler.JsonRpcRes> TellStopped()
        {
            string[] Keys = new string[] { "status", "totalLength", "completedLength", "downloadSpeed", "gid" };
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.Rpc_secret,
                0,
                50,
                Keys
            };
            JRCtler.JsonRpcRes Result = await Stc.Line.JsonRpcAsync("aria2.tellStopped", Params);
            return Result;
        }

        public static async Task<JRCtler.JsonRpcRes> GetFiles(string Gid)
        {
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.Rpc_secret,
                Gid
            };
            JRCtler.JsonRpcRes Result = await Stc.Line.JsonRpcAsync("aria2.getFiles", Params);
            return Result;
        }

        public static async Task<JRCtler.JsonRpcRes> GetGlobalStat()
        {
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.Rpc_secret,
            };
            JRCtler.JsonRpcRes Result = await Stc.Line.JsonRpcAsync("aria2.getGlobalStat", Params);
            return Result;

        }

        /// <summary>
        /// 关闭Aria2C，调用Aria2自己的方法
        /// </summary>
        public static void ShutDown()
        {
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.Rpc_secret,
            };
            JRCtler.JsonRpcRes Result = Stc.Line.JsonRpc("aria2.shutdown", Params);
        }

    }
}