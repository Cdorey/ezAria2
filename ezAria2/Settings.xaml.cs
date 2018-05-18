using Arthas.Controls.Metro;
using Arthas.Utility.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ezAria2
{
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : MetroWindow
    {
        private bool Str2Bol(String a)//一个安全的办法将conf对象中的字符串内容转换回布尔值
        {
            if (a == "true")
            {
                return true;
            }
            return false;
        }

        private class StringToBoolConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                string Value = (string)value;
                if (Value == "1")
                    return true;
                return false;
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

        private ConfigInformation Conf;

        public Settings()
        {
            Conf = Stc.GloConf;
            InitializeComponent();
            dir.Text = Conf.dir;
            disk_cache.Text = Conf.disk_cache;
            switch (Conf.file_allocation)
            {
                case "none":
                    Disk_None.IsChecked = true;
                    break;
                case "falloc":
                    Disk_Falloc.IsChecked = true;
                    break;
                case "trunc":
                    Disk_Trunc.IsChecked = true;
                    break;
                case "prealloc":
                    Disk_Prealloc.IsChecked = true;
                    break;
            }
            if (Conf.@continue == "true")
            {
                @continue.IsChecked = true;
                @continue.Content = "开启";
            }
            else
            {
                @continue.IsChecked = false;
                @continue.Content = "关闭";
            }


            max_concurrent_downloads.Text = Conf.max_concurrent_downloads;
            max_connection_per_server.Text = Conf.max_connection_per_server;
            min_split_size.Text = Conf.min_split_size;
            split.Text = Conf.split;
            max_overall_download_limit.Text = Conf.max_overall_download_limit;
            max_download_limit.Text = Conf.max_download_limit;
            max_overall_upload_limit.Text = Conf.max_overall_upload_limit;
            max_upload_limit.Text = Conf.max_upload_limit;
            disable_ipv6.Text = Conf.disable_ipv6;
            timeout.Text = Conf.timeout;
            max_tries.Text = Conf.max_tries;
            retry_wait.Text = Conf.retry_wait;

            rpc_secret.Text = Conf.rpc_secret;

            follow_torrent.Text = Conf.follow_torrent;
            listen_port.Text = Conf.listen_port;
            bt_max_peers.Text = Conf.bt_max_peers;
            enable_dht.Text = Conf.enable_dht;
            enable_dht6.Text = Conf.enable_dht6;
            dht_listen_port.Text = Conf.dht_listen_port;
            bt_enable_lpd.Text = Conf.bt_enable_lpd;
            enable_peer_exchange.Text = Conf.enable_peer_exchange;
            bt_request_peer_speed_limit.Text = Conf.bt_request_peer_speed_limit;
            peer_id_prefix.Text = Conf.peer_id_prefix;
            user_agent.Text = Conf.user_agent;
            seed_ratio.Text = Conf.seed_ratio;
            force_save.Text = Conf.force_save;
            bt_hash_check_seed.Text = Conf.bt_hash_check_seed;
            bt_seed_unverified.Text = Conf.bt_seed_unverified;
            bt_save_metadata.Text = Conf.bt_save_metadata;
        }

        /// <summary>
        /// 依照目前的代码，任何设置均将在下次启动时生效。即时更新的机制有待补充
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            Conf = Stc.GloConf;
            ConfigController Save = new ConfigController(Conf);
            Save.SavingConfigFile();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
