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
        private ConfigControler conf = new ConfigControler();//初始化配置文件控制器对象
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
            dir.Text = conf.Get("dir");
            disk_cache.Text = conf.Get("disk_cache");
        }
    }
}
