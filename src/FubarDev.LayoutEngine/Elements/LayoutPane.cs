using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FubarDev.LayoutEngine.Elements;

public class LayoutPane : ILayoutContainer, IEnumerable<ILayoutItem>
{
    public const HorizontalAlignment DefaultHorizontalAlignment = HorizontalAlignment.Fill;
    public const VerticalAlignment DefaultVerticalAlignment = VerticalAlignment.Fill;
    private readonly ILayoutOverlapLookup? _overlapLookup;
    private List<ILayoutItem> _children = new();
    private Rectangle? _bounds;

    public LayoutPane(ILayoutOverlapLookup? overlapLookup = null)
    {
        _overlapLookup = overlapLookup;
    }

    public LayoutPane(ILayoutEngine layoutEngine, ILayoutOverlapLookup? overlapLookup = null)
        : this(overlapLookup)
    {
        LayoutEngine = layoutEngine;
    }

    public string? Name { get; set; }
    public Point Location => Bounds.Location;

    public Rectangle Bounds
    {
        get => _bounds ?? DetermineDefaultBounds(this, _overlapLookup);
        private set => _bounds = new Rectangle(
            value.Left, value.Top,
            this.EnsureMaximumWidth(this.EnsureMinimumWidth(value.Width)),
            this.EnsureMaximumHeight(this.EnsureMinimumHeight(value.Height)));
    }

    public Size Size => Bounds.Size;
    public Size MinimumSize { get; } = new();
    public Size MaximumSize { get; } = new();
    public int Width => Bounds.Width;
    public int Height => Bounds.Height;
    public Visibility Visibility => DetermineVisibility(GetChildren());
    public Margin Margin { get; init; }
    public Margin Padding { get; init; }
    public ILayoutEngine? LayoutEngine { get; set; }

    public void SetBounds(Rectangle bounds)
    {
        Bounds = bounds;
    }

    public IReadOnlyCollection<ILayoutItem> GetChildren()
    {
        return _children;
    }

    public void SetChildren(IReadOnlyCollection<ILayoutItem> children)
    {
        _children = children as List<ILayoutItem> ?? children.ToList();
    }

    public void Add(ILayoutItem item)
    {
        _children.Add(item);
    }

    private static Rectangle DetermineDefaultBounds(ILayoutContainer container, ILayoutOverlapLookup? overlapLookup)
    {
        var minimumSize = container.GetMinimumSize(overlapLookup);
        return new Rectangle(
            0, 0,
            minimumSize.Width, minimumSize.Height);
    }

    private Visibility DetermineVisibility(IReadOnlyCollection<ILayoutItem> children)
    {
        if (children.Count == 0)
        {
            return Visibility.Visible;
        }

        return children.Select(x => x.Visibility).Max();
    }

    public IEnumerator<ILayoutItem> GetEnumerator()
    {
        return GetChildren().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
