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

        private ConfigInformation Conf;

        public Settings()
        {
            Conf = Stc.GloConf;
            InitializeComponent();
            DataContext = Conf;
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
