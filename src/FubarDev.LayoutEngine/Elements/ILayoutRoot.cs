using System.Drawing;

namespace FubarDev.LayoutEngine.Elements;

public interface ILayoutRoot : ILayoutContainer, ILayoutOverlapLookup
{
    Size ClientSize { get; }

    void AddOverlap(ILayoutItem item, ILayoutItem overlap);

    void Layout();
}
