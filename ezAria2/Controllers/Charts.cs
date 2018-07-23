using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.ComponentModel;

namespace ezAria2
{
    /// <summary>
    /// 一个速度曲线图
    /// </summary>
    public class SpeedChart: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 一系列线的集合，绑定Chart的Series属性；
        /// </summary>
        public SeriesCollection Lines { get; set; }

        public Axis DateTimeAxis { get; set; }

        public Axis SpeedAxis { get; set; }

        /// <summary>
        /// SpeedLine上的点数据
        /// </summary>
        public class SpeedLinePoint
        {
            public long DateTime { get; set; }
            public long Value { get; set; }
        }

        /// <summary>
        /// SpeedLine的值
        /// </summary>
        public ChartValues<SpeedLinePoint> DownloadSpeedValues { get; set; }

        /// <summary>
        /// 速度曲线
        /// </summary>
        public LineSeries DownloadSpeedLine { get; set; }

        /// <summary>
        /// SpeedLine的值
        /// </summary>
        public ChartValues<SpeedLinePoint> UploadSpeedValues { get; set; }

        /// <summary>
        /// 速度曲线
        /// </summary>
        public LineSeries UploadSpeedLine { get; set; }

        /// <summary>
        /// X轴 时间
        /// </summary>
        public Func<double, string> DateTimeFormatter { get; set; }

        private long AxisLength { get; set; }//时间轴长度，默认60（秒）

        /// <summary>
        /// X轴最大值
        /// </summary>
        public long AxisMax
        {
            get { return _axisMax; }
            set
            {
                _axisMax = value;
                OnPropertyChanged("AxisMax");
            }
        }
        private long _axisMax;

        /// <summary>
        /// X轴最小值
        /// </summary>
        public long AxisMin
        {
            get { return _axisMin; }
            set
            {
                _axisMin = value;
                OnPropertyChanged("AxisMin");
            }
        }
        private long _axisMin;

        /// <summary>
        /// 创建时间轴
        /// </summary>
        /// <param name="now">以该时刻作为终点</param>
        private void SetAxisLimits(DateTime now)
        {
            AxisMax = (now.Ticks + TimeSpan.FromSeconds(1).Ticks);
            AxisMin = (now.Ticks - TimeSpan.FromSeconds(AxisLength - 1).Ticks);
        }

        /// <summary>
        /// Y轴 速度
        /// </summary>
        public Func<double, string> SpeedDataFormatter { get; set; }

        /// <summary>
        /// 初始化轴标签格式化工具
        /// </summary>
        private void NewFormatter()
        {
            DateTimeFormatter = value =>
            {
                return new DateTime((long)value).ToString("hh:mm:ss");
            };

            SpeedDataFormatter = value =>
            {
                if (value / 1024 <= 0)
                    return Math.Round(value, 2).ToString() + "B/S";
                else if (value / 1048576 <= 0)
                    return Math.Round((value / 1024), 2).ToString() + "KB/S";
                else
                    return Math.Round((value / 1048578), 2).ToString() + "MB/S";
            };
        }


        //public int AxisStep { get; set; }

        //public int AxisUnit { get; set; }

        /// <summary>
        /// 增加一个点
        /// </summary>
        /// <param name="Speed">当前速度</param>
        public void Add(long[] Speed)
        {
            var Now = DateTime.Now;
            DownloadSpeedValues.Add(new SpeedLinePoint
            {
                DateTime = Now.Ticks,
                Value = Speed[0]
            });

            UploadSpeedValues.Add(new SpeedLinePoint
            {
                DateTime = Now.Ticks,
                Value = Speed[1]
            });

            SetAxisLimits(DateTime.Now);
            //超出点数后移除最初的
            if (DownloadSpeedValues.Count > 100) DownloadSpeedValues.RemoveAt(0);
            if (UploadSpeedValues.Count > 100) UploadSpeedValues.RemoveAt(0);
        }

        public SpeedChart()
        {
            DownloadSpeedValues = new ChartValues<SpeedLinePoint>();
            UploadSpeedValues = new ChartValues<SpeedLinePoint>();

            NewFormatter();
            var mapper = Mappers.Xy<SpeedLinePoint>()
                .X(model => model.DateTime)   //use DateTime.Ticks as X
                .Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
            Charting.For<SpeedLinePoint>(mapper);
            AxisLength = 60;
            SetAxisLimits(DateTime.Now);
            DownloadSpeedLine = new LineSeries
            {
                Values = DownloadSpeedValues
            };
            UploadSpeedLine = new LineSeries
            {
                Values = UploadSpeedValues
            };
            Lines = new SeriesCollection
            {
                DownloadSpeedLine,
                UploadSpeedLine
            };

        }
    }


}