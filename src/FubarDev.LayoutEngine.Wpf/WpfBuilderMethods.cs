using System;
using System.Windows;
using System.Windows.Controls;

using FubarDev.LayoutEngine.Elements;
using FubarDev.LayoutEngine.Engines;
using FubarDev.LayoutEngine.FrameworkElements;
using FubarDev.LayoutEngine.LayoutBuilder;

namespace FubarDev.LayoutEngine;

/// <summary>
/// Provides builder methods for creating layout elements for WPF framework elements.
/// </summary>
public static class WpfBuilderMethods
{
    /// <summary>
    /// Creates a layout root for a WPF framework element with the specified orientation.
    /// </summary>
    /// <param name="control">The framework element to use as root.</param>
    /// <param name="orientation">The orientation for the layout engine.</param>
    /// <returns>The layout builder root.</returns>
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
    /// Creates a layout pane container for a framework element with the specified orientation.
    /// </summary>
    /// <param name="control">The framework element to use.</param>
    /// <param name="orientation">The orientation for the layout engine.</param>
    /// <returns>The layout builder container.</returns>
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

    /// <summary>
    /// Creates a layout item for a framework element.
    /// </summary>
    /// <param name="control">The framework element to use.</param>
    /// <returns>The layout builder item.</returns>
    public static LayoutBuilderItem Item(FrameworkElement control)
    {
        if (control == null!)
        {
            throw new ArgumentNullException(nameof(control));
        }

        return new LayoutBuilderItem(_ => new FrameworkLayoutItem(control));
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
