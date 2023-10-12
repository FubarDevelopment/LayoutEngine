using System;
using System.Windows.Forms;

using FubarDev.LayoutEngine.Elements;
using FubarDev.LayoutEngine.Engines;
using FubarDev.LayoutEngine.LayoutBuilder;

namespace FubarDev.LayoutEngine;

public static class BuilderMethods
{
    public static LayoutBuilderRoot CreateRoot(Control control, Orientation orientation)
    {
        return new LayoutBuilderRoot(new ControlLayoutRoot(control)
        {
            LayoutEngine = orientation == Orientation.Horizontal
                ? new HorizontalStackLayoutEngine()
                : new VerticalStackLayoutEngine(),
        });
    }

    public static LayoutBuilderRoot CreateRoot(IntPtr handle, Orientation orientation)
    {
        return CreateRoot(Control.FromHandle(handle), orientation);
    }

    public static LayoutBuilderContainer Pane(Orientation orientation)
    {
        return new LayoutBuilderContainer(lookup => new LayoutPane(lookup)
        {
            LayoutEngine = orientation == Orientation.Horizontal
                ? new HorizontalStackLayoutEngine()
                : new VerticalStackLayoutEngine(),
        });
    }

    public static LayoutBuilderContainer Pane()
    {
        return new LayoutBuilderContainer(lookup => new LayoutPane(lookup));
    }

    public static LayoutBuilderContainer Pane(Control control, Orientation orientation, Visibility hiddenVisibility)
    {
        return new LayoutBuilderContainer(_ =>
            new ControlLayoutContainer(control, hiddenVisibility)
            {
                LayoutEngine = orientation == Orientation.Horizontal
                    ? new HorizontalStackLayoutEngine()
                    : new VerticalStackLayoutEngine(),
            });
    }

    public static LayoutBuilderContainer Pane(Control control, Orientation orientation)
    {
        return Pane(control, orientation, Visibility.Collapsed);
    }

    public static LayoutBuilderContainer Pane(IntPtr handle, Orientation orientation, Visibility hiddenVisibility)
    {
        return Pane(Control.FromHandle(handle), orientation, hiddenVisibility);
    }

    public static LayoutBuilderContainer Pane(IntPtr handle, Orientation orientation)
    {
        return Pane(handle, orientation, Visibility.Collapsed);
    }

    public static LayoutBuilderItem Item(Control control, Visibility hiddenVisibility)
    {
        return new LayoutBuilderItem(_ => new ControlLayoutItem(control, hiddenVisibility));
    }

    public static LayoutBuilderItem Item(Control control)
    {
        return Item(control, Visibility.Collapsed);
    }

    public static LayoutBuilderItem Item(IntPtr handle, Visibility hiddenVisibility)
    {
        return Item(Control.FromHandle(handle), hiddenVisibility);
    }

    public static LayoutBuilderItem Item(IntPtr handle)
    {
        return Item(handle, Visibility.Collapsed);
    }

    public static LayoutBuilderItem Item()
    {
        return new LayoutBuilderItem(lookup => new LayoutPane(lookup));
    }
}
