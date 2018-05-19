using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Data;

namespace ezAria2
{
    /// <summary>
    /// 一份配置文件，实例化得到的是空白内容
    /// </summary>
    public class ConfigInformation: INotifyPropertyChanged
    {
        private string dir;
        public string Dir
        {
            get
            {
                return dir;
            }
            set
            {
                dir = value;
                OnPropertyChanged("Dir");
            }
        }

        private string disk_cache;
        public string Disk_cache
        {
            get
            {
                return disk_cache;
            }
            set
            {
                disk_cache = value;
                OnPropertyChanged("disk_cache");
            }
        }

        private string file_allocation;
        public string File_allocation
        {
            get
            {
                return file_allocation;
            }
            set
            {
                file_allocation = value;
                OnPropertyChanged("File_allocation");
            }
        }

        private string aria2c_continue;
        public string Continue
        {
            get
            {
                return aria2c_continue;
            }
            set
            {
                aria2c_continue = value;
                OnPropertyChanged("Continue");
            }
        }

        private string max_concurrent_downloads;
        public string Max_concurrent_downloads
        {
            get
            {
                return max_concurrent_downloads;
            }
            set
            {
                max_concurrent_downloads = value;
                OnPropertyChanged("Max_concurrent_downloads");
            }
        }

        private string max_connection_per_server;
        public string Max_connection_per_server
        {
            get
            {
                return max_connection_per_server;
            }
            set
            {
                max_connection_per_server = value;
                OnPropertyChanged("Max_connection_per_server");
            }
        }

        private string min_split_size;
        public string Min_split_size
        {
            get
            {
                return min_split_size;
            }
            set
            {
                min_split_size = value;
                OnPropertyChanged("Min_split_size");
            }
        }

        private string split;
        public string Split
        {
            get
            {
                return split;
            }
            set
            {
                split = value;
                OnPropertyChanged("Split");
            }
        }

        private string max_overall_download_limit;
        public string Max_overall_download_limit
        {
            get
            {
                return max_overall_download_limit;
            }
            set
            {
                max_overall_download_limit = value;
                OnPropertyChanged("Max_overall_download_limit");
            }
        }

        private string max_download_limit;
        public string Max_download_limit
        {
            get
            {
                return max_download_limit;
            }
            set
            {
                max_download_limit = value;
                OnPropertyChanged("Max_download_limit");
            }
        }

        private string max_overall_upload_limit;
        public string Max_overall_upload_limit
        {
            get
            {
                return max_overall_upload_limit;
            }
            set
            {
                max_overall_upload_limit = value;
                OnPropertyChanged("Max_overall_upload_limit");
            }
        }

        private string max_upload_limit;
        public string Max_upload_limit
        {
            get
            {
                return max_upload_limit;
            }
            set
            {
                max_upload_limit = value;
                OnPropertyChanged("Max_upload_limit");
            }
        }

        private string disable_ipv6;
        public string Disable_ipv6
        {
            get
            {
                return disable_ipv6;
            }
            set
            {
                disable_ipv6 = value;
                OnPropertyChanged("Disable_ipv6");
            }
        }

