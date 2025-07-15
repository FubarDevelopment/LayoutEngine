using System.Collections;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.Test.TestElements;

public class TestContainer : ILayoutContainer, IEnumerable<ILayoutItem>, ISettableMinimumSize
{
    private List<ILayoutItem> _children = [];
    private Rectangle? _bounds;
    private ILayoutOverlapLookup? _overlapLookup;

    public TestContainer()
    {
    }

    public TestContainer(ILayoutEngine layoutEngine)
    {
        LayoutEngine = layoutEngine;
    }

    /// <inheritdoc />
    public string? Name { get; set; }
    
    /// <inheritdoc />
    public Point Location => Bounds.Location;

    /// <inheritdoc />
    public Rectangle Bounds
    {
        get => _bounds ??= DetermineDefaultBounds(this, _overlapLookup);
        set => _bounds = new Rectangle(
            new Point(value.Left, value.Top),
            this.EnsureMaximumSize(this.EnsureMinimumSize(new Size(value.Width, value.Height), this.GetEffectiveMinimumSize()), MaximumSize));
    }

    /// <inheritdoc />
    public Size Size => Bounds.Size;
    
    /// <inheritdoc cref="ILayoutItem.MinimumSize" />
    public Size MinimumSize { get; set; }
    
    /// <inheritdoc />
    public Size MaximumSize { get; init; }
    
    /// <inheritdoc />
    public int Width => Bounds.Width;
    
    /// <inheritdoc />
    public int Height => Bounds.Height;
    
    /// <inheritdoc />
    public Visibility Visibility => Visibility.Visible;
    
    /// <inheritdoc />
    public Margin Margin { get; set; }
    
    /// <inheritdoc />
    public Margin Padding { get; init; }

    /// <inheritdoc />
    public ILayoutEngine? LayoutEngine { get; set; }

    /// <inheritdoc />
    public void SetBounds(Rectangle bounds)
    {
        _bounds = bounds;
    }

    /// <inheritdoc />
    public IReadOnlyCollection<ILayoutItem> GetChildren()
    {
        return _children.AsReadOnly();
    }

    /// <inheritdoc />
    public void SetChildren(IReadOnlyCollection<ILayoutItem> children)
    {
        _children = children.ToList();
    }

    /// <inheritdoc />
    public void Add(ILayoutItem item)
    {
        _children.Add(item);
    }

    /// <inheritdoc />
    public IEnumerator<ILayoutItem> GetEnumerator()
    {
        return GetChildren().GetEnumerator();
    }

    /// <inheritdoc />
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
