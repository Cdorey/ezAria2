using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WebSocketSharp;

namespace ezAria2
{
    public class TaskLite//小型任务对象,用于任务列表
    {
        public string State { get; set; }//任务的状态

        public string Status { get; set; }//aria2c返回的任务状态，包含更多信息

        public string Icon { get; set; }//下载文件的图标

        public string Speed { get; set; }//当前速度

        public double Progress { get; set; } //进度，双精度浮点数

        public string FileName { get; set; }//下载的文件名

        public string Gid { get; set; }//任务的GID

        public string Completed { get; set; }//已完成的尺寸

        public string Total { get; set; }//任务的尺寸

        public async void GetFileInfo()
        {
            JRCtler.JsonRpcRes x = await Aria2Methords.GetFiles(Gid);
            string uri = x.Result[0].uris[0].uri;
            FileName=uri.Substring(uri.LastIndexOf(@"/")+1);
            //if (x.Result.GetLength(0)!=1)
            //{
            //    FileName = FileName + "等";
            //}
        }

        public async Task Refresh()
        {
            JRCtler.JsonRpcRes e = await Aria2Methords.TellStatus(Gid);
            InformationRefresh(e);
        }

        public void StateChangeFunction()
        {
            StateChange();
        }

        private async void StateChange()
        {
            if(State=="none")
            {
                await Aria2Methords.Pause(Gid);
                State = "error";
            }
            else
            {
                await Aria2Methords.UpPause(Gid);
                State = "none";
            }
        }

        private void InformationRefresh(JRCtler.JsonRpcRes e)
        {
            Completed = e.Result.completedLength;
            Total = e.Result.totalLength;
            string Status = e.Result.status;
            string SpeedString = e.Result.downloadSpeed;
            double SpeedLong = double.Parse(SpeedString);
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
            switch (Status)
            {
                case "active":
                    State = "none";
                    Icon = "Resources/bonfire-1849089_640.png";
                    break;
                case "waiting":
                    State = "wait";
                    Icon = "Resources/stopwatch-1849088_640.png";
                    break;
                case "paused":
                    State = "error";
                    Icon = "Resources/cup-1849083_640.png";
                    break;
                case "error":
                    State = "error";
                    Icon = "Resources/cup-1849083_640.png";
                    break;
                case "complete":
                    State = "none";
                    Icon = "Resources/stopwatch-1849088_640.png";
                    break;
                case "removed":
                    State = "error";
                    Icon = "Resources/cup-1849083_640.png";
                    break;
                default:
                    State = "wait";
                    Icon = "Resources/cup-1849083_640.png";
                    break;
            }
            if (e.Result.totalLength == 0)
            {
                Progress = 0;
            }
            else
            {
                double i = long.Parse(Completed) * 100 / long.Parse(Total);
                Progress = i;
            }
            Gid = e.Result.gid;
            GetFileInfo();
        }

        public TaskLite(JRCtler.JsonRpcRes e)//构造函数
        {
            InformationRefresh(e);
        }
    }

    public class TaskList : ObservableCollection<TaskLite>//任务列表
    {
        public delegate void TaskFinish(TaskLite e);

        public event TaskFinish TaskFinished;

        public async void Refresh()//清除并刷新任务列表
        {
            Clear();
            JRCtler.JsonRpcRes x = await Aria2Methords.TellActive();
            foreach (dynamic s in x.Result)
            {
                JRCtler.JsonRpcRes a = new JRCtler.JsonRpcRes
                {
                    Result = s
                };
                Add(new TaskLite(a));
            }
            JRCtler.JsonRpcRes y = await Aria2Methords.TellWaiting();
            foreach (dynamic s in y.Result)
            {
                JRCtler.JsonRpcRes a = new JRCtler.JsonRpcRes
                {
                    Result = s
                };
                Add(new TaskLite(a));
            }
            JRCtler.JsonRpcRes z = await Aria2Methords.TellStopped();
            foreach (dynamic s in z.Result)
            {
                JRCtler.JsonRpcRes a = new JRCtler.JsonRpcRes
                {
                    Result = s
                };
                string Status = a.Result.status;
                if (Status!= "complete")
                {
                    Add(new TaskLite(a));
                }
                else
                {
                    TaskFinished?.Invoke(new TaskLite(a));
                }
            }

        }

        public async Task Update()//历遍单个任务，调用其刷新方法
        {
            foreach (TaskLite s in this)
            {
                if (s.Status== "complete")
                {
                    Remove(s);
                    break;
                }
                await s.Refresh();
            }
            OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }

    }

    public class FinishedTask//已完成任务
    {
        private async void GetInfo(string TaskGid)
        {
            string[] keys = new string[3];
            keys[0] = "dir";
            keys[1] = "totalLength";
            keys[2] = "path";
            var x = await Aria2Methords.TellStatus(TaskGid, keys);
            Path = x.Result.path;
            FileName = Path.Substring(Path.LastIndexOf(@"/"));
            FileSize = x.Result.totalLength;
            Icon = System.Drawing.Icon.ExtractAssociatedIcon(Path);
        }

        public System.Drawing.Icon Icon { get; set; }//下载文件的图标

        public string FileName { get; set; }//下载的文件名

        public string Gid { get; set; }//任务的GID

        public string Path { get; set; }//文件路径

        public string FileSize { get; set; }//文件尺寸

        public FinishedTask(TaskLite e)
        {
            Gid = e.Gid;
            GetInfo(Gid);
        }
        public FinishedTask(string Gid)
        {
            GetInfo(Gid);
        }

    }

