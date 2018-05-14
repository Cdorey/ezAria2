using Arthas.Controls.Metro;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;


namespace ezAria2
{
    /// <summary>
    /// AddTask.xaml 的交互逻辑
    /// </summary>
    public partial class AddTask : MetroWindow
    {
        public AddTask()
        {
            InitializeComponent();
        }

        private void MetroTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(UriBox.Text== "请输入链接地址，每行一个独立的下载任务")
            {
                UriBox.Text = "";
                BT.IsEnabled = false;
                MT.IsEnabled = false;
            }
        }

        private void MetroButton_Click_Add(object sender, RoutedEventArgs e)
        {
            Add();
            Close();
        }

        private void MetroButton_Click_Cancle(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private async void Add()
        {
            string[] DownloadUris = UriBox.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string uri in DownloadUris)
            {
                await Aria2Methords.AddUri(uri);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();
            if (iData.GetDataPresent(DataFormats.Text))
            {
                UriBox.Text = (string)iData.GetData(DataFormats.Text)+Environment.NewLine;
                UriBox.Select(UriBox.Text.Length, 0);
            }
        }

        private async void BT_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "BT下载的种子文件 (*.torrent)|*.torrent"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                await Aria2Methords.AddTorrent(openFileDialog.FileName);
                Close();
            }
        }

        private async void MT_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "MetaLink文件 (*.metalink)|*.metalink"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                await Aria2Methords.AddMetalink(openFileDialog.FileName);
                Close();
            }
        }
    }
}
