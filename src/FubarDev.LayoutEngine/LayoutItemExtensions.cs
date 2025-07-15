using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using FubarDev.LayoutEngine.AttachedProperties;
using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine;

/// <summary>
/// Provides extension methods for layout items to set properties and calculate minimum sizes.
/// </summary>
public static class LayoutItemExtensions
{
    /// <summary>
    /// Sets the attached width for the layout item.
    /// </summary>
    /// <typeparam name="T">The type of the layout item.</typeparam>
    /// <param name="control">The layout item.</param>
    /// <param name="size">The attached size value.</param>
    /// <returns>The layout item.</returns>
    public static T SetLayoutWidth<T>(this T control, AttachedSize size)
        where T : ILayoutItem
    {
        AttachedWidth.SetValue(control, size);
        return control;
    }

    /// <summary>
    /// Sets the attached height for the layout item.
    /// </summary>
    /// <typeparam name="T">The type of the layout item.</typeparam>
    /// <param name="control">The layout item.</param>
    /// <param name="size">The attached size value.</param>
    /// <returns>The layout item.</returns>
    public static T SetLayoutHeight<T>(this T control, AttachedSize size)
        where T : ILayoutItem
    {
        AttachedHeight.SetValue(control, size);
        return control;
    }

    /// <summary>
    /// Sets the horizontal alignment for the layout item.
    /// </summary>
    /// <typeparam name="T">The type of the layout item.</typeparam>
    /// <param name="control">The layout item.</param>
    /// <param name="alignment">The horizontal alignment value.</param>
    /// <returns>The layout item.</returns>
    public static T SetHorizontalAlignment<T>(this T control, HorizontalAlignment alignment)
        where T : ILayoutItem
    {
        AttachedHorizontalAlignment.SetValue(control, alignment);
        return control;
    }

    /// <summary>
    /// Sets the vertical alignment for the layout item.
    /// </summary>
    /// <typeparam name="T">The type of the layout item.</typeparam>
    /// <param name="control">The layout item.</param>
    /// <param name="alignment">The vertical alignment value.</param>
    /// <returns>The layout item.</returns>
    public static T SetVerticalAlignment<T>(this T control, VerticalAlignment alignment)
        where T : ILayoutItem
    {
        AttachedVerticalAlignment.SetValue(control, alignment);
        return control;
    }

    /// <summary>
    /// Attempts to layout the item using the specified bounds.
    /// </summary>
    /// <param name="item">The layout item.</param>
    /// <param name="bounds">The bounds to use for layout.</param>
    /// <returns>True if layout was performed; otherwise, false.</returns>
    public static bool TryLayout(this ILayoutItem item, Rectangle bounds)
    {
        switch (item)
        {
            case ILayoutRoot root:
                root.Layout();
                return true;
            case ILayoutContainer { LayoutEngine: { } layoutEngine } subContainer:
                layoutEngine.Layout(subContainer, bounds.Shrink(subContainer.Padding));
                return true;
        }

        return false;
    }

    /// <summary>
    /// Gets the effective minimum size for the layout item.
    /// </summary>
    /// <param name="item">The layout item.</param>
    /// <returns>The effective minimum size.</returns>
    public static Size GetEffectiveMinimumSize(this ILayoutItem item)
    {
        var calculatedMinSize = AttachedCalculatedMinSize.GetValue(item);
        return calculatedMinSize ?? item.MinimumSize;
    }

    /// <summary>
    /// Determines the minimum size for the layout item using vertical alignment.
    /// </summary>
    /// <param name="child">The layout item.</param>
    /// <param name="overlapLookup">The lookup service for overlapping layout items.</param>
    /// <param name="alignment">The vertical alignment value.</param>
    /// <returns>The minimum size for the item.</returns>
    public static Size DetermineMinimumSize(this ILayoutItem child, ILayoutOverlapLookup? overlapLookup, VerticalAlignment alignment)
    {
        var result = child switch
        {
            ILayoutRoot root => root.DetermineMinimumSize(),
            ILayoutContainer { LayoutEngine: not null } container => container.DetermineMinimumSize(overlapLookup),
            _ => DetermineRawMinimumSize(child, alignment) + child.Padding.Size,
        };

        if (overlapLookup == null)
        {
            return result;
        }

        var items = overlapLookup.GetOverlappingItemsFor(child);
        var overlappingItems = items.Where(x => x.Visibility != Visibility.Collapsed).ToList();
        var overlappingMinSize = DetermineMinimumSize(overlappingItems, overlapLookup, alignment);
        return new Size(
            Math.Max(result.Width, overlappingMinSize.Width),
            Math.Max(result.Height, overlappingMinSize.Height));
    }

