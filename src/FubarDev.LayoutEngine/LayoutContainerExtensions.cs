using System;
using System.Drawing;
using System.Linq;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine;

public static class LayoutContainerExtensions
{
    /// <summary>
    /// Gets the required minimum size for a container including its padding.
    /// </summary>
    /// <param name="container">The container to calculate the minimum size for.</param>
    /// <param name="overlapLookup">The lookup service for overlapping layout items.</param>
    /// <returns>The minimum size of the container including its padding.</returns>
    public static Size DetermineMinimumSize(
        this ILayoutContainer container,
        ILayoutOverlapLookup? overlapLookup = null)
    {
        var result = container.LayoutEngine switch
        {
            IHorizontalLayoutEngine e => container.DetermineMinimumSize(overlapLookup, e.DefaultVerticalAlignment),
            IVerticalLayoutEngine e => container.DetermineMinimumSize(overlapLookup, e.DefaultHorizontalAlignment),
            _ => container.EnsureMinimumSize(new Size(), container.MinimumSize) + container.Padding.Size,
        };

        return result;
    }

    private static Size DetermineMinimumSize(
        this ILayoutContainer container,
        ILayoutOverlapLookup? overlapLookup,
        VerticalAlignment alignment)
    {
        var result = container.GetUncollapsedChildren().DetermineMinimumSize(overlapLookup, alignment);
        if (overlapLookup == null)
        {
            result += container.Padding.Size;
        }
        else
        {
            var items = overlapLookup.GetOverlappingItemsFor(container);
            var overlappingItems = items.Where(x => x.Visibility != Visibility.Collapsed).ToList();
            var overlappingMinSize = overlappingItems.DetermineMinimumSize(overlapLookup, alignment);
            result = new Size(
                Math.Max(result.Width, overlappingMinSize.Width),
                Math.Max(result.Height, overlappingMinSize.Height));
            result += container.Padding.Size;
        }

        var containerMinimumSize = container.MinimumSize;
        result = new Size(
            Math.Max(result.Width, containerMinimumSize.Width),
            Math.Max(result.Height, containerMinimumSize.Height));

        return result;
    }

    private static Size DetermineMinimumSize(
        this ILayoutContainer container,
        ILayoutOverlapLookup? overlapLookup,
        HorizontalAlignment alignment)
    {
        var result = container.GetUncollapsedChildren().DetermineMinimumSize(overlapLookup, alignment);
        if (overlapLookup == null)
        {
            result += container.Padding.Size;
        }
        else
        {
            var items = overlapLookup.GetOverlappingItemsFor(container);
            var overlappingItems = items.Where(x => x.Visibility != Visibility.Collapsed).ToList();
            var overlappingMinSize = overlappingItems.DetermineMinimumSize(overlapLookup, alignment);
            result = new Size(
                Math.Max(result.Width, overlappingMinSize.Width),
                Math.Max(result.Height, overlappingMinSize.Height));
            result += container.Padding.Size;
        }

        var containerMinimumSize = container.MinimumSize;
        result = new Size(
            Math.Max(result.Width, containerMinimumSize.Width),
            Math.Max(result.Height, containerMinimumSize.Height));

        return result;
    }

    public static Size ApplyMinimumSize(
        this ILayoutContainer container,
        ILayoutOverlapLookup? overlapLookup = null)
    {
        var result = container.LayoutEngine switch
        {
            IHorizontalLayoutEngine e => container.ApplyMinimumSize(overlapLookup, e.DefaultVerticalAlignment),
            IVerticalLayoutEngine e => container.ApplyMinimumSize(overlapLookup, e.DefaultHorizontalAlignment),
            _ => container.EnsureMinimumSize(new Size(), container.MinimumSize) + container.Padding.Size,
        };

        container.SetCalculatedMinimumSize(result);

        return result;
    }

    private static Size ApplyMinimumSize(
        this ILayoutContainer container,
        ILayoutOverlapLookup? overlapLookup,
        VerticalAlignment alignment)
    {
        var result = container.GetUncollapsedChildren().ApplyMinimumSize(overlapLookup, alignment);
        if (overlapLookup == null)
        {
            result += container.Padding.Size;
        }
        else
        {
            var items = overlapLookup.GetOverlappingItemsFor(container);
            var overlappingItems = items.Where(x => x.Visibility != Visibility.Collapsed).ToList();
            var overlappingMinSize = overlappingItems.ApplyMinimumSize(overlapLookup, alignment);
            result = new Size(
                Math.Max(result.Width, overlappingMinSize.Width),
                Math.Max(result.Height, overlappingMinSize.Height));
            result += container.Padding.Size;
        }

        var containerMinimumSize = container.MinimumSize;
        result = new Size(
            Math.Max(result.Width, containerMinimumSize.Width),
            Math.Max(result.Height, containerMinimumSize.Height));

        return result;
    }

    private static Size ApplyMinimumSize(
        this ILayoutContainer container,
        ILayoutOverlapLookup? overlapLookup,
        HorizontalAlignment alignment)
    {
        var result = container.GetUncollapsedChildren().ApplyMinimumSize(overlapLookup, alignment);
        if (overlapLookup == null)
        {
            result += container.Padding.Size;
        }
        else
        {
            var items = overlapLookup.GetOverlappingItemsFor(container);
            var overlappingItems = items.Where(x => x.Visibility != Visibility.Collapsed).ToList();
            var overlappingMinSize = overlappingItems.ApplyMinimumSize(overlapLookup, alignment);
            result = new Size(
                Math.Max(result.Width, overlappingMinSize.Width),
                Math.Max(result.Height, overlappingMinSize.Height));
            result += container.Padding.Size;
        }

        var containerMinimumSize = container.MinimumSize;
        result = new Size(
            Math.Max(result.Width, containerMinimumSize.Width),
            Math.Max(result.Height, containerMinimumSize.Height));

        return result;
    }
}
