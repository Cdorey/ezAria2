using Arthas.Controls.Metro;
using System;
using System.Windows;

namespace ezAria2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// 正在进行的任务列表
        /// </summary>
        public static TaskList TaskData = new TaskList();
        /// <summary>
        /// 已完成任务列表
        /// </summary>
        public static HistoryList HistoryData = new HistoryList();

        public MainWindow()
        {
            InitializeComponent();
            Stc.dispatcherTimer.Tick += new EventHandler(ListRefresh);
            TaskData.TaskFinished += HistoryData.TaskCompleted;
            TasksInProgress.ItemsSource = TaskData;
            FinishedList.ItemsSource = HistoryData;
            Informations.DataContext = Stc.Config;
    }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            Settings set = new Settings();
            set.Show();

        }

        private async void ListRefresh(object sender, EventArgs e)
        {
            Stc.dispatcherTimer.Tick -= new EventHandler(ListRefresh);
            try
            {
                await TaskData.Update();
            }
            finally
            {
                Stc.dispatcherTimer.Tick += new EventHandler(ListRefresh);
            }
        }


        private void MetroTitleMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void MetroBorder_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            if (TasksInProgress.SelectedItem != null)
            {
                await ((TaskLite)TasksInProgress.SelectedItem).Remove();
            }
        }

        private void MetroBorder_MouseLeftButtonUp_NewTask(object sender, RoutedEventArgs e)
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

        private void MetroBorder_MouseLeftButtonUp_StateChange(object sender, RoutedEventArgs e)
        {
            if(TasksInProgress.SelectedItem!=null)
            {
                ((TaskLite)TasksInProgress.SelectedItem).StateChangeFunction();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddTask Task = new AddTask();
            Task.Show();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            TaskManager TaskManager = new TaskManager();
            TaskManager.Show();
        }
    }
}
