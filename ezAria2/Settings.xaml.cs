﻿using Arthas.Controls.Metro;
using Arthas.Utility.Media;
using System;
using System.Collections.Generic;
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

namespace ezAria2
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : MetroWindow
    {
        public Settings()
        {
           InitializeComponent();
           ConfigControler conf = new ConfigControler(true);
        }
    }
}
