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

    public static LayoutBuilderContainer Pane(Control control, Orientation orientation)
    {
        if (control == null!)
        {
            throw new ArgumentNullException(nameof(control));
        }

        return Pane(control, orientation, Visibility.Collapsed);
    }

    public static LayoutBuilderItem Item(Control control, Visibility hiddenVisibility)
    {
        if (control == null!)
        {
            throw new ArgumentNullException(nameof(control));
        }

        return new LayoutBuilderItem(_ => new ControlLayoutItem(control, hiddenVisibility));
    }

    public static LayoutBuilderItem Item(Control control)
    {
        if (control == null!)
        {
            throw new ArgumentNullException(nameof(control));
        }

        return Item(control, Visibility.Collapsed);
    }

    public static LayoutBuilderItem Item()
    {
        return new LayoutBuilderItem(lookup => new LayoutPane(lookup));
    }
}
