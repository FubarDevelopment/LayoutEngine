using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

using FubarDev.LayoutEngine.Elements;

using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace FubarDev.LayoutEngine.FrameworkElements;

/// <summary>
/// Represents a layout item for WPF framework elements.
/// </summary>
/// <param name="element">The framework element to be wrapped as a layout item.</param>
public class FrameworkLayoutItem(FrameworkElement element)
    : ILayoutItem, ISettableMargin, ISettableMinimumSize, ISettablePadding
{
    private readonly Control? _control = element as Control;
    private Margin _padding;

    /// <summary>
    /// Gets the wrapped framework element.
    /// </summary>
    public FrameworkElement Element { get; } = element;

    /// <inheritdoc />
    public string? Name
    {
        get => Element.Name;
        set => Element.Name = value;
    }

    /// <inheritdoc />
    public Point Location => Bounds.Location;

    /// <inheritdoc />
    public Rectangle Bounds => new((int)Element.Margin.Left, (int)Element.Margin.Top, (int)Element.ActualWidth, (int)Element.ActualHeight);

    /// <inheritdoc />
    public Size Size => Bounds.Size;

    /// <inheritdoc cref="ILayoutItem.MinimumSize" />
    public Size MinimumSize
    {
        get => new((int)Element.MinWidth, (int)Element.MinHeight);
        set
        {
            Element.MinWidth = value.Width;
            Element.MinHeight = value.Height;
        }
    }

    /// <inheritdoc />
    public Size MaximumSize => new(
        double.IsPositiveInfinity(Element.MaxWidth) ? 0 : (int)Element.MaxWidth,
        double.IsPositiveInfinity(Element.MaxHeight) ? 0 : (int)Element.MaxHeight);

    /// <inheritdoc />
    public int Width => Bounds.Width;

    /// <inheritdoc />
    public int Height => Bounds.Height;

    /// <inheritdoc />
    public Visibility Visibility => Element.Visibility switch
    {
        System.Windows.Visibility.Visible => Visibility.Visible,
        System.Windows.Visibility.Hidden => Visibility.Hidden,
        System.Windows.Visibility.Collapsed => Visibility.Collapsed,
        _ => throw new NotSupportedException(),
    };

    /// <inheritdoc cref="ILayoutItem.Margin" />
    public Margin Margin { get; set; }

    /// <inheritdoc cref="ILayoutItem.Padding" />
    public Margin Padding
    {
        get => GetPadding();
        set => SetPadding(value);
    }

    /// <inheritdoc />
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
