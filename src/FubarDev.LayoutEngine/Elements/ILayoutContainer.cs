using System.Collections.Generic;

namespace FubarDev.LayoutEngine.Elements;

/// <summary>
/// Represents a container that can hold multiple layout items and be managed by a layout engine.
/// </summary>
public interface ILayoutContainer : ILayoutItem
{
    /// <summary>
    /// Gets or sets the layout engine used to arrange the children.
    /// </summary>
    ILayoutEngine? LayoutEngine { get; set; }

    /// <summary>
    /// Gets the collection of child layout items.
    /// </summary>
    /// <returns>The read-only collection of child items.</returns>
    IReadOnlyCollection<ILayoutItem> GetChildren();

    /// <summary>
    /// Sets the collection of child layout items.
    /// </summary>
    /// <param name="children">The collection of child items to set.</param>
    void SetChildren(IReadOnlyCollection<ILayoutItem> children);

    /// <summary>
    /// Adds a child layout item to the container.
    /// </summary>
    /// <param name="item">The item to add.</param>
    void Add(ILayoutItem item);
}
