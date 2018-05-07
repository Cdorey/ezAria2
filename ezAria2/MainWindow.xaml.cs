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
            Stc.dispatcherTimer.Tick += new EventHandler(ListRefresh);
            Stc.TaskData.TaskFinished += Stc.HistoryData.TaskCompleted;
            //Binding TaskDataBinding = new Binding();
            //TaskDataBinding.Source = Stc.TaskData;
            //TaskDataBinding.Mode = BindingMode.OneWay;
            TasksInProgress.ItemsSource = Stc.TaskData;
            FinishedList.ItemsSource = Stc.HistoryData;
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
                await Stc.TaskData.Update();
            }
            finally
            {
                Stc.dispatcherTimer.Tick += new EventHandler(ListRefresh);
            }
        }


        private void MetroTitleMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TaskManager taskManager = new TaskManager();
            taskManager.Show();
        }

        private void MetroBorder_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            Close();
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
    }
}
