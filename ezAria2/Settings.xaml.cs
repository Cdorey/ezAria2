using Arthas.Controls.Metro;
using Arthas.Utility.Media;
using System;
using System.Collections.Generic;
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
            if (a=="true")
            {
                return true;
            }
            return false;
        }

        public Settings()
        {
            InitializeComponent();
            dir.Text = Stc.GloConf.dir;
            disk_cache.Text = Stc.GloConf.disk_cache;
            switch (Stc.GloConf.file_allocation)
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
            if(Stc.GloConf.@continue=="true")
            {
                @continue.IsChecked = true;
                @continue.Content = "开启";
            }
            else
            {
                @continue.IsChecked = false;
                @continue.Content = "关闭";
            }


            max_concurrent_downloads.Text = Stc.GloConf.max_concurrent_downloads;
            max_connection_per_server.Text = Stc.GloConf.max_connection_per_server;
            min_split_size.Text = Stc.GloConf.min_split_size;
            split.Text = Stc.GloConf.split;
            max_overall_download_limit.Text = Stc.GloConf.max_overall_download_limit;
            max_download_limit.Text = Stc.GloConf.max_download_limit;
            max_overall_upload_limit.Text = Stc.GloConf.max_overall_upload_limit;
            max_upload_limit.Text = Stc.GloConf.max_upload_limit;
            disable_ipv6.Text = Stc.GloConf.disable_ipv6;
            timeout.Text = Stc.GloConf.timeout;
            max_tries.Text = Stc.GloConf.max_tries;
            retry_wait.Text = Stc.GloConf.retry_wait;

            rpc_secret.Text = Stc.GloConf.rpc_secret;

            follow_torrent.Text = Stc.GloConf.follow_torrent;
            listen_port.Text = Stc.GloConf.listen_port;
            bt_max_peers.Text = Stc.GloConf.bt_max_peers;
            enable_dht.Text = Stc.GloConf.enable_dht;
            enable_dht6.Text = Stc.GloConf.enable_dht6;
            dht_listen_port.Text = Stc.GloConf.dht_listen_port;
            bt_enable_lpd.Text = Stc.GloConf.bt_enable_lpd;
            enable_peer_exchange.Text = Stc.GloConf.enable_peer_exchange;
            bt_request_peer_speed_limit.Text = Stc.GloConf.bt_request_peer_speed_limit;
            peer_id_prefix.Text = Stc.GloConf.peer_id_prefix;
            user_agent.Text = Stc.GloConf.user_agent;
            seed_ratio.Text = Stc.GloConf.seed_ratio;
            force_save.Text = Stc.GloConf.force_save;
            bt_hash_check_seed.Text = Stc.GloConf.bt_hash_check_seed;
            bt_seed_unverified.Text = Stc.GloConf.bt_seed_unverified;
            bt_save_metadata.Text = Stc.GloConf.bt_save_metadata;
        }
    }
}
