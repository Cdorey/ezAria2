using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ezAria2
{
    class ConfigFiles
    {
    }
    class ConfigControler
    {
        public string dir = "~/downloads";//文件的保存路径(可使用绝对路径或相对路径), 默认: 当前启动位置
        public string disk_cache = "32M"; //启用磁盘缓存, 0为禁用缓存, 需1.16以上版本, 默认:16M
        public string file_allocation = "falloc";
        public bool @continue = true; //断点续传
        public byte max_concurrent_downloads = 5; //最大同时下载任务数, 运行时可修改, 默认:5
        public byte max_connection_per_server = 5;//同一服务器连接数, 添加时可指定, 默认:1
        public string min_split_size = "10M"; //最小文件分片大小, 添加时可指定, 取值范围1M _1024M, 默认:20M
        public byte split = 5; //单个任务最大线程数, 添加时可指定, 默认:5
        public byte max_overall_download_limit = 0;//整体下载速度限制, 运行时可修改, 默认:0
        public byte max_download_limit = 0;//单个任务下载速度限制, 默认:0
        public byte max_overall_upload_limit = 0;//整体上传速度限制, 运行时可修改, 默认:0
        public byte max_upload_limit = 0;//单个任务上传速度限制, 默认:0
        public bool disable_ipv6 = true;//禁用IPv6, 默认:false
        public byte timeout = 60;//连接超时时间, 默认:60
        public byte max_tries = 5;//最大重试次数, 设置为0表示不限制重试次数, 默认:5
        public byte retry_wait = 0;//设置重试等待的秒数, 默认:0
        public string input_file = "/etc/aria2/aria2.session";//从会话文件中读取下载任务
        public string save_session = "/etc/aria2/aria2.session";//在Aria2退出时保存`错误/未完成`的下载任务到会话文件
        public byte save_session_interval = 60;//定时保存会话, 0为退出时才保存, 需1.16.1以上版本, 默认:0
        public bool enable_rpc = true;//启用RPC
        public bool rpc_allow_origin_all = true;//允许所有来源, 默认:false
        public bool rpc_listen_all = true;//允许非外部访问, 默认:false
        public string @event_poll = "select"; //事件轮询方式, 取值:[epoll, kqueue, port, poll, select], 不同系统默认值不同
        public ushort rpc_listen_port = 6800;//RPC监听端口, 端口被占用时可以修改, 默认:6800
        public string rpc_secret = "<TOKEN>";//设置的RPC授权令牌
        public bool follow_torrent = true;//当下载的是一个种子(以.torrent结尾)时, 自动开始BT任务, 默认:true
        public string listen_port = "51413";//BT监听端口, 当端口被屏蔽时使用, 默认:6881_6999
        public ushort bt_max_peers = 55;//单个种子最大连接数, 默认:55
        public bool enable_dht = false;//打开DHT功能, PT需要禁用, 默认:true
        public bool enable_dht6 = false;//打开IPv6 DHT功能, PT需要禁用
        public string dht_listen_port = "6881-6999";//DHT网络监听端口, 默认:6881_6999
        public bool bt_enable_lpd = false;//本地节点查找, PT需要禁用, 默认:false
        public bool enable_peer_exchange = false;//种子交换, PT需要禁用, 默认:true
        public string bt_request_peer_speed_limit = "50K";
        public string peer_id_prefix = "_TR2770_";
        public string user_agent = "Transmission/2.77";
        public ushort seed_ratio = 0;//当种子的分享率达到这个数时, 自动停止做种, 0为一直做种, 默认:1.0
        public bool force_save = false;//强制保存会话, 即使任务已经完成, 默认:false
        public bool bt_hash_check_seed = true;//BT校验相关, 默认:true
        public bool bt_seed_unverified = true;//继续之前的BT任务时, 无需再次校验, 默认:false
        public bool bt_save_metadata = true;//保存磁力链接元数据为种子文件(.torrent文件), 默认:false
        private bool ShouldBeSaved = false;
        private void LoadingConfigFile()
        {
            //加载aria2.conf文件到类的属性
        }
        public bool SavingConfigFile()
        {
            //写配置文件，将全部类属性写入aria2.conf
            return false;
        }
        public void GiveUp()
        {
            LoadingConfigFile();
        }
        public ConfigControler(bool modify)
        {
            if (modify)
            {
                ShouldBeSaved = true;
            }
            LoadingConfigFile();
        }
        ~ConfigControler()
        {
            if (ShouldBeSaved)
            {
                SavingConfigFile();
            }
        }
    }

}
