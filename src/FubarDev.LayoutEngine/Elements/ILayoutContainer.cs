using System.Collections.Generic;

namespace FubarDev.LayoutEngine.Elements;

public interface ILayoutContainer : ILayoutItem
{
    ILayoutEngine? LayoutEngine { get; set; }

    IReadOnlyCollection<ILayoutItem> GetChildren();

    void SetChildren(IReadOnlyCollection<ILayoutItem> children);

    void Add(ILayoutItem item);
}
