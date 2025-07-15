using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.LayoutCalculation;

internal sealed class ElementInfo(ILayoutItem item, int calculatedSize, double? factor = null)
{
    public ILayoutItem Item { get; } = item;
    public int CalculatedSize { get; } = calculatedSize;
    public double? Factor { get; } = factor;

    public int? MinSize { get; set; }
    public int? MaxSize { get; set; }

    public bool Final { get; set; } = factor == null;

    public ElementInfo EnsureLimits()
    {
        var result = CalculatedSize;
        if (MaxSize <= result)
        {
            return new ElementInfo(Item, MaxSize.Value, Factor)
            {
                MinSize = MinSize,
                MaxSize = MaxSize,
                Final = true,
            };
        }

        if (MinSize >= result)
        {
            return new ElementInfo(Item, MinSize.Value, Factor)
            {
                MinSize = MinSize,
                MaxSize = MaxSize,
                Final = true,
            };
        }

        return this;
    }
}
