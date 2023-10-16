using System.Collections;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.Test.TestElements;

public class TestContainer : ILayoutContainer, IEnumerable<ILayoutItem>, ISettableMinimumSize
{
    private List<ILayoutItem> _children = new();
    private Rectangle? _bounds;
    private ILayoutOverlapLookup? _overlapLookup;

    public TestContainer()
    {
    }

    public TestContainer(ILayoutEngine layoutEngine)
    {
        LayoutEngine = layoutEngine;
    }

    public string? Name { get; set; }
    public Point Location => Bounds.Location;
    public Rectangle Bounds
    {
        get => _bounds ??= DetermineDefaultBounds(this, _overlapLookup);
        set => _bounds = new Rectangle(
            new Point(value.Left, value.Top),
            this.EnsureMaximumSize(this.EnsureMinimumSize(new Size(value.Width, value.Height), this.GetEffectiveMinimumSize()), MaximumSize));
    }
    public Size Size => Bounds.Size;
    public Size MinimumSize { get; set; }
    public Size MaximumSize { get; init; }
    public int Width => Bounds.Width;
    public int Height => Bounds.Height;
    public Visibility Visibility => Visibility.Visible;
    public Margin Margin { get; set; }
    public Margin Padding { get; init; }
    public ILayoutEngine? LayoutEngine { get; set; }
    public void SetBounds(Rectangle bounds)
    {
        _bounds = bounds;
    }

    public IReadOnlyCollection<ILayoutItem> GetChildren()
    {
        return _children.AsReadOnly();
    }

    public void SetChildren(IReadOnlyCollection<ILayoutItem> children)
    {
        _children = children.ToList();
    }

    public void Add(ILayoutItem item)
    {
        _children.Add(item);
    }

    public IEnumerator<ILayoutItem> GetEnumerator()
    {
        return GetChildren().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private static Rectangle DetermineDefaultBounds(ILayoutContainer container, ILayoutOverlapLookup? overlapLookup)
    {
        var minimumSize = container.DetermineMinimumSize(overlapLookup);
        return new Rectangle(
            0, 0,
            minimumSize.Width, minimumSize.Height);
    }
}
