using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using ezAria2;

namespace test
{
    class Program
    {
        static void Main()
        {
            int a = 0;
            for(int i=0; i<10; i++)
            {
                Console.WriteLine("成绩多少？");
                int s = int.Parse(Console.ReadLine());
                if (s>80)
                {
                    a++;
                }
            }
            if (a > 8)
            {
                Console.WriteLine("超过80%");
            }
            else if (a > 6)
            {
                Console.WriteLine("超过60%");
            }
            else
            {
                Console.WriteLine("再接再厉");
            }

            Console.ReadLine();
        }
    }


}
    //class ConfigControler
    //{
    //    private string[] ConfigInfo = new string[41];
    //    private string[] ConfigName = new string[41];
    //    private void LoadingConfigFile()//加载aria2.conf文件
    //    {
    //        XmlDocument conf = new XmlDocument();
    //        conf.Load(@"..\..\config.xml");
    //        XmlNode config = conf.SelectSingleNode("config");
    //        XmlNodeList ConfCatagorys = config.ChildNodes;//加载Section列表
    //        int n = 0;
    //        foreach (XmlNode ConfCatagory in ConfCatagorys)//循环历遍每个Section
    //        {
    //            XmlNodeList ConfItems = ConfCatagory.ChildNodes;//加载Section的配置项
    //            foreach (XmlNode ConfItem in ConfItems)//历遍每个配置项
    //            {
    //                string a = ConfItem.Name;
    //                string c = ConfItem.InnerText;
    //                ConfigName[n] = a;//将配置项的名称储存到ConfigName中，所有的-被改成_
    //                ConfigInfo[n] = c;//参数储存到ConfigInfo中
    //                n = n + 1;
    //            }
    //        }

    //    }
    //    private void CreateToken()//随机生成一个16位的RPC令牌
    //    {
    //        Random creator = new Random();
    //        string charsToUse = "AzByCxDwEvFuGtHsIrJqKpLoMnNmOlPkQjRiShTgUfVeWdXcYbZa1234567890";
    //        int i = 1;
    //        string rpc_secret = ConfigInfo[25];
    //        while (i <= 16)
    //        {
    //            int a = creator.Next(0, 61);
    //            string b = charsToUse.Substring(a, 1);
    //            rpc_secret = rpc_secret + b;
    //            i = i + 1;
    //        }
    //        ConfigInfo[25] = rpc_secret;
    //    }

    //    public string Get(string a)//根据配置项的Key查找Value
    //    {
    //        int n = Array.IndexOf(ConfigName, a);
    //        if (n == -1)
    //        {
    //            return "配置项不存在";
    //        }
    //        else
    //        {
    //            return ConfigInfo[n];
    //        }
    //    }
    //    public void Set(string a)//根据配置项的Key设置Value
    //    {
    //        int n = Array.IndexOf(ConfigName, a);
    //        if (n != -1)
    //        {
    //            ConfigInfo[n] = a;
    //        }
    //    }
    //    public void Make()//根据当前内存中的Key和Value生成一份aria2.conf文件
    //    {
    //        SavingConfigFile();
    //        string ConfigFileBody = "";
    //        foreach (string b in ConfigName)
    //        {
    //            ConfigFileBody = ConfigFileBody + b.Replace("_", "-") + " = " + Get(b) + Environment.NewLine;
    //        }
    //        File.WriteAllText(@"..\..\aria2.conf", ConfigFileBody, Encoding.UTF8); //将ConfigFileBody的内容写入aria2.conf
    //    }
    //    public bool SavingConfigFile() //写配置文件，将全部配置文件保存到xml
    //    {
    //        XmlDocument conf = new XmlDocument();
    //        conf.Load(@"..\..\config.xml");
    //        XmlNode config = conf.SelectSingleNode("config");
    //        XmlNodeList ConfCatagorys = config.ChildNodes;//一共是5个组
    //        foreach (XmlNode ConfCatagory in ConfCatagorys)
    //        {
    //            XmlNodeList ConfItems = ConfCatagory.ChildNodes;//获取单个配置文件节点的列表
    //            foreach (XmlNode ConfItem in ConfItems)//历遍每个配置项
    //            {
    //                ConfItem.InnerText = Get(ConfItem.Name);
    //            }
    //        }
    //        conf.Save(@"..\..\config.xml");

    //        return true;
    //    }
    //    public ConfigControler() //构造函数
    //    {
    //        LoadingConfigFile();
    //        if (ConfigInfo[25].Length != 16)
    //        {
    //            CreateToken();
    //        }
    //    }
    //    ~ConfigControler() //析构函数
    //    {
    //    }
    //}
    //class RpcControler
    //{
    //    private class JsonRpcReq //定义JsonRpc请求的类
    //    {
    //        public string jsonrpc { get; set; }
    //        public string id { get; set; }
    //        public string method { get; set; }
    //        public ArrayList @params { get; set; }
    //        public JsonRpcReq()
    //        {
    //            jsonrpc = "2.0";
    //            @params = new ArrayList();
    //        }
    //    }
    //    private ConfigControler config = new ConfigControler();
    //    static WebSocket ws;

    //    //aria2方法库
    //    public void addUri(string id, string uri)//指定id和uir
    //    {
    //        var task = new JsonRpcReq
    //        {
    //            id = id,
    //            method = "aria2.addUri"
    //        };

    //        string rpc_secret = config.Get("rpc_secret");
    //        rpc_secret = "token:" + "123456";
    //        task.@params.Add(rpc_secret);
    //        string[] uris = new string[1];
    //        uris[0] = uri;
    //        task.@params.Add(uris);
    //        string json = JsonConvert.SerializeObject(task);
    //        ws.Send(json);
    //    }

    //    //Ws连接控制
    //    static RpcControler()
    //    {
    //        ConfigControler port = new ConfigControler();
    //        string rpcurl = string.Format("ws://127.0.0.1:{0}/jsonrpc", port.Get("rpc_listen_port"));
    //        ws = new WebSocket(rpcurl);
    //        ws.ConnectAsync();
    //        ws.OnOpen += (sender, e) =>
    //        {
    //            //这里放需要发送的请求
    //            //ws.Send(send);

    //        };
    //        ws.OnMessage += (sender, e) =>
    //        {
    //            //e.Data是一串需要反序列化的字符串
    //            Console.WriteLine(e.Data);
    //        };
    //    }
    //}
    //private class Jsonrpcreq
    //{
    //    public string jsonrpc { get; set; }
    //    public string id { get; set; }
    //    public string method { get; set; }
    //    public ArrayList @params { get; set; }
    //    public Jsonrpcreq()
    //    {
    //        jsonrpc = "";
    //        id = "";
    //        method = "";
    //        @params = new ArrayList();
    //    }
    //}
    //static void Json()
    //{
    //    string[] product = { "a", "b", "c" };
    //    Console.WriteLine(product);
    //    //将对象转化成JSON字符串
    //    string output = JsonConvert.SerializeObject(product);
    //    Console.WriteLine("JSON内容");
    //    Console.WriteLine(output);
    //    //将JSON字符串转化成对象
    //    object product2 = JsonConvert.DeserializeObject(output);
    //    Console.WriteLine("JSON转对象内容");
    //    Console.WriteLine(product2);

    //    //测试
    //    string a0, a1, a2, a3;
    //    a0 = "2.0";
    //    a1 = "123";
    //    a2 = "aria2.addUri";
    //    a3 = "[\"token:123456\", [\"a\" ,\"b\"]]";
    //    string send = String.Format("{{\"jsonrpc\":\"{0}\", \"id\":\"{1}\", \"method\":\"{2}\", \"params\":{3} }}", a0, a1, a2, a3);
    //    Jsonrpcreq a = JsonConvert.DeserializeObject<Jsonrpcreq>(send);
    //    Console.WriteLine(a.@params);
    //    object b = (object)a.@params[1];
    //    Console.WriteLine(send);

    //    //序列化
    //    Jsonrpcreq start = new Jsonrpcreq
    //    {
    //        jsonrpc = "2.0",
    //        id = "456",
    //        method = "aria2.addUri"
    //    };
    //    start.@params.Add("token:123456");
    //    string[] requris = new string[2];
    //    requris[0] = "a";
    //    requris[1] = "b";
    //    start.@params.Add(requris);
    //    string reqjsons = JsonConvert.SerializeObject(start);
    //    Console.WriteLine(reqjsons);

    //    Jsonrpcreq rere = JsonConvert.DeserializeObject<Jsonrpcreq>(reqjsons);
    //    Console.WriteLine(rere.Equals(a));
    //    Console.ReadLine();
    //}
    //static void Jsontest2()
    //{
    //    string a0, a1, a2;
    //    a0 = "2.0";
    //    a1 = "123";
    //    a2 = "aria2.addUri";
    //    string send = String.Format("{{\"jsonrpc\":\"{0}\", \"id\":\"{1}\", \"method\":\"{2}\" }}", a0, a1, a2);
    //    Jsonrpcreq a = JsonConvert.DeserializeObject<Jsonrpcreq>(send);
    //    Console.WriteLine(a.jsonrpc);
    //    Console.ReadLine();
    //}
    //static void RandomString(string[] args)
    //{
    //    string rpc_secret="";
    //    Random creator = new Random();
    //    string charsToUse = "AzByCxDwEvFuGtHsIrJqKpLoMnNmOlPkQjRiShTgUfVeWdXcYbZa1234567890";
    //    int i = 1;
    //    while (!(i>16))
    //    {
    //        int a = creator.Next(1, 62);
    //        string b = charsToUse.Substring(a, 1);
    //        rpc_secret = rpc_secret + b;
    //        i = i + 1;
    //    }
    //    Console.WriteLine(rpc_secret);
    //    Console.ReadLine();
    //}
    //static void XMLLoader()
    //{
    //    XmlDocument conf = new XmlDocument();
    //    conf.Load(@"..\..\config.xml");
    //    XmlNode config = conf.SelectSingleNode("config");
    //    XmlNodeList ConfCatagorys = config.ChildNodes;//一共是四个
    //    foreach (XmlNode ConfCatagory in ConfCatagorys)
    //    {
    //        XmlNodeList ConfItems = ConfCatagory.ChildNodes;//获取单个配置文件节点的列表
    //        foreach (XmlNode ConfItem in ConfItems)//历遍每个配置项
    //        {
    //            string a=ConfItem.Name;
    //            string b=ConfItem.InnerText;
    //            a=a.Replace("_", "-");
    //            Console.WriteLine(a+"="+b);
    //        }
    //    }
    //    Console.ReadLine();
    //}
    //static void XMLSaver(string[] args)
    //{
    //    XmlDocument conf = new XmlDocument();
    //    conf.Load(@"..\..\config.xml");
    //    XmlNode config = conf.SelectSingleNode("config");
    //    XmlNodeList ConfCatagorys = config.ChildNodes;//一共是5个组
    //    foreach (XmlNode ConfCatagory in ConfCatagorys)
    //    {
    //        XmlNodeList ConfItems = ConfCatagory.ChildNodes;//获取单个配置文件节点的列表
    //        foreach (XmlNode ConfItem in ConfItems)//历遍每个配置项
    //        {
    //            ConfItem.InnerText = "helloworld";
    //        }
    //    }
    //    conf.Save(@"..\..\config.xml");
    //    XMLLoader();
    //}
    //static void Make()
    //{
    //    string[] ConfigName=new string[41];
    //    string[] ConfigInfo = new string[41];
    //    XmlDocument conf = new XmlDocument();
    //    conf.Load(@"..\..\config.xml");
    //    XmlNode config = conf.SelectSingleNode("config");
    //    XmlNodeList ConfCatagorys = config.ChildNodes;//一共是四个
    //    int n = 0;
    //    foreach (XmlNode ConfCatagory in ConfCatagorys)
    //    {
    //        XmlNodeList ConfItems = ConfCatagory.ChildNodes;//获取单个配置文件节点的列表
    //        foreach (XmlNode ConfItem in ConfItems)//历遍每个配置项
    //        {
    //            string a = ConfItem.Name;
    //            string c = ConfItem.InnerText;
    //            ConfigName[n] = a;
    //            ConfigInfo[n] = c;
    //            n = n + 1;
    //        }
    //    }
    //    //int m = 1;
    //    //string ConfigFileBody = m+":";
    //    //foreach (string b in ConfigName)
    //    //{
    //    //    int s = Array.IndexOf(ConfigName, b);
    //    //    string a = b.Replace("_", "-");
    //    //    string c = ConfigInfo[s];
    //    //    m = m + 1;
    //    //    ConfigFileBody =  ConfigFileBody + a + " = " + c + Environment.NewLine+ m + ":";
    //    //}
    //    //File.WriteAllText(@"..\..\aria2.conf", ConfigFileBody, Encoding.UTF8); //将ConfigFileBody的内容写入aria2.conf
    //    string JsonInfo = "{{";
    //    foreach (string b in ConfigName)
    //    {
    //        String key = b;
    //        String value = ConfigInfo[Array.IndexOf(ConfigName, b)];
    //        String add = string.Format("\"{0}\":\"{1}\",",key,value);
    //        JsonInfo = JsonInfo + add;
    //    }
    //    File.WriteAllText(@"..\..\json.conf", JsonInfo, Encoding.UTF8);
    //    //int m = 1;
    //    //foreach (string b in ConfigName)
    //    //{
    //    //    Console.WriteLine(m+b);
    //    //    m = m + 1;
    //    //}
    //    Console.ReadLine();

    //}
    //static void BangRenCeShi(string[] args)
    //{
    //    XmlDocument info = new XmlDocument();
    //    info.Load(@"..\..\RailData.xml");//你的xml文件的位置
    //    XmlNode root = info.SelectSingleNode("RailSelection");
    //    XmlNodeList items = root.ChildNodes;//加载Section列表
    //    foreach (XmlNode item in items)//循环历遍每个Section
    //    {
    //        string a = item.Name+":"+Environment.NewLine;//a是每个项目的内容
    //        XmlNodeList datas = item.ChildNodes;//加载每一种材料的子尺寸
    //        foreach (XmlNode data in datas)//历遍每个尺寸
    //        {
    //            XmlNodeList numbers = data.ChildNodes;
    //            foreach (XmlNode number in numbers)
    //            {
    //                a = a + (string)number.Name + ":" + (string)number.InnerText + "、";
    //            }
    //            a = a + Environment.NewLine;
    //        }
    //        Console.WriteLine(a);
    //    }
    //    Console.ReadLine();

    //}
    //static void ChouqianBisai()
    //{
    //    Dictionary<string, string> Group = new Dictionary<string, string>();//创建70个组
    //    for (int i=1;i<=70;i++)
    //    {
    //        Group.Add(i.ToString(), "");
    //    }
    //    int s = 0;
    //    while (PanDuan(Group)!=0)
    //    {
    //        string PlayerA = "";//创建抽签机
    //        string PlayerB = "";
    //        while (PlayerA == "" || PlayerB == "")
    //        {
    //            while (PlayerA == "")
    //            {
    //                Random Chouqian = new Random();
    //                int a = Chouqian.Next(1, 71);
    //                if (Group[a.ToString()].Length < 3)
    //                {
    //                    PlayerA = a.ToString();
    //                }
    //            }
    //            while (PlayerB == "")
    //            {
    //                Random Chouqian = new Random();
    //                int b = Chouqian.Next(1, 71);
    //                if (Group[b.ToString()].Length < 3 && b.ToString() != PlayerA)
    //                {
    //                    PlayerB = b.ToString();
    //                }
    //            }
    //        }

    //        Random Match = new Random();
    //        int c = Match.Next(0, 1);
    //        if (c == 0)
    //        {
    //            Group[PlayerA] = Group[PlayerA] + "赢";
    //            Group[PlayerB] = Group[PlayerB] + "输";
    //            Console.WriteLine("比赛" + PlayerA + "胜利，尚有"+ PanDuan(Group)+"组没有完成比赛");
    //        }
    //        else
    //        {
    //            Group[PlayerB] = Group[PlayerB] + "赢";
    //            Group[PlayerA] = Group[PlayerA] + "输";
    //            Console.WriteLine("比赛" + PlayerB + "胜利，尚有" + PanDuan(Group) + "组没有完成比赛");
    //        }
    //        s = s + 1;
    //    }
    //    Console.WriteLine("共进行了" + s + "场比赛");
    //    string result = "";
    //    foreach (string team in Group.Keys)
    //    {
    //        result = result + "第" + team + "组成绩为" + Group[team] + Environment.NewLine;
    //        if (Group[team].Length < 3)
    //        {
    //            Console.WriteLine("第" + team + "没有完成比赛，他们的当前成绩为“" + Group[team] + "”");
    //        }
    //    }
    //    Console.WriteLine(result);
    //    Console.ReadLine();
    //}
    //static int PanDuan(Dictionary<string, string> f)
    //{
    //    int i = 0;
    //    foreach (string a in f.Values)
    //    {
    //        if (a.Length < 3)
    //        {
    //            i = i + 1;
    //        }
    //    }
    //    return i;
    //}
    //static void Httpjsonrpc()
    //{
    //    string jsonget = "http://127.0.0.1:6800/jsonrpc?method=aria2.addUri&id=ID&params=";
    //    string param= "[\"token:123456\", [\"https://download.microsoft.com/download/A/B/E/ABEE70FE-7DE8-472A-8893-5F69947DE0B1/MediaCreationTool.exe\"]]";
    //    byte[] bytes = Encoding.Default.GetBytes(param);
    //    string bases=Convert.ToBase64String(bytes);
    //    HttpWebRequest aria2 = (HttpWebRequest)WebRequest.Create(jsonget+bases);
    //    aria2.Method = "GET";
    //    using (WebResponse json = aria2.GetResponse())
    //    {
    //        StreamReader sr = new StreamReader(json.GetResponseStream(), Encoding.Default);
    //        string sReturn = sr.ReadToEnd().Trim();
    //        Console.WriteLine(sReturn);
    //    }
    //    Console.ReadLine();
    //}
    //static void Wsjsonrpc()
    //{
    //    string requrl = "ws://127.0.0.1:6800/jsonrpc";
    //    WebSocket ws = new WebSocket(requrl);
    //    string a0, a1, a2, a3;
    //    a0 = "2.0";
    //    a1 = "123";
    //    a2 = "aria2.addUri";
    //    a3 = "[\"token:123456\", [\"https://download.microsoft.com/download/A/B/E/ABEE70FE-7DE8-472A-8893-5F69947DE0B1/MediaCreationTool.exe\" , \"http://down.sandai.net/thunder9/Thunder9.1.47.1020.exe\"]]";
    //    string send = String.Format("{{\"jsonrpc\":\"{0}\", \"id\":\"{1}\", \"method\":\"{2}\", \"params\":{3} }}", a0,a1,a2,a3);


    //    Jsonrpcreq start = new Jsonrpcreq
    //    {
    //        jsonrpc = "2.0",
    //        id = "456",
    //        method = "aria2.addUri"
    //    };
    //    start.@params.Add("token:123456");
    //    string[] requris = new string[1];
    //    //requris[1] = "https://download.microsoft.com/download/A/B/E/ABEE70FE-7DE8-472A-8893-5F69947DE0B1/MediaCreationTool.exe";
    //    requris[0] = "http://down.sandai.net/thunder9/Thunder9.1.47.1020.exe";
    //    start.@params.Add(requris);
    //    string test = JsonConvert.SerializeObject(start);
    //    ws.ConnectAsync();
    //    ws.OnOpen += (sender, e) =>
    //    {
    //        //ws.Send(send);
    //        ws.Send(test);

    //    };
    //    ws.OnMessage += (sender, e) => 
    //    {
    //        Console.WriteLine(e.Data);
    //    };
    //    Console.ReadLine();
    //}
    //static void Rpctest()
    //{
    //    string uri = "https://download.microsoft.com/download/A/B/E/ABEE70FE-7DE8-472A-8893-5F69947DE0B1/MediaCreationTool.exe\" , \"http://down.sandai.net/thunder9/Thunder9.1.47.1020.exe";
    //    RpcControler start = new RpcControler();
    //    start.addUri("1",uri);
    //    Console.ReadLine();
    //}
    //private class Jsontest
    //{
    //    public bool Bool;
    //    public string String;
    //}
    //static void Jsontest3()
    //{
    //    Jsontest a = new Jsontest();
    //    a.Bool = true;
    //    a.String = "true";
    //    string output = JsonConvert.SerializeObject(a);
    //    Console.WriteLine(output);
    //    Jsontest b = JsonConvert.DeserializeObject<Jsontest>(output);
    //    Console.WriteLine(b.Bool);
    //    Console.WriteLine(b.String);
    //    Console.ReadLine();
    //}