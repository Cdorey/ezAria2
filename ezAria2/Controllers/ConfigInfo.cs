using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace ezAria2
{
    /// <summary>
    /// 一份配置文件，实例化得到的是已保存内容，可能和当前Aria2C进程拥有的配置文件不同
    /// </summary>
    public class ConfigInformation 
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

    public class ConfigController //配置文件控制器
    {
        public ConfigInformation Configs = new ConfigInformation();

        /// <summary>
        /// 序列化配置文件为ConfigInformation对象
        /// </summary>
        /// <param name="Json_config">Json形式的Config配置文件</param>
        private void LoadConfigFile(string Json_config)
        {
            Configs = JsonConvert.DeserializeObject<ConfigInformation>(Json_config);
            if (Configs.rpc_secret.Length != 16)
            {
                Random creator = new Random();
                string charsToUse = "AzByCxDwEvFuGtHsIrJqKpLoMnNmOlPkQjRiShTgUfVeWdXcYbZa1234567890";
                int i = 1;
                Configs.rpc_secret = "";
                while (i <= 16)
                {
                    int a = creator.Next(0, 61);
                    string b = charsToUse.Substring(a, 1);
                    Configs.rpc_secret = Configs.rpc_secret + b;
                    i = i + 1;
                }
            }
        }

        /// <summary>
        /// 根据当前实例的Configs属性中的Key和Value生成一份aria2.conf文件
        /// </summary>
        public void Make()
        {
            string ConfigFileBody = "";
            foreach (System.Reflection.PropertyInfo p in Configs.GetType().GetProperties())
            {
                string info = string.Format("{0} = {1}", p.Name.Replace("_", "-"), p.GetValue(Configs));
                ConfigFileBody = ConfigFileBody + info + Environment.NewLine;
            }
            File.WriteAllText(Stc.Config.Aria2cConfigPath, ConfigFileBody, Encoding.Default); //将ConfigFileBody的内容写入aria2.conf
            SavingConfigFile();
        }

        /// <summary>
        /// 写配置文件，将全部配置文件保存到json
        /// </summary>
        public void SavingConfigFile()
        {
            string output = JsonConvert.SerializeObject(Configs);
            File.WriteAllText(@"json.conf", output, Encoding.UTF8); //将output的内容写入json.conf
        }

        public ConfigController() //构造函数
        {
            using (StreamReader sr = new StreamReader(@"json.conf"))
            {
                LoadConfigFile(sr.ReadToEnd());
            }
        }
    }

    public class ApplicationConfig //应用程序自身的配置文件
    {
        public string Aria2cPath;

        public string Aria2cConfigPath;

        public string ApplicationConfigPath;

        public string Aria2cConfigJson;

        public ApplicationConfig()
        {
            Aria2cPath = @"aria2c.exe";
            Aria2cConfigPath = @"aria2.conf";
            ApplicationConfigPath = @"Config.xml";
        }
    }
}
