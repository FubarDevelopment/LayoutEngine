using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using FubarDev.LayoutEngine.AttachedProperties;
using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine;

public static class LayoutItemExtensions
{
    public static T SetLayoutWidth<T>(this T control, AttachedSize size)
        where T : ILayoutItem
    {
        AttachedWidth.SetValue(control, size);
        return control;
    }

    public static T SetLayoutHeight<T>(this T control, AttachedSize size)
        where T : ILayoutItem
    {
        AttachedHeight.SetValue(control, size);
        return control;
    }

    public static T SetHorizontalAlignment<T>(this T control, HorizontalAlignment alignment)
        where T : ILayoutItem
    {
        AttachedHorizontalAlignment.SetValue(control, alignment);
        return control;
    }

    public static T SetVerticalAlignment<T>(this T control, VerticalAlignment alignment)
        where T : ILayoutItem
    {
        AttachedVerticalAlignment.SetValue(control, alignment);
        return control;
    }

    public static bool TryLayout(this ILayoutItem item, Rectangle bounds)
    {
        if (item is not ILayoutContainer { LayoutEngine: { } layoutEngine } subContainer)
        {
            return false;
        }

        layoutEngine.Layout(subContainer, bounds.Shrink(subContainer.Padding));
        return true;

    }

    public static Size GetMinimumSize(this ILayoutItem child, ILayoutOverlapLookup? overlapLookup, VerticalAlignment alignment)
    {
        var result =
            child is ILayoutContainer { LayoutEngine: not null } container
                ? container.GetMinimumSize(overlapLookup)
                : GetRawMinimumSize(child, alignment) + child.Padding.Size;
        if (overlapLookup == null)
        {
            return result;
        }

        var items = overlapLookup.GetOverlappingItemsFor(child);
        var overlappingItems = items.Where(x => x.Visibility != Visibility.Collapsed).ToList();
        var overlappingMinSize = GetMinimumSize(overlappingItems, overlapLookup, alignment);
        return new Size(
            Math.Max(result.Width, overlappingMinSize.Width),
            Math.Max(result.Height, overlappingMinSize.Height));
    }

    public static Size GetMinimumSize(this ILayoutItem child, ILayoutOverlapLookup? overlapLookup, HorizontalAlignment alignment)
    {
        var result =
            child is ILayoutContainer { LayoutEngine: not null } container
                ? container.GetMinimumSize(overlapLookup)
                : GetRawMinimumSize(child, alignment) + child.Padding.Size;
        if (overlapLookup == null)
        {
            return result;
        }

        var items = overlapLookup.GetOverlappingItemsFor(child);
        var overlappingItems = items.Where(x => x.Visibility != Visibility.Collapsed).ToList();
        var overlappingMinSize = GetMinimumSize(overlappingItems, overlapLookup, alignment);
        return new Size(
            Math.Max(result.Width, overlappingMinSize.Width),
            Math.Max(result.Height, overlappingMinSize.Height));
    }

    internal static IEnumerable<ILayoutItem> GetUncollapsedChildren(this ILayoutContainer container)
    {
        return container.GetChildren().Where(child => child.Visibility != Visibility.Collapsed);
    }

    internal static Size EnsureMaximumSize(this ILayoutItem item, Size size)
    {
        var maxSize = item.MaximumSize;
        var newWidth = maxSize.Width != 0 && maxSize.Width < size.Width
            ? maxSize.Width
            : size.Width;
        var newHeight = maxSize.Height != 0 && maxSize.Height < size.Height
            ? maxSize.Height
            : size.Height;
        if (size.Width != newWidth || size.Height != newHeight)
        {
            return new Size(newWidth, newHeight);
        }

        return size;
    }

    internal static Size EnsureMinimumSize(this ILayoutItem item, Size size)
    {
        var minSize = item.MinimumSize;
        if (minSize.Width > size.Width || minSize.Height > size.Height)
        {
            return new Size(Math.Max(size.Width, minSize.Width), Math.Max(size.Height, minSize.Height));
        }

        return size;
    }

    internal static Size GetMinimumSize(this IEnumerable<ILayoutItem> children, ILayoutOverlapLookup? overlapLookup, HorizontalAlignment alignment)
    {
        return children.Aggregate(new Size(), (acc, item) =>
        {
            var itemSize = item.GetMinimumSize(overlapLookup, alignment) + item.Margin.Size;
            return new Size(Math.Max(acc.Width, itemSize.Width), acc.Height + itemSize.Height);
        });
    }

    internal static Size GetMinimumSize(this IEnumerable<ILayoutItem> children, ILayoutOverlapLookup? overlapLookup, VerticalAlignment alignment)
    {
        return children.Aggregate(new Size(), (acc, item) =>
        {
            var itemSize = GetMinimumSize(item, overlapLookup, alignment) + item.Margin.Size;
            return new Size(acc.Width + itemSize.Width, Math.Max(acc.Height, itemSize.Height));
        });
    }

    internal static Size GetRawMinimumSize(this ILayoutItem item, VerticalAlignment alignment)
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

        return item.EnsureMinimumSize(new Size(minWidth, minHeight));
    }

    internal static Size GetRawMinimumSize(this ILayoutItem item, HorizontalAlignment alignment)
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

        return item.EnsureMinimumSize(new Size(minWidth, minHeight));
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
