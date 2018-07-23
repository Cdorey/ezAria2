using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace ezAria2
{
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
            //string[] Gids = new string[] { Gid };
            ArrayList Params = new ArrayList
            {
                "token:" + Stc.GloConf.Rpc_secret,
                Gid
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
            Stc.Line.JsonRpcWithoutRes("aria2.shutdown", Params);
            Stc.Line.Quit();
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
            JsonRpcRes a = RespondList[NewTask.Id];
            RespondList.Remove(NewTask.Id);
            return a;
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
            JsonRpcRes a = RespondList[NewTask.Id];
            RespondList.Remove(NewTask.Id);
            return a;
        }

        /// <summary>
        /// 开启一个调用不等待返回结果
        /// </summary>
        /// <param name="methord"></param>
        /// <param name="Params"></param>
        /// <returns></returns>
        public void JsonRpcWithoutRes(string methord, ArrayList Params)
        {
            JsonRpcReq NewTask = new JsonRpcReq(methord, Idlist++, Params);
            Send(NewTask);//这一行应该调用控制器的方法执行NewTask请求
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

}