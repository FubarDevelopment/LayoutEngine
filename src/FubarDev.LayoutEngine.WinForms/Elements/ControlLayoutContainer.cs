﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FubarDev.LayoutEngine.Elements;

public class ControlLayoutContainer : ControlLayoutItem, ILayoutContainer
{
    private List<ILayoutItem> _children = new();

    public ControlLayoutContainer(Control control, Visibility hiddenVisibility = Visibility.Collapsed)
        : base(control, hiddenVisibility)
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
    }
}