    /// <summary>
    /// Determines the minimum size for the layout item using horizontal alignment.
    /// </summary>
    /// <param name="child">The layout item.</param>
    /// <param name="overlapLookup">The lookup service for overlapping layout items.</param>
    /// <param name="alignment">The horizontal alignment value.</param>
    /// <returns>The minimum size for the item.</returns>
    public static Size DetermineMinimumSize(this ILayoutItem child, ILayoutOverlapLookup? overlapLookup, HorizontalAlignment alignment)
    {
        var result = child switch
        {
            ILayoutRoot root => root.DetermineMinimumSize(),
            ILayoutContainer { LayoutEngine: not null } container => container.DetermineMinimumSize(overlapLookup),
            _ => DetermineRawMinimumSize(child, alignment) + child.Padding.Size,
        };

        if (overlapLookup == null)
        {
            return result;
        }

        var items = overlapLookup.GetOverlappingItemsFor(child);
        var overlappingItems = items.Where(x => x.Visibility != Visibility.Collapsed).ToList();
        var overlappingMinSize = DetermineMinimumSize(overlappingItems, overlapLookup, alignment);
        return new Size(
            Math.Max(result.Width, overlappingMinSize.Width),
            Math.Max(result.Height, overlappingMinSize.Height));
    }

    /// <summary>
    /// Applies the minimum size for the layout item using vertical alignment.
    /// </summary>
    /// <param name="child">The layout item.</param>
    /// <param name="overlapLookup">The lookup service for overlapping layout items.</param>
    /// <param name="alignment">The vertical alignment value.</param>
    /// <returns>The applied minimum size for the item.</returns>
    public static Size ApplyMinimumSize(this ILayoutItem child, ILayoutOverlapLookup? overlapLookup, VerticalAlignment alignment)
    {
        var result = child switch
        {
            ILayoutRoot root => root.ApplyMinimumSize(),
            ILayoutContainer { LayoutEngine: not null } container => container.ApplyMinimumSize(overlapLookup),
            _ => DetermineRawMinimumSize(child, alignment) + child.Padding.Size,
        };

        if (overlapLookup != null)
        {
            var items = overlapLookup.GetOverlappingItemsFor(child);
            var overlappingItems = items.Where(x => x.Visibility != Visibility.Collapsed).ToList();
            var overlappingMinSize = ApplyMinimumSize(overlappingItems, overlapLookup, alignment);
            result = new Size(
                Math.Max(result.Width, overlappingMinSize.Width),
                Math.Max(result.Height, overlappingMinSize.Height));
        }

        child.SetCalculatedMinimumSize(result);

        return result;
    }

    /// <summary>
    /// Applies the minimum size for the layout item using horizontal alignment.
    /// </summary>
    /// <param name="child">The layout item.</param>
    /// <param name="overlapLookup">The lookup service for overlapping layout items.</param>
    /// <param name="alignment">The horizontal alignment value.</param>
    /// <returns>The applied minimum size for the item.</returns>
    public static Size ApplyMinimumSize(this ILayoutItem child, ILayoutOverlapLookup? overlapLookup, HorizontalAlignment alignment)
    {
        var result = child switch
        {
            ILayoutRoot root => root.ApplyMinimumSize(),
            ILayoutContainer { LayoutEngine: not null } container => container.ApplyMinimumSize(overlapLookup),
            _ => DetermineRawMinimumSize(child, alignment) + child.Padding.Size,
        };

        if (overlapLookup != null)
        {
            var items = overlapLookup.GetOverlappingItemsFor(child);
            var overlappingItems = items.Where(x => x.Visibility != Visibility.Collapsed).ToList();
            var overlappingMinSize = ApplyMinimumSize(overlappingItems, overlapLookup, alignment);
            result = new Size(
                Math.Max(result.Width, overlappingMinSize.Width),
                Math.Max(result.Height, overlappingMinSize.Height));
        }

        child.SetCalculatedMinimumSize(result);

        return result;
    }

    internal static void SetCalculatedMinimumSize(this ILayoutItem item, Size minimumSize)
    {
        AttachedCalculatedMinSize.SetValue(item, minimumSize);
    }

    internal static IEnumerable<ILayoutItem> GetUncollapsedChildren(this ILayoutContainer container)
    {
        return container.GetChildren().Where(child => child.Visibility != Visibility.Collapsed);
    }

    internal static Size EnsureMaximumSize(this ILayoutItem item, Size size, Size maximumSize)
    {
        var newWidth = maximumSize.Width != 0 && maximumSize.Width < size.Width
            ? maximumSize.Width
            : size.Width;
        var newHeight = maximumSize.Height != 0 && maximumSize.Height < size.Height
            ? maximumSize.Height
            : size.Height;
        if (size.Width != newWidth || size.Height != newHeight)
        {
            return new Size(newWidth, newHeight);
        }

        return size;
    }

    internal static Size EnsureMinimumSize(this ILayoutItem item, Size size, Size minimumSize)
    {
        if (minimumSize.Width > size.Width || minimumSize.Height > size.Height)
        {
            return new Size(Math.Max(size.Width, minimumSize.Width), Math.Max(size.Height, minimumSize.Height));
        }

        return size;
    }

