using System.Drawing;

namespace FubarDev.LayoutEngine;

/// <summary>
/// Provides extension methods for working with <see cref="Rectangle"/> and <see cref="Margin"/>.
/// </summary>
public static class RectangleExtensions
{
    /// <summary>
    /// Expands the specified rectangle by the given margin.
    /// </summary>
    /// <param name="rect">The rectangle to expand.</param>
    /// <param name="margin">The margin to apply.</param>
    /// <returns>The expanded rectangle.</returns>
    public static Rectangle Expand(this Rectangle rect, Margin margin)
    {
        return new(
            rect.Left - margin.Left,
            rect.Top - margin.Top,
            rect.Width + margin.Horizontal,
            rect.Height + margin.Vertical);
    }

    /// <summary>
    /// Shrinks the specified rectangle by the given padding.
    /// </summary>
    /// <param name="rect">The rectangle to shrink.</param>
    /// <param name="padding">The padding to apply.</param>
    /// <returns>The shrunken rectangle.</returns>
    public static Rectangle Shrink(this Rectangle rect, Margin padding)
    {
        return new(
            rect.Left + padding.Left,
            rect.Top + padding.Top,
            rect.Width - padding.Horizontal,
            rect.Height - padding.Vertical);
    }
}
