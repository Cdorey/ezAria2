using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Thrift;
using Thrift.Protocol;
using Thrift.Transport;

namespace test
{
    class Program
    {
        static void Json(string[] args)
        {
            string[] product = { "a", "b", "c" };
            Console.WriteLine(product);
            //将对象转化成JSON字符串
            string output = JsonConvert.SerializeObject(product);
            Console.WriteLine("JSON内容");
            Console.WriteLine(output);
            //将JSON字符串转化成对象
            object product2 = JsonConvert.DeserializeObject(output);
            Console.WriteLine("JSON转对象内容");
            Console.WriteLine(product2);
            Console.ReadLine();
        }
        static void RandomString(string[] args)
        {
            string rpc_secret="";
            Random creator = new Random();
            string charsToUse = "AzByCxDwEvFuGtHsIrJqKpLoMnNmOlPkQjRiShTgUfVeWdXcYbZa1234567890";
            int i = 1;
            while (!(i>16))
            {
                int a = creator.Next(1, 62);
                string b = charsToUse.Substring(a, 1);
                rpc_secret = rpc_secret + b;
                i = i + 1;
            }
            Console.WriteLine(rpc_secret);
            Console.ReadLine();
        }
        static void XMLLoader()
        {
            XmlDocument conf = new XmlDocument();
            conf.Load(@"..\..\config.xml");
            XmlNode config = conf.SelectSingleNode("config");
            XmlNodeList ConfCatagorys = config.ChildNodes;//一共是四个
            foreach (XmlNode ConfCatagory in ConfCatagorys)
            {
                XmlNodeList ConfItems = ConfCatagory.ChildNodes;//获取单个配置文件节点的列表
                foreach (XmlNode ConfItem in ConfItems)//历遍每个配置项
                {
                    string a=ConfItem.Name;
                    string b=ConfItem.InnerText;
                    a=a.Replace("_", "-");
                    Console.WriteLine(a+"="+b);
                }
            }
            Console.ReadLine();
        }
        static void XMLSaver(string[] args)
        {
            XmlDocument conf = new XmlDocument();
            conf.Load(@"..\..\config.xml");
            XmlNode config = conf.SelectSingleNode("config");
            XmlNodeList ConfCatagorys = config.ChildNodes;//一共是5个组
            foreach (XmlNode ConfCatagory in ConfCatagorys)
            {
                XmlNodeList ConfItems = ConfCatagory.ChildNodes;//获取单个配置文件节点的列表
                foreach (XmlNode ConfItem in ConfItems)//历遍每个配置项
                {
                    ConfItem.InnerText = "helloworld";
                }
            }
            conf.Save(@"..\..\config.xml");
            XMLLoader();
        }
        static void Make(string[] args)
        {
            string[] ConfigName=new string[41];
            string[] ConfigInfo = new string[41];
            XmlDocument conf = new XmlDocument();
            conf.Load(@"..\..\config.xml");
            XmlNode config = conf.SelectSingleNode("config");
            XmlNodeList ConfCatagorys = config.ChildNodes;//一共是四个
            int n = 0;
            foreach (XmlNode ConfCatagory in ConfCatagorys)
            {
                XmlNodeList ConfItems = ConfCatagory.ChildNodes;//获取单个配置文件节点的列表
                foreach (XmlNode ConfItem in ConfItems)//历遍每个配置项
                {
                    string a = ConfItem.Name;
                    string c = ConfItem.InnerText;
                    ConfigName[n] = a;
                    ConfigInfo[n] = c;
                    n = n + 1;
                }
            }
            int m = 1;
            string ConfigFileBody = m+":";
            foreach (string b in ConfigName)
            {
                int s = Array.IndexOf(ConfigName, b);
                string a = b.Replace("_", "-");
                string c = ConfigInfo[s];
                m = m + 1;
                ConfigFileBody =  ConfigFileBody + a + " = " + c + Environment.NewLine+ m + ":";
            }
            File.WriteAllText(@"..\..\aria2.conf", ConfigFileBody, Encoding.UTF8); //将ConfigFileBody的内容写入aria2.conf
            //int m = 1;
            //foreach (string b in ConfigName)
            //{
            //    Console.WriteLine(m+b);
            //    m = m + 1;
            //}
            Console.ReadLine();

        }
        static void BangRenCeShi(string[] args)
        {
            XmlDocument info = new XmlDocument();
            info.Load(@"..\..\RailData.xml");//你的xml文件的位置
            XmlNode root = info.SelectSingleNode("RailSelection");
            XmlNodeList items = root.ChildNodes;//加载Section列表
            foreach (XmlNode item in items)//循环历遍每个Section
            {
                string a = item.Name+":"+Environment.NewLine;//a是每个项目的内容
                XmlNodeList datas = item.ChildNodes;//加载每一种材料的子尺寸
                foreach (XmlNode data in datas)//历遍每个尺寸
                {
                    XmlNodeList numbers = data.ChildNodes;
                    foreach (XmlNode number in numbers)
                    {
                        a = a + (string)number.Name + ":" + (string)number.InnerText + "、";
                    }
                    a = a + Environment.NewLine;
                }
                Console.WriteLine(a);
            }
            Console.ReadLine();

        }
        static void Main()
        {
            Dictionary<string, string> Group = new Dictionary<string, string>();//创建70个组
            for (int i=1;i<=70;i++)
            {
                Group.Add(i.ToString(), "");
            }
            int s = 0;
            while (PanDuan(Group)!=0)
            {
                string PlayerA = "";//创建抽签机
                string PlayerB = "";
                while (PlayerA == "" || PlayerB == "")
                {
                    while (PlayerA == "")
                    {
                        Random Chouqian = new Random();
                        int a = Chouqian.Next(1, 71);
                        if (Group[a.ToString()].Length < 3)
                        {
                            PlayerA = a.ToString();
                        }
                    }
                    while (PlayerB == "")
                    {
                        Random Chouqian = new Random();
                        int b = Chouqian.Next(1, 71);
                        if (Group[b.ToString()].Length < 3 && b.ToString() != PlayerA)
                        {
                            PlayerB = b.ToString();
                        }
                    }
                }

                Random Match = new Random();
                int c = Match.Next(0, 1);
                if (c == 0)
                {
                    Group[PlayerA] = Group[PlayerA] + "赢";
                    Group[PlayerB] = Group[PlayerB] + "输";
                    Console.WriteLine("比赛" + PlayerA + "胜利，尚有"+ PanDuan(Group)+"组没有完成比赛");
                }
                else
                {
                    Group[PlayerB] = Group[PlayerB] + "赢";
                    Group[PlayerA] = Group[PlayerA] + "输";
                    Console.WriteLine("比赛" + PlayerB + "胜利，尚有" + PanDuan(Group) + "组没有完成比赛");
                }
                s = s + 1;
            }
            Console.WriteLine("共进行了" + s + "场比赛");
            string result = "";
            foreach (string team in Group.Keys)
            {
                result = result + "第" + team + "组成绩为" + Group[team] + Environment.NewLine;
                if (Group[team].Length < 3)
                {
                    Console.WriteLine("第" + team + "没有完成比赛，他们的当前成绩为“" + Group[team] + "”");
                }
            }
            Console.WriteLine(result);
            Console.ReadLine();
        }
        static int PanDuan(Dictionary<string, string> f)
        {
            int i = 0;
            foreach (string a in f.Values)
            {
                if (a.Length < 3)
                {
                    i = i + 1;
                }
            }
            return i;
        }
        //class HelloWorldServiceClient
        //{
        //    public const string SERVERIP = "localhost";//基本参数设置
        //    public static int SERVERPORT = 8090;
        //    public static int TIMEOUT = 3000;

        //    public void startClient(String username)//客户端的方法
        //    {
        //        TTransport transport = null;
        //        try
        //        {
        //            transport = new TSocket(SERVERIP, SERVERPORT, TIMEOUT);
        //            //协议要和服务端一致
        //            TProtocol protocol = new TCompactProtocol(transport);
        //            HelloWorldService.Client client = new HelloWorldService.Client(protocol);
        //            transport.Open();
        //            String result = client.sayHello(username);
        //            Console.WriteLine("Thrift client result =: " + result);

        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.StackTrace);
        //        }
        //        finally
        //        {
        //            if (null != transport)
        //            {
        //                //close
        //                transport.Close();
        //            }
        //        }
        //    }
        //    class HelloWroldImpl : HelloWorldService.Iface
        //    {

        //        public string sayHello(string username)
        //        {
        //            Console.WriteLine("hello" + username);
        //            return "hello" + username;
        //        }

        //    }


        //}
    }


}
