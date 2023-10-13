﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.HandleElements;

public class HwndLayoutContainer : HwndLayoutItem, ILayoutContainer
{
    private List<ILayoutItem> _children = new();

    public HwndLayoutContainer(IWin32Window window, Visibility hiddenVisibility = Visibility.Collapsed)
        : base(window, hiddenVisibility)
    {
    }

    public ILayoutEngine? LayoutEngine { get; set; }

    public IReadOnlyCollection<ILayoutItem> GetChildren()
    {
        return _children;
    }

    public void SetChildren(IReadOnlyCollection<ILayoutItem> children)
    {
        _children = children as List<ILayoutItem> ?? children.ToList();
    }

    public void Add(ILayoutItem item)
    {
        _children.Add(item);

        if (RootWindow != null)
        {
            SetRootWindow(RootWindow, item);
        }
    }

    protected static void SetRootWindow(IWin32Window rootWindow, ILayoutItem item)
    {
        if (item is HwndLayoutItem handleItem)
        {
            handleItem.RootWindow = rootWindow;
        }

        if (item is ILayoutContainer container)
        {
            foreach (var child in container.GetChildren())
            {
                SetRootWindow(rootWindow, child);
            }
        }
    }
}