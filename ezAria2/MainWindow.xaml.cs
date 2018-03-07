using Arthas.Controls.Metro;
using System.Windows;

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
            ((TaskList)this.FindResource("TaskData")).Add(new TaskLite { Icon = "Resources/bonfire-1849089_640.png", Progress = 20D, Speed = "300", Gid = "12345678", FileName = "test task" });
            ((TaskList)this.FindResource("TaskData")).Add(new TaskLite { Icon = "Resources/bonfire-1849089_640.png", Progress = 20D, Speed = "300", Gid = "12345678", FileName = "test task" });
            ((TaskList)this.FindResource("TaskData")).Add(new TaskLite { Icon = "Resources/bonfire-1849089_640.png", Progress = 20D, Speed = "300", Gid = "12345678", FileName = "test task" });
            ((TaskList)this.FindResource("TaskData")).Add(new TaskLite { Icon = "Resources/stopwatch-1849088_640.png", State = "wait", Gid = "12345678", FileName = "test task" });
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
    }
}
