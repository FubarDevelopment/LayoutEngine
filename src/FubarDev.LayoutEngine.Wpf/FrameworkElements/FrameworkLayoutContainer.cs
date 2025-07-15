using System.Collections.Generic;
using System.Linq;
using System.Windows;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.FrameworkElements;

/// <summary>
/// Represents a layout container for WPF framework elements.
/// </summary>
public class FrameworkLayoutContainer : FrameworkLayoutItem, ILayoutContainer
{
    private List<ILayoutItem> _children = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="FrameworkLayoutContainer"/> class with the specified framework element.
    /// </summary>
    /// <param name="element">The framework element to be wrapped as a layout container.</param>
    public FrameworkLayoutContainer(FrameworkElement element)
        : base(element)
    {
        Margin = new Margin(3, 4);
    }

    /// <inheritdoc />
    public ILayoutEngine? LayoutEngine { get; set; }

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
}
