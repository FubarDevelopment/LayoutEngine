using System.Collections.Generic;

namespace FubarDev.LayoutEngine.Elements;

public interface ILayoutOverlapLookup
{
    IReadOnlyCollection<ILayoutItem> GetOverlappingItemsFor(ILayoutItem item);
}
