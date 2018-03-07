using Arthas.Controls.Metro;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Xml.Linq;

namespace ezAria2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ((TaskList)this.FindResource("TaskData")).Refresh();
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            Settings set = new Settings();
            set.Show();
        }

        private void MetroTitleMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TaskManager taskManager = new TaskManager();
            taskManager.Show();
        }

        private void MetroBorder_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void MetroBorder_MouseLeftButtonUp_NewTask(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AddTask Task = new AddTask();
            Task.Show();
        }

        private void ForDeveloper_ButtonClick(object sender, System.EventArgs e)
        {
        }
    }
}
