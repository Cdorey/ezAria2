﻿using Arthas.Utility.Element;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Arthas.Controls.Metro
{
    public class MetroTabItem : TabItem
    {
        public static readonly DependencyProperty IconProperty = ElementBase.Property<MetroTabItem, ImageSource>(nameof(IconProperty), null);

        public ImageSource Icon { get { return (ImageSource)GetValue(IconProperty); } set { SetValue(IconProperty, value); } }

        static MetroTabItem()
        {
            ElementBase.DefaultStyle<MetroTabItem>(DefaultStyleKeyProperty);
        }
    }
}