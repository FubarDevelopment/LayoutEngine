using System.Collections.Generic;
using System.Diagnostics;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine;

/// <summary>
/// Provides debugging extension methods for layout items and containers.
/// </summary>
public static class DebugExtensions
{
    /// <summary>
    /// Dumps the layout information of the specified layout root to the debug output.
    /// </summary>
    /// <param name="item">The layout root to dump.</param>
    [Conditional("DEBUG")]
    public static void DumpLayout(this ILayoutRoot item)
    {
        Debug.WriteLine($"{item.Name}: {item.Bounds}, {item.GetMinimumClientSize()}");
        item.DumpLayout(string.Empty, item);
    }

    private static void DumpLayout(this ILayoutItem item, string prefix, ILayoutOverlapLookup? overlapLookup)
    {
        Debug.WriteLine($"{prefix}{item.Name}: {item.Bounds}");
        (item as ILayoutContainer)?.DumpLayout(prefix, overlapLookup);
    }

    private static void DumpLayout(this IEnumerable<ILayoutItem> items, string prefix, ILayoutOverlapLookup? overlapLookup)
    {
        foreach (var child in items)
        {
            DumpLayout(child, prefix, overlapLookup);
        }
    }

    private static void DumpLayout(this IEnumerable<ILayoutItem> items, string prefix, ILayoutOverlapLookup? overlapLookup, VerticalAlignment alignment)
    {
        foreach (var child in items)
        {
            DumpLayout(child, prefix, overlapLookup, alignment);
        }
    }

    private static void DumpLayout(this IEnumerable<ILayoutItem> items, string prefix, ILayoutOverlapLookup? overlapLookup, HorizontalAlignment alignment)
    {
        foreach (var child in items)
        {
            DumpLayout(child, prefix, overlapLookup, alignment);
        }
    }

    private static void DumpLayout(this ILayoutItem item, string prefix, ILayoutOverlapLookup? overlapLookup, VerticalAlignment alignment)
    {
        Debug.WriteLine($"{prefix}{item.Name}: {item.Bounds}, {item.DetermineMinimumSize(overlapLookup, alignment)}");
        (item as ILayoutContainer)?.DumpLayout(prefix, overlapLookup);
    }

    private static void DumpLayout(this ILayoutItem item, string prefix, ILayoutOverlapLookup? overlapLookup, HorizontalAlignment alignment)
    {
        Debug.WriteLine($"{prefix}{item.Name}: {item.Bounds}, {item.DetermineMinimumSize(overlapLookup, alignment)}");
        (item as ILayoutContainer)?.DumpLayout(prefix, overlapLookup);
    }

    private static void DumpLayout(this ILayoutContainer item, string prefix, ILayoutOverlapLookup? overlapLookup)
    {
        switch (item.LayoutEngine)
        {
            case IHorizontalLayoutEngine horizontalEngine:
                DumpLayout(item.GetChildren(), prefix + " ", overlapLookup, horizontalEngine.DefaultVerticalAlignment);
                break;
            case IVerticalLayoutEngine verticalEngine:
                DumpLayout(item.GetChildren(), prefix + " ", overlapLookup, verticalEngine.DefaultHorizontalAlignment);
                break;
            default:
                DumpLayout(item.GetChildren(), prefix + " ", overlapLookup);
                break;
        }
    }
}
