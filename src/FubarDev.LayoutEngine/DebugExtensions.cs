using System.Collections.Generic;
using System.Diagnostics;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine;

public static class DebugExtensions
{
    public static void DumpLayout(this ILayoutRoot item)
    {
        Debug.WriteLine($"{item.Name}: {item.Bounds}, {item.GetMinimumClientSize()}");
        switch (item.LayoutEngine)
        {
            case IHorizontalLayoutEngine horizontalEngine:
                DumpLayout(item.GetChildren(), " ", item, horizontalEngine.DefaultVerticalAlignment);
                break;
            case IVerticalLayoutEngine verticalEngine:
                DumpLayout(item.GetChildren(), " ", item, verticalEngine.DefaultHorizontalAlignment);
                break;
            default:
                DumpLayout(item.GetChildren(), " ", item);
                break;
        }
    }


    private static void DumpLayout(this ILayoutItem item, string prefix, ILayoutOverlapLookup? overlapLookup)
    {
        Debug.WriteLine($"{prefix}{item.Name}: {item.Bounds}");
        if (item is not ILayoutContainer container)
        {
            return;
        }

        switch (container.LayoutEngine)
        {
            case IHorizontalLayoutEngine horizontalEngine:
                DumpLayout(container.GetChildren(), prefix + " ", overlapLookup, horizontalEngine.DefaultVerticalAlignment);
                break;
            case IVerticalLayoutEngine verticalEngine:
                DumpLayout(container.GetChildren(), prefix + " ", overlapLookup, verticalEngine.DefaultHorizontalAlignment);
                break;
            default:
                DumpLayout(container.GetChildren(), prefix + " ", overlapLookup);
                break;
        }
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
        Debug.WriteLine($"{prefix}{item.Name}: {item.Bounds}, {item.GetMinimumSize(overlapLookup, alignment)}");
        if (item is not ILayoutContainer container)
        {
            return;
        }

        switch (container.LayoutEngine)
        {
            case IHorizontalLayoutEngine horizontalEngine:
                DumpLayout(container.GetChildren(), prefix + " ", overlapLookup, horizontalEngine.DefaultVerticalAlignment);
                break;
            case IVerticalLayoutEngine verticalEngine:
                DumpLayout(container.GetChildren(), prefix + " ", overlapLookup, verticalEngine.DefaultHorizontalAlignment);
                break;
            default:
                DumpLayout(container.GetChildren(), prefix + " ", overlapLookup);
                break;
        }
    }

    private static void DumpLayout(this ILayoutItem item, string prefix, ILayoutOverlapLookup? overlapLookup, HorizontalAlignment alignment)
    {
        Debug.WriteLine($"{prefix}{item.Name}: {item.Bounds}, {item.GetMinimumSize(overlapLookup, alignment)}");
        if (item is not ILayoutContainer container)
        {
            return;
        }

        switch (container.LayoutEngine)
        {
            case IHorizontalLayoutEngine horizontalEngine:
                DumpLayout(container.GetChildren(), prefix + " ", overlapLookup, horizontalEngine.DefaultVerticalAlignment);
                break;
            case IVerticalLayoutEngine verticalEngine:
                DumpLayout(container.GetChildren(), prefix + " ", overlapLookup, verticalEngine.DefaultHorizontalAlignment);
                break;
            default:
                DumpLayout(container.GetChildren(), prefix + " ", overlapLookup);
                break;
        }
    }
}
