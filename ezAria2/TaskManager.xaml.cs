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
        //private TaskInformation TaskInformation;
        private Test TestObj = new Test();
        public TaskManager()
        {
            InitializeComponent();
            //TaskInformation = new TaskInformation("helloworld");
            //DataContext = TaskInformation;
            DataContext = TestObj;
        }

        class Test
        {
            public SeriesCollection SpeedLine { get;set;}
            public Test()
            {
                SpeedLine = new SeriesCollection();
                ChartValues<int> LineValues = new ChartValues<int> { 1, 2, 3, 4 };
                SpeedLine.Add(new LineSeries
                {
                    Values = LineValues
                });
            }
        }
    }
}