        private string timeout;
        public string Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
                OnPropertyChanged("Timeout");
            }
        }

        private string max_tries;
        public string Max_tries
        {
            get
            {
                return max_tries;
            }
            set
            {
                max_tries = value;
                OnPropertyChanged("Max_tries");
            }
        }

        private string retry_wait;
        public string Retry_wait
        {
            get
            {
                return retry_wait;
            }
            set
            {
                retry_wait = value;
                OnPropertyChanged("Retry_wait");
            }
        }

        private string input_file;
        public string Input_file
        {
            get
            {
                return input_file;
            }
            set
            {
                input_file = value;
                OnPropertyChanged("Input_file");
            }
        }

        private string save_session;
        public string Save_session
        {
            get
            {
                return save_session;
            }
            set
            {
                save_session = value;
                OnPropertyChanged("Save_session");
            }
        }

        private string save_session_interval;
        public string Save_session_interval
        {
            get
            {
                return save_session_interval;
            }
            set
            {
                save_session_interval = value;
                OnPropertyChanged("Save_session_interval");
            }
        }

        private string enable_rpc;
        public string Enable_rpc
        {
            get
            {
                return enable_rpc;
            }
            set
            {
                enable_rpc = value;
                OnPropertyChanged("Enable_rpc");
            }
        }

        private string rpc_allow_origin_all;
        public string Rpc_allow_prigin_all
        {
            get
            {
                return rpc_allow_origin_all;
            }
            set
            {
                rpc_allow_origin_all = value;
                OnPropertyChanged("Rpc_allow_prigin_all");
            }
        }

        private string rpc_listen_all;
        public string Rpc_listen_all
        {
            get
            {
                return rpc_listen_all;
            }
            set
            {
                rpc_listen_all = value;
                OnPropertyChanged("Rpc_listen_all");
            }
        }

        private string event_poll;
        public string Event_poll
        {
            get
            {
                return event_poll;
            }
            set
            {
                event_poll = value;
                OnPropertyChanged("Event_poll");
            }
        }

        private string rpc_listen_port;
        public string Rpc_listen_port
        {
            get
            {
                return rpc_listen_port;
            }
            set
            {
                rpc_listen_port = value;
                OnPropertyChanged("Rpc_listen_port");
            }
        }

        private string rpc_secret;
        public string Rpc_secret
        {
            get
            {
                return rpc_secret;
            }
            set
            {
                rpc_secret = value;
                OnPropertyChanged("Rpc_secret");
            }
        }

        private string follow_torrent;
        public string Follow_torrent
        {
            get
            {
                return follow_torrent;
            }
            set
            {
                follow_torrent = value;
                OnPropertyChanged("Follow_torrent");
            }
        }

        private string listen_port;
        public string Listen_port
        {
            get
            {
                return listen_port;
            }
            set
            {
                listen_port = value;
                OnPropertyChanged("Listen_port");
            }
        }

        private string bt_max_peers;
        public string Bt_max_peers
        {
            get
            {
                return bt_max_peers;
            }
            set
            {
                bt_max_peers = value;
                OnPropertyChanged("Bt_max_peers");
            }
        }

        private string enable_dht;
        public string Enable_dht
        {
            get
            {
                return enable_dht;
            }
            set
            {
                enable_dht = value;
                OnPropertyChanged("Enable_dht");
            }
        }

        private string enable_dht6;
        public string Enable_dht6
        {
            get
            {
                return enable_dht6;
            }
            set
            {
                enable_dht6 = value;
                OnPropertyChanged("Enable_dht6");
            }
        }

        private string dht_listen_port;
        public string Dht_listen_port
        {
            get
            {
                return dht_listen_port;
            }
            set
            {
                dht_listen_port = value;
                OnPropertyChanged("Dht_listen_port");
            }
        }

        private string bt_enable_lpd;
        public string Bt_enable_lpd
        {
            get
            {
                return bt_enable_lpd;
            }
            set
            {
                bt_enable_lpd = value;
                OnPropertyChanged("Bt_enable_lpd");
            }
        }

        private string enable_peer_exchange;
        public string Enable_peer_exchange
        {
            get
            {
                return enable_peer_exchange;
            }
            set
            {
                enable_peer_exchange = value;
                OnPropertyChanged("Enable_peer_exchange");
            }
        }

        private string bt_request_peer_speed_limit;
        public string Bt_request_peer_speed_limit
        {
            get
            {
                return bt_request_peer_speed_limit;
            }
            set
            {
                bt_request_peer_speed_limit = value;
                OnPropertyChanged("Bt_request_peer_speed_limit");
            }
        }

        private string peer_id_prefix;
        public string Peer_id_prefix
        {
            get
            {
                return peer_id_prefix;
            }
            set
            {
                peer_id_prefix = value;
                OnPropertyChanged("Peer_id_prefix");
            }
        }

        private string user_agent;
        public string User_agent
        {
            get
            {
                return user_agent;
            }
            set
            {
                user_agent = value;
                OnPropertyChanged("User_agent");
            }
        }

        private string seed_ratio;
        public string Seed_ratio
        {
            get
            {
                return seed_ratio;
            }
            set
            {
                seed_ratio = value;
                OnPropertyChanged("Seed_ratio");
            }
        }

        private string force_save;
        public string Force_save
        {
            get
            {
                return force_save;
            }
            set
            {
                force_save = value;
                OnPropertyChanged("Force_save");
            }
        }

        private string bt_hash_check_seed;
        public string Bt_hash_check_seed
        {
            get
            {
                return bt_hash_check_seed;
            }
            set
            {
                bt_hash_check_seed = value;
                OnPropertyChanged("Bt_hash_check_seed");
            }
        }

        private string bt_seed_unverified;
        public string Bt_seed_unverified
        {
            get
            {
                return bt_seed_unverified;
            }
            set
            {
                bt_seed_unverified = value;
                OnPropertyChanged("Bt_seed_unverified");
            }
        }

        private string bt_save_metadata;
        public string Bt_save_metadata
        {
            get
            {
                return bt_save_metadata;
            }
            set
            {
                bt_save_metadata = value;
                OnPropertyChanged("Bt_save_metadata");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
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
            if (Configs.Rpc_secret.Length != 16)
            {
                Random creator = new Random();
                string charsToUse = "AzByCxDwEvFuGtHsIrJqKpLoMnNmOlPkQjRiShTgUfVeWdXcYbZa1234567890";
                int i = 1;
                Configs.Rpc_secret = "";
                while (i <= 16)
                {
                    int a = creator.Next(0, 61);
                    string b = charsToUse.Substring(a, 1);
                    Configs.Rpc_secret = Configs.Rpc_secret + b;
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
                string info = string.Format("{0} = {1}", p.Name.Replace("_", "-").ToLower(), p.GetValue(Configs));
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

        /// <summary>
        /// 加载配置文件，来自json.conf
        /// </summary>
        public ConfigController() //构造函数
        {
            using (StreamReader sr = new StreamReader(@"json.conf"))
            {
                LoadConfigFile(sr.ReadToEnd());
            }
        }

        /// <summary>
        /// 接收一个ConfigInformation对象，并对其操作
        /// </summary>
        /// <param name="e">使用时应加入Out前缀</param>
        public ConfigController(ConfigInformation e)
        {
            Configs = e;
        }

        /// <summary>
        /// 接收一个序列化的Json字符串，并对其操作
        /// </summary>
        /// <param name="Json_config"></param>
        public ConfigController(string Json_config)
        {
            LoadConfigFile(Json_config); 
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

    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string Value = (string)value;
            if (Value == "true")
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool Value = (bool)value;
            if (Value)
                return "true";
            else
                return "false";
        }
    }

    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool Value = (bool)value;
            if (Value)
                return "开启";
            return "关闭";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool Value = (bool)value;
            if (Value)
                return "1";
            else
                return "0";
        }
    }

}
