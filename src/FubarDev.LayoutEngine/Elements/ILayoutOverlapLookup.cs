using System.Collections.Generic;

namespace FubarDev.LayoutEngine.Elements;

/// <summary>
/// Provides lookup functionality for overlapping layout items.
/// </summary>
public interface ILayoutOverlapLookup
{
    /// <summary>
    /// Gets the collection of layout items that overlap with the specified item.
    /// </summary>
    /// <param name="item">The layout item to check for overlaps.</param>
    /// <returns>The collection of overlapping layout items.</returns>
    IReadOnlyCollection<ILayoutItem> GetOverlappingItemsFor(ILayoutItem item);
}
