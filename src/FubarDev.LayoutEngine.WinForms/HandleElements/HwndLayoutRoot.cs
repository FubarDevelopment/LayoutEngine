using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.HandleElements;

public class HwndLayoutRoot : HwndLayoutContainer, ILayoutRoot
{
    private static readonly ILayoutItem[] EmptyItems = Array.Empty<ILayoutItem>();
    private readonly Dictionary<ILayoutItem, List<ILayoutItem>> _overlaps = new();

    public HwndLayoutRoot(IWin32Window window)
        : base(window)
    {
        RootWindow = window;
    }

    public Size ClientSize => GetClientRect(Handle).Size;
    public Rectangle DisplayRectangle => GetDisplayRectangle(Handle);

    public void AddOverlap(ILayoutItem item, ILayoutItem overlap)
    {
        if (!_overlaps.TryGetValue(item, out var overlaps))
        {
            overlaps = new();
            _overlaps.Add(item, overlaps);
        }

        overlaps.Add(overlap);

        SetRootWindow(RootWindow, overlap);
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

    protected override void SetRootWindow(ILayoutItem item)
    {
        SetRootWindow(RootWindow, item);
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

    private Rectangle GetDisplayRectangle(IntPtr handle)
    {
        var style = WindowsApi.GetWindowLongPtr(handle, WindowsApi.GWL_STYLE);
        var hasHorizontalScrollbar = (style.ToInt32() & WindowsApi.WS_HSCROLL) != 0;
        var hasVerticalScrollbar = (style.ToInt32() & WindowsApi.WS_VSCROLL) != 0;
        if (!hasHorizontalScrollbar && !hasVerticalScrollbar)
        {
            return GetClientRect(Handle);
        }

        var clientRect = !hasHorizontalScrollbar || !hasVerticalScrollbar
            ? GetClientRect(Handle)
            : Rectangle.Empty;

        int posX, width;
        if (hasHorizontalScrollbar)
        {
            var info = new WindowsApi.SCROLLINFO()
            {
                fMask = WindowsApi.SIF_RANGE | WindowsApi.SIF_POS,
            };

            if (!WindowsApi.GetScrollInfo(Handle, WindowsApi.SB_HORZ, info))
            {
                throw new Win32Exception();
            }

            posX = -info.nPos;
            width = info.nMax - info.nMin + 1;
        }
        else
        {
            posX = 0;
            width = clientRect.Width;
        }

        int posY, height;
        if (hasVerticalScrollbar)
        {
            var info = new WindowsApi.SCROLLINFO()
            {
                fMask = WindowsApi.SIF_RANGE | WindowsApi.SIF_POS,
            };

            if (!WindowsApi.GetScrollInfo(Handle, WindowsApi.SB_VERT, info))
            {
                throw new Win32Exception();
            }

            posY = -info.nPos;
            height = info.nMax - info.nMin + 1;
        }
        else
        {
            posY = 0;
            height = clientRect.Height;
        }

        return new Rectangle(posX, posY, width, height);
    }
}