    public class HistoryList : ObservableCollection<FinishedTask>//历史任务列表
    {
        private string Historys;

        private void Save(Object Sender, EventArgs e)
        {
            File.WriteAllText(@"HistoryList.log", Historys);
        }


        private void Load()
        {
            if (!File.Exists(@"HistoryList.log"))
            {
                File.Create(@"HistoryList.log");
            }
            Historys = File.ReadAllText(@"HistoryList.log");
        }

        public void TaskCompleted(TaskLite e)
        {
            FinishedTask a = new FinishedTask(e);
            if(!Contains(a))
            {
                Add(a);
            }
        }

        public HistoryList()//任务列表的构造函数，实际使用时应当修改
        {
            //Load();
            //CollectionChanged += Save;
        }
    }

    public class JRCtler//JsonRpc控制器，基于WebSocket
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
            //if (!File.Exists(@"log.txt"))
            //{
            //    File.CreateText(@"log.txt");
            //}
            //string Logs = File.ReadAllText(@"log.txt") +"send:"+ JsonConvert.SerializeObject(SendMessage) + Environment.NewLine;
            //File.WriteAllText(@"log.txt", Logs);
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
                File.CreateText(@"log.txt");
            }
            string Logs = File.ReadAllText(@"log.txt") + "Message:" + Message + Environment.NewLine;
            File.WriteAllText(@"log.txt", Logs);
        }

        //公开方法
        public async Task<JsonRpcRes> JsonRpcAsync(string methord, ArrayList Params)//异步开始一个远程调用
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

    public static class Aria2Methords//Aria2 Rpc接口的方法库
    {
        private static string Base64Encode(FileStream File)
        {
            byte[] bt = new byte[File.Length];
            File.Read(bt, 0, bt.Length);
            return Convert.ToBase64String(bt);
        }

        public static async Task<string> AddUri(string Uri)
        {
            string[] Uris = new string[] { Uri };
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.rpc_secret,
                Uris
            };
            string Gid = (await Stc.Line.JsonRpcAsync("aria2.addUri", Params)).Result;
            return Gid;
        }

        public static async Task<string> AddUri(string[] Uris)
        {
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.rpc_secret,
                Uris
            };
            string Gid = (await Stc.Line.JsonRpcAsync("aria2.addUri", Params)).Result;
            return Gid;
        }

        public static async Task<string> AddTorrent(FileStream File)//添加种子
        {
            string TorrentBase64 = Base64Encode(File);
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.rpc_secret,
                TorrentBase64
            };
            string Gid = (await Stc.Line.JsonRpcAsync("aria2.addTorrent", Params)).Result;
            return Gid;
        }

        public static async Task<string> AddMetalink(FileStream File)//添加MetaLink
        {
            string MetalinkBase64 = Base64Encode(File);
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.rpc_secret,
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
                "token:" + Stc.GloConf.rpc_secret,
                Gids
            };
            string Result = (await Stc.Line.JsonRpcAsync("aria2.remove", Params)).Result;
            return Result;
        }

        public static async Task<string> Pause(string Gid)
        {
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.rpc_secret,
                Gid
            };
            string Result = (await Stc.Line.JsonRpcAsync("aria2.pause", Params)).Result;
            return Result;
        }

        public static async Task PauseAll()
        {
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.rpc_secret,
            };
            string Result = (await Stc.Line.JsonRpcAsync("aria2.pauseAll", Params)).Result;
        }

        public static async Task<string> UpPause(string Gid)
        {
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.rpc_secret,
                Gid
            };
            string Result = (await Stc.Line.JsonRpcAsync("aria2.unpause", Params)).Result;
            return Result;
        }

        public static async Task UpPauseAll()
        {
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.rpc_secret,
            };
            string Result = (await Stc.Line.JsonRpcAsync("aria2.unpauseAll", Params)).Result;
        }

        public static async Task<JRCtler.JsonRpcRes> TellStatus(string Gid)
        {
            string[] Keys = new string[] { "status", "totalLength", "completedLength", "downloadSpeed", "gid" };
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.rpc_secret,
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
                "token:" + Stc.GloConf.rpc_secret,
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
                "token:" + Stc.GloConf.rpc_secret,
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
                "token:" + Stc.GloConf.rpc_secret,
                0,
                50,
                Keys
            };
            JRCtler.JsonRpcRes Result = await Stc.Line.JsonRpcAsync("aria2.tellWaiting", Params);
            return Result;
        }

        public static async Task<JRCtler.JsonRpcRes> TellStopped()
        {
            string[] Keys = new string[] { "status", "totalLength", "completedLength", "downloadSpeed", "gid" };
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.rpc_secret,
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
                "token:" + Stc.GloConf.rpc_secret,
                Gid
            };
            JRCtler.JsonRpcRes Result = await Stc.Line.JsonRpcAsync("aria2.getFiles", Params);
            return Result;
        }

        public static async Task<JRCtler.JsonRpcRes> GetGlobalStat()
        {
            ArrayList Params = new ArrayList
            {
                "token" + Stc.GloConf.rpc_secret,
            };
            JRCtler.JsonRpcRes Result = await Stc.Line.JsonRpcAsync("aria2.getGlobalStat", Params);
            return Result;

        }

        public static async void ShutDown()
        {
            ArrayList Params = new ArrayList
            {
                "token" + Stc.GloConf.rpc_secret,
            };
            JRCtler.JsonRpcRes Result = await Stc.Line.JsonRpcAsync("aria2.shutdown", Params);
        }

    }

}
