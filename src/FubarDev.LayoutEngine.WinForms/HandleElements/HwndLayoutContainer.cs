using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.HandleElements;

public class HwndLayoutContainer : HwndLayoutItem, ILayoutContainer
{
    private List<ILayoutItem> _children = [];

    public HwndLayoutContainer(IWin32Window window, Visibility hiddenVisibility = Visibility.Collapsed)
        : base(window, hiddenVisibility)
    {
        Margin = new Margin(3, 4, 3, 4);
    }

    public ILayoutEngine? LayoutEngine { get; set; }

    public IReadOnlyCollection<ILayoutItem> GetChildren()
    {
        return _children;
    }

    public void SetChildren(IReadOnlyCollection<ILayoutItem> children)
    {
        _children = children.ToList();
        foreach (var item in children)
        {
            SetRootWindow(item);
        }
    }

    public void Add(ILayoutItem item)
    {
        _children.Add(item);
        SetRootWindow(item);
    }

    protected virtual void SetRootWindow(ILayoutItem item)
    {
        SetRootWindow(RootWindow, item);
    }

    protected static void SetRootWindow(IWin32Window rootWindow, ILayoutItem item)
    {
        switch (item)
        {
            case HwndLayoutItem handleItem:
                handleItem.RootWindow = rootWindow;
                break;

            case ILayoutRoot:
                break;

            case ILayoutContainer container:
                foreach (var child in container.GetChildren())
                {
                    SetRootWindow(rootWindow, child);
                }
                break;
        }
    }
}
