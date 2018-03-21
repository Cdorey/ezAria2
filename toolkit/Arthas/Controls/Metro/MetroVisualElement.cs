﻿using Arthas.Utility.Element;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Arthas.Controls.Metro
{
    public class MetroVisualElement: ContentControl
    {
        public static readonly DependencyProperty VisualProperty = ElementBase.Property<MetroVisualElement, Visual>(nameof(VisualProperty));
        public static readonly DependencyProperty VisualOpacityProperty = ElementBase.Property<MetroVisualElement, double>(nameof(VisualOpacityProperty));
        public static readonly DependencyProperty VisualBlurRadiusProperty = ElementBase.Property<MetroVisualElement, double>(nameof(VisualBlurRadiusProperty));
        public static readonly DependencyProperty LeftProperty = ElementBase.Property<MetroVisualElement, double>(nameof(LeftProperty));
        public static readonly DependencyProperty TopProperty = ElementBase.Property<MetroVisualElement, double>(nameof(TopProperty));

        public Visual Visual { get { return (Visual)GetValue(VisualProperty); } set { SetValue(VisualProperty, value); } }
        public new double VisualOpacity { get { return (double)GetValue(VisualOpacityProperty); } set { SetValue(VisualOpacityProperty, value); } }
        public double VisualBlurRadius { get { return (double)GetValue(VisualBlurRadiusProperty); } set { SetValue(VisualBlurRadiusProperty, value); } }
        public double Left { get { return (double)GetValue(LeftProperty); } set { SetValue(LeftProperty, value); } }
        public double Top { get { return (double)GetValue(TopProperty); } set { SetValue(TopProperty, value); } }

        public MetroVisualElement()
        {
            Utility.Refresh(this);
        }

        static MetroVisualElement()
        {
            ElementBase.DefaultStyle<MetroVisualElement>(DefaultStyleKeyProperty);
        }
    }
}