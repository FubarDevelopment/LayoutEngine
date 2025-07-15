using System.Drawing;

namespace FubarDev.LayoutEngine.Elements;

/// <summary>
/// Represents the root of a layout tree, providing access to client size, display rectangle, and overlap management.
/// </summary>
public interface ILayoutRoot : ILayoutContainer, ILayoutOverlapLookup
{
    /// <summary>
    /// Gets the client size of the layout root.
    /// </summary>
    Size ClientSize { get; }

    /// <summary>
    /// Gets the display rectangle of the layout root.
    /// </summary>
    Rectangle DisplayRectangle { get; }

    /// <summary>
    /// Adds an overlapping layout item to the specified item.
    /// </summary>
    /// <param name="item">The item to be overlapped.</param>
    /// <param name="overlap">The overlapping item.</param>
    void AddOverlap(ILayoutItem item, ILayoutItem overlap);

    /// <summary>
    /// Performs the layout operation for the root and its children.
    /// </summary>
    void Layout();
}
