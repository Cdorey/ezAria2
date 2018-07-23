using Arthas.Controls.Metro;
using Arthas.Utility.Media;
using System;
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
using DMSkin;
using LiveCharts;
using LiveCharts.Wpf;

namespace ezAria2
{
    /// <summary>
    /// TaskManager.xaml 的交互逻辑
    /// </summary>
    public partial class TaskManager : MetroWindow
    {
        /// <summary>
        /// View Model
        /// </summary>
        private SpeedChart MainChart { get; set; }

        private TaskInformation MainInformation { get; set; }

        public TaskManager(TaskLite e)
        {
            InitializeComponent();
            MainChart = new SpeedChart();
            Chart.DataContext = MainChart;
            MainInformation = new TaskInformation(e);
            Lists.DataContext = MainInformation;
            StartSpeedUpdate();
        }

        private async Task SpeedUpdate()
        {
            MainChart.Add(await MainInformation.GetSpeed());
            System.Threading.Thread.Sleep(1000);
        }

        private async void StartSpeedUpdate()
        {
            await Task.Run( async () =>
            {
                while (true)
                {
                    await SpeedUpdate();
                }
            });
        }
    }
}
