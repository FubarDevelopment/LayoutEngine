using System.Drawing;
using System.Windows.Forms;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.ControlElements;

/// <summary>
/// Represents a layout item for Windows Forms controls.
/// </summary>
/// <param name="control">The control to be wrapped as a layout item.</param>
/// <param name="hiddenVisibility">The visibility state to use when the control is not visible.</param>
public class ControlLayoutItem(Control control, Visibility hiddenVisibility = Visibility.Collapsed)
    : ILayoutItem, ISettableMinimumSize, ISettableMargin, ISettablePadding
{
    /// <summary>
    /// Gets the control associated with this layout item.
    /// </summary>
    public Control Control => control;

    /// <inheritdoc />
    public string? Name
    {
        get => control.Name;
        set => control.Name = value;
    }

    /// <inheritdoc />
    public Point Location => control.Location;
    
    /// <inheritdoc />
    public Rectangle Bounds => control.Bounds;
    
    /// <inheritdoc />
    public Size Size => control.Size;
    
    /// <inheritdoc cref="ILayoutItem.MinimumSize" />
    public Size MinimumSize
    {
        get => control.MinimumSize;
        set => control.MinimumSize = value;
    }
    
    /// <inheritdoc />
    public Size MaximumSize => control.MaximumSize;
    
    /// <inheritdoc />
    public int Width => control.Width;
    
    /// <inheritdoc />
    public int Height => control.Height;
    
    /// <inheritdoc />
    public Visibility Visibility => control.Visible ? Visibility.Visible : hiddenVisibility;
    
    /// <inheritdoc cref="ILayoutItem.Margin" />
    public Margin Margin
    {
        get => control.Margin.ToMargin();
        set => control.Margin = value.ToPadding();
    }
    
    /// <inheritdoc cref="ILayoutItem.Padding" />
    public Margin Padding
    {
        get => control.Padding.ToMargin();
        set => control.Padding = value.ToPadding();
    }
    
    /// <inheritdoc />
    public void SetBounds(Rectangle bounds)
    {
        control.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
    }
}
