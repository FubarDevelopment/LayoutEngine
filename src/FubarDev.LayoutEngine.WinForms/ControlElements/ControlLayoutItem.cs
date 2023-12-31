﻿using System.Drawing;
using System.Windows.Forms;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.ControlElements;

public class ControlLayoutItem : ILayoutItem, ISettableMinimumSize, ISettableMargin, ISettablePadding
{
    private readonly Control _control;
    private readonly Visibility _hiddenVisibility;

    public ControlLayoutItem(Control control, Visibility hiddenVisibility = Visibility.Collapsed)
    {
        _control = control;
        _hiddenVisibility = hiddenVisibility;
    }

    public Control Control => _control;
    public string? Name
    {
        get => _control.Name;
        set => _control.Name = value;
    }
    public Point Location => _control.Location;
    public Rectangle Bounds => _control.Bounds;
    public Size Size => _control.Size;
    public Size MinimumSize
    {
        get => _control.MinimumSize;
        set => _control.MinimumSize = value;
    }

    public Size MaximumSize => _control.MaximumSize;
    public int Width => _control.Width;
    public int Height => _control.Height;
    public Visibility Visibility => _control.Visible ? Visibility.Visible : _hiddenVisibility;
    public Margin Margin
    {
        get => _control.Margin.ToMargin();
        set => _control.Margin = value.ToPadding();
    }

    public Margin Padding
    {
        get => _control.Padding.ToMargin();
        set => _control.Padding = value.ToPadding();
    }

    public void SetBounds(Rectangle bounds)
    {
        _control.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
    }
}
