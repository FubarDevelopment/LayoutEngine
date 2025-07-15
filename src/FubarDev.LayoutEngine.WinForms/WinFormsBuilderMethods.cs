using System;
using System.Windows.Forms;

using FubarDev.LayoutEngine.ControlElements;
using FubarDev.LayoutEngine.Elements;
using FubarDev.LayoutEngine.Engines;
using FubarDev.LayoutEngine.HandleElements;
using FubarDev.LayoutEngine.LayoutBuilder;

namespace FubarDev.LayoutEngine;

/// <summary>
/// Provides builder methods for creating layout elements for Windows Forms and Win32 windows.
/// </summary>
public static class WinFormsBuilderMethods
{
    /// <summary>
    /// Creates a layout root for a Windows Forms control with the specified orientation.
    /// </summary>
    /// <param name="control">The control to use as root.</param>
    /// <param name="orientation">The orientation for the layout engine.</param>
    /// <returns>The layout builder root.</returns>
    public static LayoutBuilderRoot CreateRoot(Control control, Orientation orientation)
    {
        if (control == null!)
        {
            throw new ArgumentNullException(nameof(control));
        }

        return new LayoutBuilderRoot(new ControlLayoutRoot(control)
        {
            LayoutEngine = orientation == Orientation.Horizontal
                ? new HorizontalStackLayoutEngine()
                : new VerticalStackLayoutEngine(),
        });
    }

    /// <summary>
    /// Creates a layout root for a Win32 window with the specified orientation.
    /// </summary>
    /// <param name="window">The window to use as root.</param>
    /// <param name="orientation">The orientation for the layout engine.</param>
    /// <returns>The layout builder root.</returns>
    public static LayoutBuilderRoot CreateRoot(IWin32Window window, Orientation orientation)
    {
        return new LayoutBuilderRoot(new HwndLayoutRoot(window)
        {
            LayoutEngine = orientation == Orientation.Horizontal
                ? new HorizontalStackLayoutEngine()
                : new VerticalStackLayoutEngine(),
        });
    }

    /// <summary>
    /// Creates a layout pane container with the specified orientation.
    /// </summary>
    /// <param name="orientation">The orientation for the layout engine.</param>
    /// <returns>The layout builder container.</returns>
    public static LayoutBuilderContainer Pane(Orientation orientation)
    {
        return new LayoutBuilderContainer(lookup => new LayoutPane(lookup)
        {
            LayoutEngine = orientation == Orientation.Horizontal
                ? new HorizontalStackLayoutEngine()
                : new VerticalStackLayoutEngine(),
        });
    }

    /// <summary>
    /// Creates a layout pane container with default orientation.
    /// </summary>
    /// <returns>The layout builder container.</returns>
    public static LayoutBuilderContainer Pane()
    {
        return new LayoutBuilderContainer(lookup => new LayoutPane(lookup));
    }

    /// <summary>
    /// Creates a layout pane container for a control with the specified orientation and visibility.
    /// </summary>
    /// <param name="control">The control to use.</param>
    /// <param name="orientation">The orientation for the layout engine.</param>
    /// <param name="hiddenVisibility">The visibility for hidden state.</param>
    /// <returns>The layout builder container.</returns>
    public static LayoutBuilderContainer Pane(Control control, Orientation orientation, Visibility hiddenVisibility)
    {
        if (control == null!)
        {
            throw new ArgumentNullException(nameof(control));
        }

        return new LayoutBuilderContainer(_ =>
            new ControlLayoutContainer(control, hiddenVisibility)
            {
                LayoutEngine = orientation == Orientation.Horizontal
                    ? new HorizontalStackLayoutEngine()
                    : new VerticalStackLayoutEngine(),
            });
    }

    /// <summary>
    /// Creates a layout pane container for a Win32 window with the specified orientation and visibility.
    /// </summary>
    /// <param name="window">The window to use.</param>
    /// <param name="orientation">The orientation for the layout engine.</param>
    /// <param name="hiddenVisibility">The visibility for hidden state.</param>
    /// <returns>The layout builder container.</returns>
    public static LayoutBuilderContainer Pane(IWin32Window window, Orientation orientation, Visibility hiddenVisibility)
    {
        return new LayoutBuilderContainer(_ =>
            new HwndLayoutContainer(window, hiddenVisibility)
            {
                LayoutEngine = orientation == Orientation.Horizontal
                    ? new HorizontalStackLayoutEngine()
                    : new VerticalStackLayoutEngine(),
            });
    }

    /// <summary>
    /// Creates a layout pane container for a control with the specified orientation.
    /// </summary>
    /// <param name="control">The control to use.</param>
    /// <param name="orientation">The orientation for the layout engine.</param>
    /// <returns>The layout builder container.</returns>
    public static LayoutBuilderContainer Pane(Control control, Orientation orientation)
    {
        if (control == null!)
        {
            throw new ArgumentNullException(nameof(control));
        }

        return Pane(control, orientation, Visibility.Collapsed);
    }

    /// <summary>
    /// Creates a layout pane container for a Win32 window with the specified orientation.
    /// </summary>
    /// <param name="window">The window to use.</param>
    /// <param name="orientation">The orientation for the layout engine.</param>
    /// <returns>The layout builder container.</returns>
    public static LayoutBuilderContainer Pane(IWin32Window window, Orientation orientation)
    {
        return Pane(window, orientation, Visibility.Hidden);
    }

    /// <summary>
    /// Creates a layout item for a control with the specified visibility.
    /// </summary>
    /// <param name="control">The control to use.</param>
    /// <param name="hiddenVisibility">The visibility for hidden state.</param>
    /// <returns>The layout builder item.</returns>
    public static LayoutBuilderItem Item(Control control, Visibility hiddenVisibility)
    {
        if (control == null!)
        {
            throw new ArgumentNullException(nameof(control));
        }

        return new LayoutBuilderItem(_ => new ControlLayoutItem(control, hiddenVisibility));
    }

    /// <summary>
    /// Creates a layout item for a Win32 window with the specified visibility.
    /// </summary>
    /// <param name="window">The window to use.</param>
    /// <param name="hiddenVisibility">The visibility for hidden state.</param>
    /// <returns>The layout builder item.</returns>
    public static LayoutBuilderItem Item(IWin32Window window, Visibility hiddenVisibility)
    {
        return new LayoutBuilderItem(_ => new HwndLayoutItem(window, hiddenVisibility));
    }

    /// <summary>
    /// Creates a layout item for a control with default visibility.
    /// </summary>
    /// <param name="control">The control to use.</param>
    /// <returns>The layout builder item.</returns>
    public static LayoutBuilderItem Item(Control control)
    {
        if (control == null!)
        {
            throw new ArgumentNullException(nameof(control));
        }

        return Item(control, Visibility.Collapsed);
    }

    /// <summary>
    /// Creates a layout item for a Win32 window with default visibility.
    /// </summary>
    /// <param name="window">The window to use.</param>
    /// <returns>The layout builder item.</returns>
    public static LayoutBuilderItem Item(IWin32Window window)
    {
        return Item(window, Visibility.Hidden);
    }

    /// <summary>
    /// Creates a layout item for a default pane.
    /// </summary>
    /// <returns>The layout builder item.</returns>
    public static LayoutBuilderItem Item()
    {
        return new LayoutBuilderItem(lookup => new LayoutPane(lookup));
    }
}
