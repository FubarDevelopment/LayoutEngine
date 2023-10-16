using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

using FubarDev.LayoutEngine.Elements;

using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace FubarDev.LayoutEngine.FrameworkElements;

public class FrameworkLayoutItem : ILayoutItem, ISettableMargin, ISettableMinimumSize, ISettablePadding
{
    private readonly Control? _control;
    private Margin _padding;

    public FrameworkLayoutItem(FrameworkElement element)
    {
        Element = element;
        _control = element as Control;
    }

    public FrameworkElement Element { get; }

    public string? Name
    {
        get => Element.Name;
        set => Element.Name = value;
    }

    public Point Location => Bounds.Location;
    public Rectangle Bounds => new((int)Element.Margin.Left, (int)Element.Margin.Top, (int)Element.ActualWidth, (int)Element.ActualHeight);
    public Size Size => Bounds.Size;
    public Size MinimumSize
    {
        get => new((int)Element.MinWidth, (int)Element.MinHeight);
        set
        {
            Element.MinWidth = value.Width;
            Element.MinHeight = value.Height;
        }
    }

    public Size MaximumSize => new(
        double.IsPositiveInfinity(Element.MaxWidth) ? 0 : (int)Element.MaxWidth,
        double.IsPositiveInfinity(Element.MaxHeight) ? 0 : (int)Element.MaxHeight);
    public int Width => Bounds.Width;
    public int Height => Bounds.Height;

    public Visibility Visibility => Element.Visibility switch
    {
        System.Windows.Visibility.Visible => Visibility.Visible,
        System.Windows.Visibility.Hidden => Visibility.Hidden,
        System.Windows.Visibility.Collapsed => Visibility.Collapsed,
        _ => throw new NotSupportedException(),
    };

    public Margin Margin { get; set; }

    public Margin Padding
    {
        get => GetPadding();
        set => SetPadding(value);
    }

    public void SetBounds(Rectangle bounds)
    {
        Element.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
        Element.VerticalAlignment = System.Windows.VerticalAlignment.Top;
        Element.Margin = new Thickness(bounds.X, bounds.Y, 0, 0);
        Element.Width = bounds.Width;
        Element.Height = bounds.Height;
    }

    private void SetPadding(Margin value)
    {
        if (_control == null)
        {
            _padding = value;
            return;
        }

        _control.Padding = new Thickness(value.Left, value.Top, value.Right, value.Bottom);
    }

    private Margin GetPadding()
    {
        if (_control == null)
        {
            return _padding;
        }

        return new Margin(
            (int)_control.Padding.Left,
            (int)_control.Padding.Top,
            (int)_control.Padding.Right,
            (int)_control.Padding.Bottom);
    }
}
