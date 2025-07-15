using System;
using System.Windows;
using System.Windows.Controls;

using FubarDev.LayoutEngine.Elements;
using FubarDev.LayoutEngine.Engines;
using FubarDev.LayoutEngine.FrameworkElements;
using FubarDev.LayoutEngine.LayoutBuilder;

namespace FubarDev.LayoutEngine;

public static class WpfBuilderMethods
{
    public static LayoutBuilderRoot CreateRoot(FrameworkElement control, Orientation orientation)
    {
        if (control == null!)
        {
            throw new ArgumentNullException(nameof(control));
        }

        return new LayoutBuilderRoot(new FrameworkLayoutRoot(control)
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

    public static LayoutBuilderContainer Pane(FrameworkElement control, Orientation orientation)
    {
        if (control == null!)
        {
            throw new ArgumentNullException(nameof(control));
        }

        return new LayoutBuilderContainer(_ =>
            new FrameworkLayoutContainer(control)
            {
                LayoutEngine = orientation == Orientation.Horizontal
                    ? new HorizontalStackLayoutEngine()
                    : new VerticalStackLayoutEngine(),
            });
    }

    public static LayoutBuilderItem Item(FrameworkElement control)
    {
        if (control == null!)
        {
            throw new ArgumentNullException(nameof(control));
        }

        return new LayoutBuilderItem(_ => new FrameworkLayoutItem(control));
    }

    public static LayoutBuilderItem Item()
    {
        return new LayoutBuilderItem(lookup => new LayoutPane(lookup));
    }
}
