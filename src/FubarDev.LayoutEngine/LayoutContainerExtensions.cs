using System;
using System.Drawing;
using System.Linq;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine;

public static class LayoutContainerExtensions
{
    public static Size GetMinimumSize(
        this ILayoutContainer container,
        ILayoutOverlapLookup? overlapLookup = null)
    {
        var result = container.LayoutEngine switch
        {
            IHorizontalLayoutEngine e => container.GetMinimumSize(overlapLookup, e.DefaultVerticalAlignment),
            IVerticalLayoutEngine e => container.GetMinimumSize(overlapLookup, e.DefaultHorizontalAlignment),
            _ => new Size(container.EnsureMinimumWidth(0), container.EnsureMinimumHeight(0)),
        };

        return result + container.Padding.Size;
    }

    private static Size GetMinimumSize(
        this ILayoutContainer container,
        ILayoutOverlapLookup? overlapLookup,
        VerticalAlignment alignment)
    {
        var result = container.GetUncollapsedChildren().GetMinimumSize(overlapLookup, alignment);

        if (overlapLookup == null)
        {
            return result;
        }

        var items = overlapLookup.GetOverlappingItemsFor(container);
        var overlappingItems = items.Where(x => x.Visibility != Visibility.Collapsed).ToList();
        var overlappingMinSize = overlappingItems.GetMinimumSize(overlapLookup, alignment);
        return new Size(
            Math.Max(result.Width, overlappingMinSize.Width),
            Math.Max(result.Height, overlappingMinSize.Height));
    }

    private static Size GetMinimumSize(
        this ILayoutContainer container,
        ILayoutOverlapLookup? overlapLookup,
        HorizontalAlignment alignment)
    {
        var result = container.GetUncollapsedChildren().GetMinimumSize(overlapLookup, alignment);

        if (overlapLookup == null)
        {
            return result;
        }

        var items = overlapLookup.GetOverlappingItemsFor(container);
        var overlappingItems = items.Where(x => x.Visibility != Visibility.Collapsed).ToList();
        var overlappingMinSize = overlappingItems.GetMinimumSize(overlapLookup, alignment);
        return new Size(
            Math.Max(result.Width, overlappingMinSize.Width),
            Math.Max(result.Height, overlappingMinSize.Height));
    }
}
