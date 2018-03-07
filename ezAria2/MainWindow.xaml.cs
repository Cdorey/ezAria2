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

        private void MetroButton_Click(object sender, RoutedEventArgs e)
        {
            AddTask Add = new AddTask();
            Add.Show();
        }
    }
}
