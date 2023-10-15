using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.Test.TestElements;

public class TestRoot : TestContainer, ILayoutRoot
{
    private readonly Dictionary<ILayoutItem, List<ILayoutItem>> _overlaps = new();

    public TestRoot(ILayoutEngine layoutEngine)
        : base(layoutEngine)
    {
    }

    public Size ClientSize => Size;
    public Rectangle DisplayRectangle => new(Point.Empty, ClientSize);

    public IReadOnlyCollection<ILayoutItem> GetOverlappingItemsFor(ILayoutItem item)
    {
        if (_overlaps.TryGetValue(item, out var overlaps))
        {
            return overlaps.AsReadOnly();
        }

        return Array.Empty<ILayoutItem>();
    }

    public void AddOverlap(ILayoutItem item, ILayoutItem overlap)
    {
        if (!_overlaps.TryGetValue(item, out var overlaps))
        {
            overlaps = new List<ILayoutItem>();
            _overlaps.Add(item, overlaps);
        }

        overlaps.Add(overlap);
    }

    public void Layout()
    {
        this.TryLayout(Bounds.Shrink(Padding));
    }
}
