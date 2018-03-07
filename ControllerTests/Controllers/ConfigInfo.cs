using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace ezAria2
{
    class ConfigInformation //配置文件对象
    {
        public string dir { get; set; }
        public string disk_cache { get; set; }
        public string file_allocation { get; set; }
        public string @continue { get; set; }
        public string max_concurrent_downloads { get; set; }
        public string max_connection_per_server { get; set; }
        public string min_split_size { get; set; }
        public string split { get; set; }
        public string max_overall_download_limit { get; set; }
        public string max_download_limit { get; set; }
        public string max_overall_upload_limit { get; set; }
        public string max_upload_limit { get; set; }
        public string disable_ipv6 { get; set; }
        public string timeout { get; set; }
        public string max_tries { get; set; }
        public string retry_wait { get; set; }
        public string input_file { get; set; }
        public string save_session { get; set; }
        public string save_session_interval { get; set; }
        public string enable_rpc { get; set; }
        public string rpc_allow_origin_all { get; set; }
        public string rpc_listen_all { get; set; }
        public string event_poll { get; set; }
        public string rpc_listen_port { get; set; }
        public string rpc_secret { get; set; }
        public string follow_torrent { get; set; }
        public string listen_port { get; set; }
        public string bt_max_peers { get; set; }
        public string enable_dht { get; set; }
        public string enable_dht6 { get; set; }
        public string dht_listen_port { get; set; }
        public string bt_enable_lpd { get; set; }
        public string enable_peer_exchange { get; set; }
        public string bt_request_peer_speed_limit { get; set; }
        public string peer_id_prefix { get; set; }
        public string user_agent { get; set; }
        public string seed_ratio { get; set; }
        public string force_save { get; set; }
        public string bt_hash_check_seed { get; set; }
        public string bt_seed_unverified { get; set; }
        public string bt_save_metadata { get; set; }

    }
    class ConfigController //配置文件控制器
    {
        public static ConfigInformation GlobalConfigInformation = new ConfigInformation();
        public static void LoadConfigFile()
        {
            using (StreamReader sr = new StreamReader(@"json.conf"))
            {
                // Read the stream to a string, and write the string to the console.
                String Json_config = sr.ReadToEnd();
                GlobalConfigInformation = JsonConvert.DeserializeObject<ConfigInformation>(Json_config);
            }
            if (GlobalConfigInformation.rpc_secret.Length != 16)
            {
                Random creator = new Random();
                string charsToUse = "AzByCxDwEvFuGtHsIrJqKpLoMnNmOlPkQjRiShTgUfVeWdXcYbZa1234567890";
                int i = 1;
                GlobalConfigInformation.rpc_secret = "";
                while (i <= 16)
                {
                    int a = creator.Next(0, 61);
                    string b = charsToUse.Substring(a, 1);
                    GlobalConfigInformation.rpc_secret = GlobalConfigInformation.rpc_secret + b;
                    i = i + 1;
                }
            }
        }


        public void LoadConfigFile(ref ConfigInformation a)
        {
            using (StreamReader sr = new StreamReader(@"json.conf"))
            {
                // Read the stream to a string, and write the string to the console.
                String Json_config = sr.ReadToEnd();
                a = JsonConvert.DeserializeObject<ConfigInformation>(Json_config);
            }

        }
        public void Make()//根据当前内存中的Key和Value生成一份aria2.conf文件
        {
            string ConfigFileBody = "";
            foreach (System.Reflection.PropertyInfo p in GlobalConfigInformation.GetType().GetProperties())
            {
                string info = string.Format("{0} = {1}", p.Name.Replace("_", "-"), p.GetValue(GlobalConfigInformation));
                ConfigFileBody = ConfigFileBody + info + Environment.NewLine;

            }
            File.WriteAllText(@"aria2.conf", ConfigFileBody, Encoding.Default); //将ConfigFileBody的内容写入aria2.conf
            SavingConfigFile();
        }
        public void Make(ref ConfigInformation a)//根据当前内存中的Key和Value生成一份aria2.conf文件
        {
            string ConfigFileBody = "";
            foreach (System.Reflection.PropertyInfo p in a.GetType().GetProperties())
            {
                string info = string.Format("{0}={1}", p.Name.Replace("_", "-"), p.GetValue(a));
                ConfigFileBody = ConfigFileBody + info + Environment.NewLine;

            }
            File.WriteAllText(@"aria2.conf", ConfigFileBody, Encoding.Default); //将ConfigFileBody的内容写入aria2.conf
        }


        public void SavingConfigFile() //写配置文件，将全部配置文件保存到json
        {
            string output = JsonConvert.SerializeObject(GlobalConfigInformation);
            File.WriteAllText(@"json.conf", output, Encoding.UTF8); //将output的内容写入json.conf
        }
        public void SavingConfigFile(ConfigInformation e) //写配置文件，将全部配置文件保存到json
        {
            string output = JsonConvert.SerializeObject(e);
            File.WriteAllText(@"json.conf", output, Encoding.UTF8); //将output的内容写入json.conf
        }

        static ConfigController() //构造函数
        {
            LoadConfigFile();
        }

        ~ConfigController() //析构函数
        {
        }
    }
}
