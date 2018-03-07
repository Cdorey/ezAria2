using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using WebSocketSharp;
using System.Data.JsonRpc;

namespace ezAria2
{
    //这里应该有，单个任务的类、任务列表的类、历史列表的类、单个RPC请求的类、单个RPC消息的类、RPC控制器



    class Task//任务对象，每一个对象的实例代表一个独立的任务
    {
        public string gid { get; set; }//任务的GID
        public string State//任务的状态
        {
            get
            {
                switch (status)
                {
                    case "active":
                        return "none";
                    case "waiting":
                        return "wait";
                    default:
                        return "error";
                }
            }
        }
        public string status { get; set; }//从json文件接收的任务状态
        public int totalLength { get; set; }//下载的总长度，单位字节
        public int completedLength { get; set; }//已经完成的下载长度
        public double Progress//进度，双精度浮点数
        {
            get
            {
                if (totalLength == 0)
                {
                    return 0;
                }
                else
                {
                    double i = completedLength / totalLength;
                    return i;
                }
            }
        }
        public string connections { get; set; }
        public string dir { get; set; }
        public string downloadSpeed { get; set; }
        public file[] files { get; set; }
        public class file
        {
            public string completedLength { get; set; }
            public string index { get; set; }
            public string length { get; set; }
            public string path { get; set; }
            public string selected { get; set; }
            public uuri[] uris { get; set; }
            public class uuri
            {
                public string status { get; set; }
                public string uri { get; set; }
            }
        }
        public string numPieces { get; set; }
        public string pieceLength { get; set; }
        public string uploadLength { get; set; }
        public string uploadSpeed { get; set; }

        //public Task(string icon,string filename, string speed)//单个任务的构造函数
        //{
        //    Icon = icon;
        //    FileName = filename;
        //    Speed = speed;
        //}
        public Task(string G)//创建一个任务对象，唯一的参数是任务的gid
        {
            //Icon = "Resources/editIcon.png";
            gid = G;
        }

        public Task()
        {
        }
    }
    class TaskList : ObservableCollection<Task>//任务列表
    {
        //public void TaskListAdd(String Uri)
        //{
        //    RpcControler NewTask = new RpcControler();
        //    Task NewOne=NewTask.addUri(Uri);
        //    Add(NewOne);
        //}
        public TaskList()
        {
        }
    }
    class HistoryList : ObservableCollection<Task>
    {
        public HistoryList()//任务列表的构造函数，实际使用时应当修改
        {
        }

    }

    class JsonRpcReq//一个独立的任务请求
    {
        public string jsonrpc { get; set; }
        public int id { get; set; }
        public string method { get; set; }
        public ArrayList @params { get; set; }
        public JsonRpcReq()
        {
            jsonrpc = "2.0";
            @params = new ArrayList();
        }
    }
    class JsonRpcMessage//一个独立的服务器消息
    {
        public string jsonrpc { get; set; }
        public string id { get; set; }
        public string result { get; set; }
        public JsonRpcMessage()
        {
            jsonrpc = "2.0";
            id = "null";
        }
    }
    class RpcControler //rpc请求控制器
    {
        //由其他对象实例化本控制器，直接调用本控制器的方法发送请求，同步等待结果
        //需要写一个方法，接受一个task对象，返回一个result对象
        private class JsonRpcRes //这个类提供一个ID和result的字典
        {
            private Dictionary<string, string> IdList = new Dictionary<string, string>();//这个字典的key是请求的id，value是返回的结果的json序列
            public Queue<string> NoticeList = new Queue<string>();//通知和消息存到这里;
            public string Result(string Id)//用于查询结果的方法，输入id，返回json字符串，如果没有返回waiting
            {
                if (IdList.ContainsKey(Id))
                {
                    string ret = IdList[Id];
                    IdList.Remove(Id);
                    return ret;
                }
                else
                {
                    return "waiting";
                }
            }
            public void AddResult(string Result)
            {
                JsonRpcMessage Res = JsonConvert.DeserializeObject<JsonRpcMessage>(Result);
                if (Res.id != "null")//这里的方法需要修改
                {
                    IdList.Add(Res.id, Result);
                }
                else
                {
                    NoticeList.Enqueue(Result);
                }
            }
        }

