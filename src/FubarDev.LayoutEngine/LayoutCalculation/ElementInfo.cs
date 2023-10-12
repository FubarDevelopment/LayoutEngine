using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.LayoutCalculation;

internal record ElementInfo(ILayoutItem Item, int CalculatedSize, double? Factor = null)
{
    public int? MinSize { get; init; }
    public int? MaxSize { get; init; }

    public bool Final { get; init; } = Factor == null;

    public ElementInfo EnsureLimits()
    {
        if (Final)
        {
            return this;
        }

        var result = CalculatedSize;
        if (MaxSize <= result)
        {
            return this with {CalculatedSize = MaxSize.Value, Final = true};
        }

        if (MinSize >= result)
        {
            return this with {CalculatedSize = MinSize.Value, Final = true};
        }

        return this;
    }
}
