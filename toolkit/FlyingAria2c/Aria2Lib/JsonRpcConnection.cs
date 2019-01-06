using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace FlyingAria2c.Aria2Lib
{
    internal class JsonRpcConnection
    {
        private class Line
        {
            public Line(Aria2cProgressHandle aria2CProgressHandle)
            {
                if (aria2CProgressHandle == null)
                {
                    throw new System.ArgumentNullException(nameof(aria2CProgressHandle));
                }

                WebSocket = new WebSocket(string.Format("ws://127.0.0.1:{0}/jsonrpc", aria2CProgressHandle.Port));
                WebSocket.Connect();
                Idlist = 1;
                RespondList = new Dictionary<int, string>();
                WebSocket.OnMessage += (sender, e) =>
                {
                    OnMessage(e.Data);
                };
            }

            public int Idlist;

            public Dictionary<int, string> RespondList;

            private WebSocket WebSocket;

            /// <summary>
            /// 发送消息
            /// </summary>
            /// <param name="SendMessage"></param>
            public void Send(Request SendMessage)
            {
                WebSocket.Send(JsonConvert.SerializeObject(SendMessage));
            }

            /// <summary>
            /// 消息到达的事件处理程序
            /// </summary>
            /// <param name="Message"></param>
            private void OnMessage(string Message)
            {
                ResponseBase ResponseMessage = JsonConvert.DeserializeObject<ResponseBase>(Message);
                if (ResponseMessage.Id == -1)
                {
                    //NoticeList.Enqueue(NewMessage);
                }
                else
                {
                    RespondList.Add(ResponseMessage.Id, Message);
                }
            }
        }

        private class Request
        {
            [JsonProperty(PropertyName = "jsonrpc")]
            public string Version => "2.0";

            [JsonProperty(PropertyName = "method")]
            public string Method { get; set; }

            [JsonProperty(PropertyName = "params")]
            public object[] Params { get; set; }

            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }

            public Request(string n, int i, object[] e)
            {
                Method = n;
                Id = i;
                Params = e;
            }
        }

        public class ResponseBase
        {
            [JsonProperty(PropertyName = "jsonrpc")]
            public string Version { get; set; }

            [JsonProperty(PropertyName = "method")]
            public string Method { get; set; }

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

            public ResponseBase()
            {
                Id = -1;
            }
        }

        public class Response<T> : ResponseBase
        {
            [JsonProperty(PropertyName = "result")]
            public T Result { get; set; }
        }

        private Line WebSocketLine;

        public string Token;

        /// <summary>
        /// 异步开始一个远程调用
        /// </summary>
        /// <param name="methord">方法</param>
        /// <param name="Params">参数数组</param>
        /// <returns></returns>
        public async Task<T> JsonRpcAsync<T>(string methord, object[] Params)
        {
            Request NewTask = new Request(methord, WebSocketLine.Idlist++, Params);
            WebSocketLine.Send(NewTask);
            await Task.Run(() =>
            {
                while (!WebSocketLine.RespondList.ContainsKey(NewTask.Id))
                {
                    Thread.Sleep(500);
                }
            });
            T a = JsonConvert.DeserializeObject<T>(WebSocketLine.RespondList[NewTask.Id]);
            WebSocketLine.RespondList.Remove(NewTask.Id);
            return a;
        }

        /// <summary>
        /// 同步开始一个远程调用
        /// </summary>
        /// <param name="methord">方法</param>
        /// <param name="Params">参数数组</param>
        /// <returns></returns>
        public T JsonRpc<T>(string methord, object[] Params)
        {
            Request NewTask = new Request(methord, WebSocketLine.Idlist++, Params);
            WebSocketLine.Send(NewTask);
            while (!WebSocketLine.RespondList.ContainsKey(NewTask.Id))
            {
                Thread.Sleep(500);
            }
            T a = JsonConvert.DeserializeObject<T>(WebSocketLine.RespondList[NewTask.Id]);
            WebSocketLine.RespondList.Remove(NewTask.Id);
            return a;
        }

        /// <summary>
        /// 开启一个调用不等待返回结果
        /// </summary>
        /// <param name="methord"></param>
        /// <param name="Params"></param>
        /// <returns></returns>
        public void JsonRpcWithoutRes(string methord, object[] Params)
        {
            Request NewTask = new Request(methord, WebSocketLine.Idlist++, Params);
            WebSocketLine.Send(NewTask);
        }

        public JsonRpcConnection(Aria2cProgressHandle aria2CProgressHandle)
        {
            WebSocketLine = new Line(aria2CProgressHandle);
            Token = aria2CProgressHandle.Token;
        }
    }
}