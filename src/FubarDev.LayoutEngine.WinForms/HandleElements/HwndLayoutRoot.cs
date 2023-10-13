using System.Collections.Generic;
using System.Drawing;
using System;
using System.ComponentModel;

using FubarDev.LayoutEngine.Elements;
using System.Windows.Forms;

namespace FubarDev.LayoutEngine.HandleElements;

public class HwndLayoutRoot : HwndLayoutContainer, ILayoutRoot
{
    private static readonly ILayoutItem[] EmptyItems = Array.Empty<ILayoutItem>();
    private readonly Dictionary<ILayoutItem, List<ILayoutItem>> _overlaps = new();
    private readonly IWin32Window _rootWindow;

    public HwndLayoutRoot(IWin32Window window)
        : base(window)
    {
        RootWindow = _rootWindow = window;
    }

    public Size ClientSize => GetClientRect(Handle).Size;

    public void AddOverlap(ILayoutItem item, ILayoutItem overlap)
    {
        if (!_overlaps.TryGetValue(item, out var overlaps))
        {
            overlaps = new();
            _overlaps.Add(item, overlaps);
        }

        overlaps.Add(overlap);

        SetRootWindow(_rootWindow, overlap);
    }

    public IReadOnlyCollection<ILayoutItem> GetOverlappingItemsFor(ILayoutItem item)
    {
        if (_overlaps.TryGetValue(item, out var overlaps))
        {
            return overlaps;
        }

        return EmptyItems;
    }

    public void Layout()
    {
        if (LayoutEngine == null)
        {
            return;
        }

        var bounds = new Rectangle(new Point(), ClientSize).Shrink(Margin).Shrink(Padding);
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

    private static Rectangle GetClientRect(IntPtr handle)
    {
        if (!WindowsApi.GetClientRect(handle, out var rect))
        {
            throw new Win32Exception();
        }

        return rect;
    }
}
