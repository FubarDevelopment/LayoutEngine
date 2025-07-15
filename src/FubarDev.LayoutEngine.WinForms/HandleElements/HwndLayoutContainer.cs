using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.HandleElements;

/// <summary>
/// Represents a layout container for native Win32 windows.
/// </summary>
public class HwndLayoutContainer : HwndLayoutItem, ILayoutContainer
{
    private List<ILayoutItem> _children = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="HwndLayoutContainer"/> class with the specified window.
    /// </summary>
    /// <param name="window">The window to be wrapped as a layout container.</param>
    /// <param name="hiddenVisibility">The visibility state when the window is hidden.</param>
    public HwndLayoutContainer(IWin32Window window, Visibility hiddenVisibility = Visibility.Collapsed)
        : base(window, hiddenVisibility)
    {
        Margin = new Margin(3, 4, 3, 4);
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
        foreach (var item in children)
        {
            SetRootWindow(item);
        }
    }

    /// <inheritdoc />
    public void Add(ILayoutItem item)
    {
        _children.Add(item);
        SetRootWindow(item);
    }

    /// <summary>
    /// Sets the root window for the specified layout item.
    /// </summary>
    /// <param name="item">The layout item to set the root window for.</param>
    protected virtual void SetRootWindow(ILayoutItem item)
    {
        SetRootWindow(RootWindow, item);
    }

    /// <summary>
    /// Sets the root window for the specified layout item and its children recursively.
    /// </summary>
    /// <param name="rootWindow">The root window to set for the layout item.</param>
    /// <param name="item">The layout item to set the root window for.</param>
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
