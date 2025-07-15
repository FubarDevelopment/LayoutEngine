using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.HandleElements;

/// <summary>
/// Represents a layout item for native Win32 windows.
/// </summary>
/// <param name="window">The window associated with this layout item.</param>
/// <param name="hiddenVisibility">The visibility state when the window is hidden.</param>
public class HwndLayoutItem(IWin32Window window, Visibility hiddenVisibility = Visibility.Collapsed)
    : ILayoutItem, ISettableMinimumSize, ISettableMargin, ISettablePadding
{
    private IntPtr? _handle;
    private Size? _minimumSize;
    private Rectangle? _bounds;
    private IWin32Window? _rootWindow;

    /// <summary>
    /// Gets the handle of the window associated with this layout item.
    /// </summary>
    protected IntPtr Handle => _handle ??= window.Handle;

    internal IWin32Window RootWindow
    {
        get => _rootWindow ?? throw new InvalidOperationException("No root window set yet");
        set => _rootWindow = value;
    }

    /// <inheritdoc />
    public string? Name { get; set; }
    
    /// <inheritdoc />
    public Point Location => Bounds.Location;
    
    /// <inheritdoc />
    public virtual Rectangle Bounds
    {
        get
        {
            return _bounds
                ??= RootWindow.Handle == IntPtr.Zero || RootWindow.Handle == Handle
                    ? GetBounds(Handle)
                    : ScreenToClient(RootWindow.Handle, GetBounds(Handle));
        }
    }

    /// <inheritdoc />
    public Size Size => Bounds.Size;
    
    /// <inheritdoc cref="ILayoutItem.MinimumSize" />
    public Size MinimumSize
    {
        get => _minimumSize ?? GetMinSize();
        set => _minimumSize = value;
    }

    /// <inheritdoc />
    public Size MaximumSize { get; } = new();
    
    /// <inheritdoc />
    public int Width => Bounds.Width;
    
    /// <inheritdoc />
    public int Height => Bounds.Height;
    
    /// <inheritdoc />
    public Visibility Visibility => GetVisibility(Handle, hiddenVisibility);

    /// <inheritdoc cref="ILayoutItem.Margin" />
    public Margin Margin { get; set; }

    /// <inheritdoc cref="ILayoutItem.Padding" />
    public Margin Padding { get; set; }

    /// <inheritdoc />
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

    /// <summary>
    /// Gets the bounds of the specified window handle.
    /// </summary>
    /// <param name="handle">The window handle.</param>
    /// <returns>The bounds as a <see cref="Rectangle"/>.</returns>
    protected static Rectangle GetBounds(IntPtr handle)
    {
        if (!WindowsApi.GetWindowRect(handle, out var rect))
        {
            throw new Win32Exception();
        }

        return rect;
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
