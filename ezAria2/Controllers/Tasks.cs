using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace ezAria2
{
    //这里应该有，单个任务的类、任务列表的类、历史列表的类、单个RPC请求的类、单个RPC消息的类、RPC控制器
    public class TaskLite//小型任务对象,用于任务列表
    {
        public string State { get; set; }//任务的状态
        public string Icon { get; set; }//下载文件的图标
        public string Speed { get; set; }//当前速度
        public double Progress { get; set; } //进度，双精度浮点数
        public string FileName { get; set; }//下载的文件名

        public string Gid { get; set; }//任务的GID
        public TaskLite(JRCtler.JsonRpcRes e)
        {
            switch (e.Result.status)
            {
                case "active":
                    State= "none";
                    Icon = "Resources/bonfire-1849089_640.png";
                    Speed = e.Result.downloadSpeed;
                    break;
                case "waiting":
                    State= "wait";
                    Icon = "Resources/stopwatch-1849088_640.png";
                    break;
                default:
                    State= "error";
                    Icon = "Resources/cup-1849083_640.png";
                    break;
            }
            if (e.Result.totalLength == 0)
            {
                Progress= 0;
            }
            else
            {
                double i = e.Result.completedLength / e.Result.totalLength;
                Progress= i;
            }
            Gid = e.Result.gid;
        }

        public TaskLite()
        {
        }
    }
    public class TaskList : ObservableCollection<TaskLite>//任务列表
    {
        public TaskList()
        {
        }
    }
    public class HistoryList:ObservableCollection<Task>
    {
        public HistoryList()//任务列表的构造函数，实际使用时应当修改
        {
        }

    }
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
        }

        //公开方法
        public async Task<JsonRpcRes> JsonRpcAsync(string methord, ArrayList Params)//异步开始一个远程调用
        {
            JsonRpcReq NewTask = new JsonRpcReq(methord, Idlist++, Params);
            Console.WriteLine(JsonConvert.SerializeObject(NewTask));//这一行应该调用控制器的方法执行NewTask请求
            await System.Threading.Tasks.Task.Run(() =>
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
            ws.ConnectAsync();
            ws.OnOpen += (sender, e) =>
            {

            };
            ws.OnMessage += (sender, e) =>
            {
                JsonRpcMessage(e.Data);
            };
        }
    }
    public static class Aria2Methords
    {
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
            string[] Gids = new string[1];
            Gids[0] = Gid;
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.rpc_secret,
                Gids
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
        public static async Task<JRCtler.JsonRpcRes> GetGlobalStat()
        {
            ArrayList Params = new ArrayList
            {
                "token" + Stc.GloConf.rpc_secret,
            };
            JRCtler.JsonRpcRes Result = await Stc.Line.JsonRpcAsync("aria2.getGlobalStat", Params);
            return Result;

        }
        public static async Task ShutDown()
        {
            ArrayList Params = new ArrayList
            {
                "token" + Stc.GloConf.rpc_secret,
            };
            JRCtler.JsonRpcRes Result = await Stc.Line.JsonRpcAsync("aria2.shutdown", Params);
        }

    }
}
