using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FubarDev.LayoutEngine.Elements;

/// <summary>
/// Represents a layout container that arranges its children and supports overlap lookup.
/// </summary>
/// <param name="overlapLookup">An optional overlap lookup to determine item overlaps.</param>
public class LayoutPane(ILayoutOverlapLookup? overlapLookup = null)
    : ILayoutContainer, ISettableMinimumSize, ISettableMargin, ISettablePadding, IEnumerable<ILayoutItem>
{
    /// <summary>
    /// Gets the default horizontal alignment for a layout pane.
    /// </summary>
    public const HorizontalAlignment DefaultHorizontalAlignment = HorizontalAlignment.Fill;
    /// <summary>
    /// Gets the default vertical alignment for a layout pane.
    /// </summary>
    public const VerticalAlignment DefaultVerticalAlignment = VerticalAlignment.Fill;

    private List<ILayoutItem> _children = [];

    private Rectangle? _bounds;

    /// <summary>
    /// Initializes a new instance of the <see cref="LayoutPane"/> class with an optional overlap lookup.
    /// </summary>
    /// <param name="layoutEngine">The layout engine to use for arranging items within the pane.</param>
    /// <param name="overlapLookup">An optional overlap lookup to determine item overlaps.</param>
    public LayoutPane(ILayoutEngine layoutEngine, ILayoutOverlapLookup? overlapLookup = null)
        : this(overlapLookup)
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
        get => _bounds ??= DetermineDefaultBounds(this, overlapLookup);
        private set => _bounds = new Rectangle(
            new Point(value.Left, value.Top),
            this.EnsureMaximumSize(this.EnsureMinimumSize(new Size(value.Width, value.Height), this.GetEffectiveMinimumSize()), MaximumSize));
    }

    /// <inheritdoc />
    public Size Size => Bounds.Size;
    
    /// <inheritdoc cref="ILayoutItem.MinimumSize" />
    public Size MinimumSize { get; set; }
    
    /// <inheritdoc />
    public Size MaximumSize { get; } = new();
    
    /// <inheritdoc />
    public int Width => Bounds.Width;
    
    /// <inheritdoc />
    public int Height => Bounds.Height;
    
    /// <inheritdoc />
    public Visibility Visibility => DetermineVisibility(GetChildren());

    /// <inheritdoc cref="ILayoutItem.Margin" />
    public Margin Margin { get; set; }
    
    /// <inheritdoc cref="ILayoutItem.Padding" />
    public Margin Padding { get; set; }
    
    /// <inheritdoc />
    public ILayoutEngine? LayoutEngine { get; set; }
    
    /// <inheritdoc />
    public void SetBounds(Rectangle bounds)
    {
        Bounds = bounds;
    }
    
    /// <inheritdoc />
    public IReadOnlyCollection<ILayoutItem> GetChildren()
    {
        return _children;
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

    private Visibility DetermineVisibility(IReadOnlyCollection<ILayoutItem> children)
    {
        if (children.Count == 0)
        {
            return Visibility.Visible;
        }

        return children.Select(x => x.Visibility).Max();
    }
}
