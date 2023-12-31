﻿using System;
using System.Windows.Forms;

using FubarDev.LayoutEngine.ControlElements;
using FubarDev.LayoutEngine.Elements;
using FubarDev.LayoutEngine.Engines;
using FubarDev.LayoutEngine.HandleElements;
using FubarDev.LayoutEngine.LayoutBuilder;

namespace FubarDev.LayoutEngine;

public static class WinFormsBuilderMethods
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

    public static LayoutBuilderRoot CreateRoot(IWin32Window window, Orientation orientation)
    {
        return new LayoutBuilderRoot(new HwndLayoutRoot(window)
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

    public static LayoutBuilderContainer Pane(Control control, Orientation orientation)
    {
        if (control == null!)
        {
            throw new ArgumentNullException(nameof(control));
        }

        return Pane(control, orientation, Visibility.Collapsed);
    }

    public static LayoutBuilderContainer Pane(IWin32Window window, Orientation orientation)
    {
        return Pane(window, orientation, Visibility.Hidden);
    }

    public static LayoutBuilderItem Item(Control control, Visibility hiddenVisibility)
    {
        if (control == null!)
        {
            throw new ArgumentNullException(nameof(control));
        }

        return new LayoutBuilderItem(_ => new ControlLayoutItem(control, hiddenVisibility));
    }

    public static LayoutBuilderItem Item(IWin32Window window, Visibility hiddenVisibility)
    {
        return new LayoutBuilderItem(_ => new HwndLayoutItem(window, hiddenVisibility));
    }

    public static LayoutBuilderItem Item(Control control)
    {
        if (control == null!)
        {
            throw new ArgumentNullException(nameof(control));
        }

        return Item(control, Visibility.Collapsed);
    }

    public static LayoutBuilderItem Item(IWin32Window window)
    {
        return Item(window, Visibility.Hidden);
    }

    public static LayoutBuilderItem Item()
    {
        return new LayoutBuilderItem(lookup => new LayoutPane(lookup));
    }
}
