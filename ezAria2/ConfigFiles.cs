using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ezAria2
{
    class ConfigControler
    {
        private string[] ConfigInfo = new string[41];
        private string[] ConfigName = new string[41];
        private void LoadingConfigFile()//加载aria2.conf文件
        {
            XmlDocument conf = new XmlDocument();
            conf.Load(@"..\..\config.xml");
            XmlNode config = conf.SelectSingleNode("config");
            XmlNodeList ConfCatagorys = config.ChildNodes;//加载Section列表
            int n = 0;
            foreach (XmlNode ConfCatagory in ConfCatagorys)//循环历遍每个Section
            {
                XmlNodeList ConfItems = ConfCatagory.ChildNodes;//加载Section的配置项
                foreach (XmlNode ConfItem in ConfItems)//历遍每个配置项
                {
                    string a = ConfItem.Name;
                    string c = ConfItem.InnerText;
                    ConfigName[n] = a;//将配置项的名称储存到ConfigName中，所有的-被改成_
                    ConfigInfo[n] = c;//参数储存到ConfigInfo中
                    n = n + 1;
                }
            }

        }
        private void CreateToken()//随机生成一个16位的RPC令牌
        {
            Random creator = new Random();
            string charsToUse = "AzByCxDwEvFuGtHsIrJqKpLoMnNmOlPkQjRiShTgUfVeWdXcYbZa1234567890";
            int i = 1;
            string rpc_secret = ConfigInfo[25];
            while (i <= 16)
            {
                int a = creator.Next(0, 61);
                string b = charsToUse.Substring(a, 1);
                rpc_secret = rpc_secret + b;
                i = i + 1;
            }
            ConfigInfo[25] = rpc_secret;
        }

        public string Get(string a)//根据配置项的Key查找Value
        {
            int n = Array.IndexOf(ConfigName, a);
            if (n==-1)
            {
                return "配置项不存在";
            }
            else
            {
                return ConfigInfo[n];
            }
        }
        public void Set(string a)//根据配置项的Key设置Value
        {
            int n = Array.IndexOf(ConfigName, a);
            if (n != -1)
            {
                ConfigInfo[n] = a;
            }
        }
        public void Make()//根据当前内存中的Key和Value生成一份aria2.conf文件
        {
            SavingConfigFile();
            string ConfigFileBody = "";
            foreach (string b in ConfigName)
            {
                ConfigFileBody = ConfigFileBody + b.Replace("_", "-") + " = " + Get(b) + Environment.NewLine;
            }
            File.WriteAllText(@"..\..\aria2.conf", ConfigFileBody, Encoding.UTF8); //将ConfigFileBody的内容写入aria2.conf
        }
        public bool SavingConfigFile() //写配置文件，将全部配置文件保存到xml
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
                    ConfItem.InnerText = Get(ConfItem.Name);
                }
            }
            conf.Save(@"..\..\config.xml");

            return true;
        }
        public ConfigControler() //构造函数
        {
            LoadingConfigFile();
            if (ConfigInfo[25].Length!=16)
            {
                CreateToken();
            }
        }
        ~ConfigControler() //析构函数
        {
        }
    }
}
