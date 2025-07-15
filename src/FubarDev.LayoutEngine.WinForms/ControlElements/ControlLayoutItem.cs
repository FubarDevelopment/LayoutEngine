using System.Drawing;
using System.Windows.Forms;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.ControlElements;

public class ControlLayoutItem(Control control, Visibility hiddenVisibility = Visibility.Collapsed)
    : ILayoutItem, ISettableMinimumSize, ISettableMargin, ISettablePadding
{
    public Control Control => control;
    public string? Name
    {
        get => control.Name;
        set => control.Name = value;
    }
    public Point Location => control.Location;
    public Rectangle Bounds => control.Bounds;
    public Size Size => control.Size;
    public Size MinimumSize
    {
        get => control.MinimumSize;
        set => control.MinimumSize = value;
    }

    public Size MaximumSize => control.MaximumSize;
    public int Width => control.Width;
    public int Height => control.Height;
    public Visibility Visibility => control.Visible ? Visibility.Visible : hiddenVisibility;
    public Margin Margin
    {
        get => control.Margin.ToMargin();
        set => control.Margin = value.ToPadding();
    }

    public Margin Padding
    {
        get => control.Padding.ToMargin();
        set => control.Padding = value.ToPadding();
    }

    public void SetBounds(Rectangle bounds)
    {
        control.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
    }
}
