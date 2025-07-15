using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using FubarDev.LayoutEngine.Elements;
using FubarDev.LayoutEngine.HandleElements;

namespace FubarDev.LayoutEngine.ControlElements;

/// <summary>
/// Represents the root of a layout tree for Windows Forms controls.
/// </summary>
/// <param name="control">The control that serves as the root of the layout tree.</param>
public class ControlLayoutRoot(Control control) : ControlLayoutContainer(control), ILayoutRoot
{
    private static readonly ILayoutItem[] EmptyItems = [];
    private readonly Dictionary<ILayoutItem, List<ILayoutItem>> _overlaps = new();

    /// <inheritdoc />
    public Size ClientSize => Control.ClientSize;
    
    /// <inheritdoc />
    public Rectangle DisplayRectangle => Control.DisplayRectangle;

    /// <inheritdoc />
    public override void Add(ILayoutItem item)
    {
        base.Add(item);

        SetRootWindow(Control, item);
    }

    /// <inheritdoc />
    public void AddOverlap(ILayoutItem item, ILayoutItem overlap)
    {
        if (!_overlaps.TryGetValue(item, out var overlaps))
        {
            overlaps = [];
            _overlaps.Add(item, overlaps);
        }

        overlaps.Add(overlap);

        SetRootWindow(Control, overlap);
    }

    /// <inheritdoc />
    public IReadOnlyCollection<ILayoutItem> GetOverlappingItemsFor(ILayoutItem item)
    {
        if (_overlaps.TryGetValue(item, out var overlaps))
        {
            return overlaps;
        }

        return EmptyItems;
    }

    /// <inheritdoc />
    public void Layout()
    {
        if (LayoutEngine == null)
        {
            return;
        }

        var bounds = DisplayRectangle.Shrink(Padding);
        LayoutEngine.Layout(this, bounds);

        foreach (var overlapItem in _overlaps)
        {
            var item = overlapItem.Key;
            var overlaps = overlapItem.Value;
            foreach (var overlap in overlaps)
            {
                LayoutOverlap(item, overlap);
            }
        }
    }

    private static void LayoutOverlap(
        ILayoutItem item,
        ILayoutItem overlap)
    {
        overlap.SetBounds(item.Bounds);
        if (overlap is ILayoutContainer { LayoutEngine: { } layoutEngine } container)
        {
            layoutEngine.Layout(container, item.Bounds);
        }
    }

    private static void SetRootWindow(IWin32Window rootWindow, ILayoutItem item)
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
