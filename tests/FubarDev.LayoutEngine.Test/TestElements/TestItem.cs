using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.Test.TestElements;

public class TestItem : ILayoutItem, ISettableMinimumSize
{
    private Rectangle? _bounds;

    public string? Name { get; set; }
    public Point Location => Bounds.Location;
    public Rectangle Bounds
    {
        get => _bounds ?? Rectangle.Empty;
        set => _bounds = value;
    }
    public Size Size => Bounds.Size;
    public Size MinimumSize { get; set; }
    public Size MaximumSize { get; init; }
    public int Width => Bounds.Width;
    public int Height => Bounds.Height;
    public Visibility Visibility { get; set; } = Visibility.Visible;
    public Margin Margin { get; init; }
    public Margin Padding { get; init; }
    public void SetBounds(Rectangle bounds)
    {
        _bounds = bounds;
    }
}