    internal static Size ApplyMinimumSize(this IEnumerable<ILayoutItem> children, ILayoutOverlapLookup? overlapLookup, HorizontalAlignment alignment)
    {
        return children.Aggregate(new Size(), (acc, item) =>
        {
            var itemSize = item.ApplyMinimumSize(overlapLookup, alignment) + item.Margin.Size;
            return new Size(Math.Max(acc.Width, itemSize.Width), acc.Height + itemSize.Height);
        });
    }

    internal static Size ApplyMinimumSize(this IEnumerable<ILayoutItem> children, ILayoutOverlapLookup? overlapLookup, VerticalAlignment alignment)
    {
        return children.Aggregate(new Size(), (acc, item) =>
        {
            var itemSize = item.ApplyMinimumSize(overlapLookup, alignment) + item.Margin.Size;
            return new Size(acc.Width + itemSize.Width, Math.Max(acc.Height, itemSize.Height));
        });
    }

    internal static Size DetermineMinimumSize(this IEnumerable<ILayoutItem> children, ILayoutOverlapLookup? overlapLookup, HorizontalAlignment alignment)
    {
        return children.Aggregate(new Size(), (acc, item) =>
        {
            var itemSize = item.DetermineMinimumSize(overlapLookup, alignment) + item.Margin.Size;
            return new Size(Math.Max(acc.Width, itemSize.Width), acc.Height + itemSize.Height);
        });
    }

    internal static Size DetermineMinimumSize(this IEnumerable<ILayoutItem> children, ILayoutOverlapLookup? overlapLookup, VerticalAlignment alignment)
    {
        return children.Aggregate(new Size(), (acc, item) =>
        {
            var itemSize = item.DetermineMinimumSize(overlapLookup, alignment) + item.Margin.Size;
            return new Size(acc.Width + itemSize.Width, Math.Max(acc.Height, itemSize.Height));
        });
    }

    internal static Size DetermineRawMinimumSize(this ILayoutItem item, VerticalAlignment alignment)
    {
        var minWidth = AttachedWidth.GetValue(item) switch
        {
            AttachedSize.UnchangedSize => item.Width,
            AttachedSize.FixedSize s => s.Value,
            _ => 0,
        };

        var minHeight = (AttachedVerticalAlignment.GetValue(item) ?? alignment) switch
        {
            VerticalAlignment.Fill => 0,
            _ => item.Height,
        };

        return item.EnsureMinimumSize(new Size(minWidth, minHeight), item.MinimumSize);
    }

    internal static Size DetermineRawMinimumSize(this ILayoutItem item, HorizontalAlignment alignment)
    {
        var minWidth = (AttachedHorizontalAlignment.GetValue(item) ?? alignment) switch
        {
            HorizontalAlignment.Fill => 0,
            _ => item.Width,
        };

        var minHeight = AttachedHeight.GetValue(item) switch
        {
            AttachedSize.UnchangedSize => item.Height,
            AttachedSize.FixedSize s => s.Value,
            _ => 0,
        };

        return item.EnsureMinimumSize(new Size(minWidth, minHeight),  item.MinimumSize);
    }

    internal static Rectangle ApplyLayout(
        this ILayoutItem item,
        Rectangle bounds,
        HorizontalAlignment alignment)
    {
        return alignment switch
        {
            HorizontalAlignment.Fill => bounds,
            HorizontalAlignment.Left => new Rectangle(bounds.Location, new Size(item.Width, bounds.Height)),
            HorizontalAlignment.Center => bounds.Width < item.Width
                ? item.ApplyLayout(bounds, HorizontalAlignment.Left)
                : new Rectangle(
                    new Point(bounds.X + (bounds.Width - item.Width) / 2, bounds.Y),
                    new Size(item.Width, bounds.Height)),
            HorizontalAlignment.Right => bounds.Width < item.Width
                ? item.ApplyLayout(bounds, HorizontalAlignment.Left)
                : new Rectangle(
                    new Point(bounds.X + bounds.Width - item.Width, bounds.Y),
                    new Size(item.Width, bounds.Height)),
            _ => throw new NotSupportedException(),
        };
    }
    
    internal static Rectangle ApplyLayout(
        this ILayoutItem item,
        Rectangle bounds,
        VerticalAlignment alignment)
    {
        return alignment switch
        {
            VerticalAlignment.Fill => bounds,
            VerticalAlignment.Top => new Rectangle(bounds.Location, new Size(bounds.Width, item.Height)),
            VerticalAlignment.Center => bounds.Height < item.Height
                ? item.ApplyLayout(bounds, VerticalAlignment.Top)
                : new Rectangle(
                    new Point(bounds.X, bounds.Y + (bounds.Height - item.Height) / 2),
                    new Size(bounds.Width, item.Height)),
            VerticalAlignment.Bottom => bounds.Height < item.Height
                ? item.ApplyLayout(bounds, VerticalAlignment.Top)
                : new Rectangle(
                    new Point(bounds.X, bounds.Y + bounds.Height - item.Height),
                    new Size(bounds.Width, item.Height)),
            _ => throw new NotSupportedException(),
        };
    }
}
