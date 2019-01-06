using Arthas.Controls.Metro;
using System.Threading.Tasks;

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
            FilesListBox.ItemsSource = MainInformation.FilesList;
            PeersListBox.ItemsSource = MainInformation.Peers;
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
