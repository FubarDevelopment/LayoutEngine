using System.Windows.Controls;
using System.Windows;
using System;
using System.Drawing;

using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace FubarDev.LayoutEngine;

/// <summary>
/// Provides extension methods for <see cref="FrameworkElement"/> to retrieve layout information.
/// </summary>
public static class FrameworkElementExtensions
{
    /// <summary>
    /// Gets the client size of the specified <see cref="FrameworkElement"/>.
    /// </summary>
    /// <param name="element">The framework element.</param>
    /// <returns>The client size as a <see cref="Size"/>.</returns>
    public static Size GetClientSize(this FrameworkElement element)
    {
        return element switch
        {
            ScrollViewer scrollViewer =>
                new Size((int)scrollViewer.ViewportWidth, (int)scrollViewer.ViewportHeight),
            ContentControl { Content: FrameworkElement contentElement } =>
                GetClientSize(contentElement),
            _ => new Size((int)element.ActualWidth, (int)element.ActualHeight),
        };
    }

    /// <summary>
    /// Gets the display rectangle of the specified <see cref="FrameworkElement"/>.
    /// </summary>
    /// <param name="element">The framework element.</param>
    /// <returns>The display rectangle as a <see cref="Rectangle"/>.</returns>
    public static Rectangle GetDisplayRectangle(this FrameworkElement element)
    {
        var result = element switch
        {
            ScrollViewer scrollViewer =>
                new Rectangle(
                    new Point(-(int)scrollViewer.HorizontalOffset, -(int)scrollViewer.VerticalOffset),
                    new Size(
                        Math.Max((int)scrollViewer.ViewportWidth, (int)scrollViewer.ExtentWidth),
                        Math.Max((int)scrollViewer.ViewportHeight, (int)scrollViewer.ExtentHeight))),
            ContentControl { Content: FrameworkElement contentElement } =>
                GetDisplayRectangle(contentElement),
            _ => new Rectangle(Point.Empty, new Size((int)element.ActualWidth, (int)element.ActualHeight)),
        };

        return result;
    }
}
