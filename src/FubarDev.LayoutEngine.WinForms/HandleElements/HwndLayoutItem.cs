using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.HandleElements;

public class HwndLayoutItem : ILayoutItem, ISettableMinimumSize, ISettableMargin, ISettablePadding
{
    private readonly IWin32Window _window;
    private readonly Visibility _hiddenVisibility;
    private IntPtr? _handle;
    private Size? _minimumSize;
    private Rectangle? _bounds;

    public HwndLayoutItem(IWin32Window window, Visibility hiddenVisibility = Visibility.Collapsed)
    {
        _window = window;
        _hiddenVisibility = hiddenVisibility;
    }

    protected IntPtr Handle => _handle ??= _window.Handle;
    internal IWin32Window? RootWindow { get; set; }

    public string? Name { get; set; }
    public Point Location => Bounds.Location;
    public Rectangle Bounds
    {
        get { return _bounds ??= ScreenToClient(RootWindow?.Handle ?? IntPtr.Zero, GetBounds(Handle)); }
    }

    public Size Size => Bounds.Size;
    public Size MinimumSize
    {
        get => _minimumSize ?? GetMinSize();
        set => _minimumSize = value;
    }

    public Size MaximumSize { get; } = new();
    public int Width => Bounds.Width;
    public int Height => Bounds.Height;
    public Visibility Visibility => GetVisibility(Handle, _hiddenVisibility);
    public Margin Margin { get; set; }
    public Margin Padding { get; set; }

    public void SetBounds(Rectangle bounds)
    {
        if (_bounds != null)
        {
            if (_bounds == bounds)
            {
                return;
            }

            _bounds = null;
        }

        if (!WindowsApi.SetWindowPos(
            Handle,
            IntPtr.Zero,
            bounds.X, bounds.Y, bounds.Width, bounds.Height,
            WindowsApi.SWP.NOZORDER | WindowsApi.SWP.NOACTIVATE | WindowsApi.SWP.NOOWNERZORDER))
        {
            throw new Win32Exception();
        }

        _bounds = bounds;
    }

    private static Rectangle ScreenToClient(IntPtr handle, Rectangle rect)
    {
        var pointLeftTop = rect.Location;
        if (!WindowsApi.ScreenToClient(handle, ref pointLeftTop))
        {
            throw new Win32Exception();
        }

        var pointRightBottom = new Point(rect.Right, rect.Bottom);
        if (!WindowsApi.ScreenToClient(handle, ref pointRightBottom))
        {
            throw new Win32Exception();
        }

        var width = pointRightBottom.X - pointLeftTop.X;
        var height = pointRightBottom.Y - pointLeftTop.Y;
        return new Rectangle(pointLeftTop, new Size(width, height));
    }

    private static Rectangle GetBounds(IntPtr handle)
    {
        if (!WindowsApi.GetWindowRect(handle, out var rect))
        {
            throw new Win32Exception();
        }

        return rect;
    }

    private unsafe WindowsApi.MINMAXINFO? GetMinMaxInfo()
    {
        var minMaxInfo = new WindowsApi.MINMAXINFO();
        if (WindowsApi.SendMessage(Handle, WindowsApi.WM.GETMINMAXINFO, UIntPtr.Zero, new IntPtr(&minMaxInfo)) != IntPtr.Zero)
        {
            return null;
        }

        return minMaxInfo;
    }

    private Size GetMinSize()
    {
        var minMaxInfo = GetMinMaxInfo();
        if (minMaxInfo == null)
        {
            return new Size();
        }

        return new Size(minMaxInfo.Value.ptMinTrackSize);
    }

    private static Visibility GetVisibility(IntPtr handle, Visibility hiddenVisibility)
    {
        if (WindowsApi.IsWindowVisible(handle))
        {
            return Visibility.Visible;
        }

        return hiddenVisibility;
    }
}
