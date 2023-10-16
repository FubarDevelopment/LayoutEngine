using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

using FubarDev.LayoutEngine.Elements;

using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace FubarDev.LayoutEngine.FrameworkElements;

public class FrameworkLayoutRoot : FrameworkLayoutContainer, ILayoutRoot
{
    private static readonly ILayoutItem[] EmptyItems = Array.Empty<ILayoutItem>();
    private readonly Dictionary<ILayoutItem, List<ILayoutItem>> _overlaps = new();

    public FrameworkLayoutRoot(FrameworkElement element)
        : base(element)
    {
    }

    public Size ClientSize => GetClientSize(Element);

    public Rectangle DisplayRectangle => GetDisplayRectangle(Element);

    public IReadOnlyCollection<ILayoutItem> GetOverlappingItemsFor(ILayoutItem item)
    {
        if (_overlaps.TryGetValue(item, out var overlaps))
        {
            return overlaps;
        }

        return EmptyItems;
    }

    public void AddOverlap(ILayoutItem item, ILayoutItem overlap)
    {
        if (!_overlaps.TryGetValue(item, out var overlaps))
        {
            overlaps = new();
            _overlaps.Add(item, overlaps);
        }

        overlaps.Add(overlap);
    }

    public void Layout()
    {
        if (LayoutEngine == null)
        {
            return;
        }

        var bounds = DisplayRectangle.Shrink(Padding);
        LayoutEngine.Layout(this, bounds);

        foreach (var overlapItem in _overlaps)
        {
            var item = overlapItem.Key;
            var overlaps = overlapItem.Value;
            foreach (var overlap in overlaps)
            {
                LayoutOverlap(item, overlap);
            }
        }
    }

    private static void LayoutOverlap(
        ILayoutItem item,
        ILayoutItem overlap)
    {
        overlap.SetBounds(item.Bounds);
        if (overlap is ILayoutContainer { LayoutEngine: { } layoutEngine } container)
        {
            layoutEngine.Layout(container, item.Bounds);
        }
    }

    private static Size GetClientSize(FrameworkElement element)
    {
        return element switch
        {
            ScrollViewer scrollViewer =>
                new Size((int)scrollViewer.ViewportWidth, (int)scrollViewer.ViewportHeight),
            ContentControl { Content: FrameworkElement contentElement } =>
                GetClientSize(contentElement),
            _ => new Size((int)element.ActualWidth, (int)element.ActualHeight),
        };
    }

    private static Rectangle GetDisplayRectangle(FrameworkElement element)
    {
        var result = element switch
        {
            ScrollViewer scrollViewer =>
                new Rectangle(
                    new Point(-(int)scrollViewer.HorizontalOffset, -(int)scrollViewer.VerticalOffset),
                    new Size(
                        Math.Max((int)scrollViewer.ViewportWidth, (int)scrollViewer.ExtentWidth),
                        Math.Max((int)scrollViewer.ViewportHeight, (int)scrollViewer.ExtentHeight))),
            ContentControl { Content: FrameworkElement contentElement } =>
                GetDisplayRectangle(contentElement),
            _ => new Rectangle(Point.Empty, new Size((int)element.ActualWidth, (int)element.ActualHeight)),
        };

        return result;
    }
}
