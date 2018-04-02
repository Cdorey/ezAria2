using Arthas.Controls.Metro;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
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
            ((TaskList)FindResource("TaskData")).Refresh();
            Stc.dispatcherTimer.Tick += new EventHandler(ListRefresh);
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            Settings set = new Settings();
            set.Show();
        }

        private async void ListRefresh(object sender, EventArgs e)
        {
            Stc.dispatcherTimer.Tick -= new EventHandler(ListRefresh);
            await ((TaskList)FindResource("TaskData")).Update();
            Stc.dispatcherTimer.Tick += new EventHandler(ListRefresh);
        }


        private void MetroTitleMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TaskManager taskManager = new TaskManager();
            taskManager.Show();
        }

        private void MetroBorder_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Close();
        }

        private void MetroBorder_MouseLeftButtonUp_NewTask(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AddTask Task = new AddTask();
            Task.Show();
        }

        private void ForDeveloper_ButtonClick(object sender, EventArgs e)
        {
        }

        private void MetroWindow_Activated(object sender, EventArgs e)
        {
        }

        private void MetroBorder_MouseLeftButtonUp_StateChange(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(TasksInProgress.SelectedItem!=null)
            {
                ((TaskLite)TasksInProgress.SelectedItem).StateChangeFunction();
            }
        }
    }
}
