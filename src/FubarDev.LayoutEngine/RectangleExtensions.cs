using System.Drawing;

namespace FubarDev.LayoutEngine;

public static class RectangleExtensions
{
    public static Rectangle Expand(this Rectangle rect, Margin margin)
    {
        return new(
            rect.Left - margin.Left,
            rect.Top - margin.Top,
            rect.Width + margin.Horizontal,
            rect.Height + margin.Vertical);
    }

    public static Rectangle Shrink(this Rectangle rect, Margin padding)
    {
        return new(
            rect.Left + padding.Left,
            rect.Top + padding.Top,
            rect.Width - padding.Horizontal,
            rect.Height - padding.Vertical);
    }
}
