﻿using System.Windows.Controls;

namespace Arthas.Controls.Metro
{
    /*
    public class MetroTextBlock : ContentControl
    {

        public static readonly DependencyProperty SpacingProperty = ElementBase.Property<MetroTextBlock, Thickness>(nameof(SpacingProperty), new Thickness(0.25));
        public Thickness Spacing
        {
            get { return (Thickness)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = ElementBase.Property<MetroTextBlock, string>(nameof(TextProperty), "");
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        static MetroTextBlock()
        {
            ElementBase.DefaultStyle<MetroTextBlock>(DefaultStyleKeyProperty);
        }
    }
    */
    public class MetroTextBlock : TextBlock
    {

    }
}