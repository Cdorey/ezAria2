﻿using System.Windows;

namespace Arthas.Themes.Metro
{
    public partial class MetroWindow
    {
        void Minimized(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(sender as FrameworkElement).WindowState = WindowState.Minimized;
        }
        void Normal(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(sender as FrameworkElement).WindowState = WindowState.Normal;
        }
        void Maximized(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(sender as FrameworkElement).WindowState = WindowState.Maximized;
        }
        void Close(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(sender as FrameworkElement).Close();
        }
    }
}