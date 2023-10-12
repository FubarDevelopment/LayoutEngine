using System.Drawing;
using System.Windows.Forms;

namespace TestLayoutEngine;

public static class DrawingExtensions
{
    public static Point Add(this Point point, Point offset)
        => new(point.X + offset.X, point.Y + offset.Y);

    public static Rectangle Add(this Rectangle rectangle, Point offset)
        => rectangle with {X = rectangle.X + offset.X, Y = rectangle.Y + offset.Y};

    public static Rectangle Add(this Rectangle rectangle, Padding margin)
        => new(rectangle.X - margin.Left, rectangle.Y - margin.Top,
            rectangle.Right + margin.Right, rectangle.Bottom + margin.Bottom);

    public static Rectangle Subtract(this Rectangle rectangle, Padding padding)
        => new(rectangle.X + padding.Left, rectangle.Y + padding.Top,
            rectangle.Right - padding.Right, rectangle.Bottom - padding.Bottom);
}
