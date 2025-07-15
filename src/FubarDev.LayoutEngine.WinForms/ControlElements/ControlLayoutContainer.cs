using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.ControlElements;

public class ControlLayoutContainer(Control control, Visibility hiddenVisibility = Visibility.Collapsed)
    : ControlLayoutItem(control, hiddenVisibility), ILayoutContainer
{
    private List<ILayoutItem> _children = [];

    public ILayoutEngine? LayoutEngine { get; set; }

    public IReadOnlyCollection<ILayoutItem> GetChildren()
    {
        return _children;
    }

    public void SetChildren(IReadOnlyCollection<ILayoutItem> children)
    {
        _children = children.ToList();
    }

    public virtual void Add(ILayoutItem item)
    {
        _children.Add(item);
    }
}
