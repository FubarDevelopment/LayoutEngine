using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FubarDev.LayoutEngine.Elements;

public class ControlLayoutRoot : ControlLayoutContainer, ILayoutRoot
{
    private static readonly ILayoutItem[] EmptyItems = new ILayoutItem[0];
    private readonly Dictionary<ILayoutItem, List<ILayoutItem>> _overlaps = new();

    public ControlLayoutRoot(Control control)
        : base(control)
    {
    }

    public Size ClientSize => Control.ClientSize;

    public void AddOverlap(ILayoutItem item, ILayoutItem overlap)
    {
        if (!_overlaps.TryGetValue(item, out var overlaps))
        {
            overlaps = new();
            _overlaps.Add(item, overlaps);
        }

        overlaps.Add(overlap);
    }

    public IReadOnlyCollection<ILayoutItem> GetOverlappingItemsFor(ILayoutItem item)
    {
        if (_overlaps.TryGetValue(item, out var overlaps))
        {
            return overlaps;
        }

        return EmptyItems;
    }

    public void Layout()
    {
        if (LayoutEngine == null)
        {
            return;
        }

        var bounds = new Rectangle(new Point(), ClientSize).Shrink(Margin).Shrink(Padding);
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
}