        static int RpcReqId;
        static WebSocket ws; //一个全局对象，控制WebSocket连接
        static JsonRpcRes ResultList;//所有从WebSocket返回的Result都先由它处理

        //aria2方法库
        public Task addUri(string uri)//指定uri,返回一个任务对象
        {
            var task = new JsonRpcReq//建立一个Rpc请求
            {
                method = "aria2.addUri"
            };
            string rpc_secret = ConfigController.GlobalConfigInformation.rpc_secret;//查找令牌
            rpc_secret = "token:" + rpc_secret;
            task.@params.Add(rpc_secret);
            string[] uris = new string[1];//格式化并添加URI
            uris[0] = uri;
            task.@params.Add(uris);
            task.id = RpcReqId++;//添加ID
            string json = JsonConvert.SerializeObject(task);
            ws.Send(json);
            string Result;
            do
            {
                Thread.Sleep(200);
                Result = ResultList.Result(task.id.ToString());
            } while (Result == "waiting");
            JsonRpcMessage Res = JsonConvert.DeserializeObject<JsonRpcMessage>(Result);
            Task ReturnObject = new Task(Res.result);//建立需要返回的对象，这个对象只有Gid属性是对的；
            return ReturnObject;
        }
        public string tellStatus(string gid)//指定gid,返回一个任务对象
        {
            var task = new JsonRpcReq//建立一个Rpc请求
            {
                method = "aria2.tellStatus"
            };
            string rpc_secret = ConfigController.GlobalConfigInformation.rpc_secret;//查找令牌
            rpc_secret = "token:" + rpc_secret;
            task.@params.Add(rpc_secret);
            string[] gids = new string[1];//格式化并添加URI
            gids[0] = gid;
            task.@params.Add(gids);
            task.id = RpcReqId++;//添加ID
            string json = JsonConvert.SerializeObject(task);
            ws.Send(json);
            string Result;
            do
            {
                Thread.Sleep(200);
                Result = ResultList.Result(task.id.ToString());
            } while (Result == "waiting");
            return Result;
        }
        public string tellStatus(Task info)//输入一个task对象,根据它的gid返回完整信息
        {
            var task = new JsonRpcReq//建立一个Rpc请求
            {
                method = "aria2.tellStatus"
            };
            string rpc_secret = ConfigController.GlobalConfigInformation.rpc_secret;//查找令牌
            rpc_secret = "token:" + rpc_secret;
            task.@params.Add(rpc_secret);
            string[] gids = new string[1];//格式化并添加URI
            gids[0] = info.gid;
            task.@params.Add(info.gid);
            task.id = RpcReqId++;//添加ID
            string json = JsonConvert.SerializeObject(task);
            ws.Send(json);
            string Result;
            do
            {
                Thread.Sleep(200);
                Result = ResultList.Result(task.id.ToString());
            } while (Result == "waiting");
            return Result;
        }

        //Ws连接控制
        static RpcControler()
        {
            string rpcurl = string.Format("ws://127.0.0.1:{0}/jsonrpc", ConfigController.GlobalConfigInformation.rpc_listen_port);
            ws = new WebSocket(rpcurl);
            ws.ConnectAsync();
            ResultList = new JsonRpcRes();
            RpcReqId = 1;
            ws.OnOpen += (sender, e) =>
            {
                //这里放需要发送的请求
                //ws.Send(send);

            };
            ws.OnMessage += (sender, e) =>
            {
                Console.WriteLine(e.Data);
                ResultList.AddResult(e.Data);
            };
        }

    }
    class RpcController
    {
        static RpcController()
        {
            //var client = new Spooky.Json20.JsonRpcHttpClient(new Uri(string.Format("http://127.0.0.1:{0}/jsonrpc", ConfigController.GlobalConfigInformation.rpc_listen_port)));
            //ArrayList @params = new ArrayList();
            //;//查找令牌
            //@params.Add("token:" + ConfigController.GlobalConfigInformation.rpc_secret);
            //string[] Uris = new String[1];
            //Uris[0] = @"http://mirror.lug.udel.edu/pub/centos/7/isos/x86_64/CentOS-7-x86_64-DVD-1708.iso";
            //@params.Add(Uris);
            //var answer = client.Invoke<int>("aria2.addUri",Uris);


        }
    }
}
