using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.Test.TestElements;

public class TestRoot(ILayoutEngine layoutEngine) : TestContainer(layoutEngine), ILayoutRoot
{
    private readonly Dictionary<ILayoutItem, List<ILayoutItem>> _overlaps = new();

    public Size ClientSize => Size;
    public Rectangle DisplayRectangle => new(Point.Empty, ClientSize);

    public IReadOnlyCollection<ILayoutItem> GetOverlappingItemsFor(ILayoutItem item)
    {
        if (_overlaps.TryGetValue(item, out var overlaps))
        {
            return overlaps.AsReadOnly();
        }

        return [];
    }

    public void AddOverlap(ILayoutItem item, ILayoutItem overlap)
    {
        if (!_overlaps.TryGetValue(item, out var overlaps))
        {
            overlaps = [];
            _overlaps.Add(item, overlaps);
        }

        overlaps.Add(overlap);
    }

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
}
