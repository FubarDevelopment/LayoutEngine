using System.Collections.Generic;
using System.Linq;
using System.Windows;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.FrameworkElements;

public class FrameworkLayoutContainer : FrameworkLayoutItem, ILayoutContainer
{
    private List<ILayoutItem> _children = new();

    public FrameworkLayoutContainer(FrameworkElement element)
        : base(element)
    {
        Margin = new Margin(3, 4);
    }

    public ILayoutEngine? LayoutEngine { get; set; }

    public IReadOnlyCollection<ILayoutItem> GetChildren()
    {
        return _children;
    }

    public void SetChildren(IReadOnlyCollection<ILayoutItem> children)
    {
        _children = children.ToList();
    }

    public void Add(ILayoutItem item)
    {
        _children.Add(item);
    }
}
