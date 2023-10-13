using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.LayoutCalculation;

internal sealed class ElementInfo
{
    public ElementInfo(ILayoutItem item, int calculatedSize, double? factor = null)
    {
        Item = item;
        CalculatedSize = calculatedSize;
        Factor = factor;
        Final = factor == null;
    }

    public ILayoutItem Item { get; }
    public int CalculatedSize { get; }
    public double? Factor { get; }

    public int? MinSize { get; set; }
    public int? MaxSize { get; set; }

    public bool Final { get; set; }

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
