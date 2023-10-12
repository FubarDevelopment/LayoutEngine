using System.Windows.Forms;

namespace FubarDev.LayoutEngine;

internal static class Extensions
{
    public static Margin ToMargin(this Padding padding)
    {
        return new Margin(padding.Left, padding.Top, padding.Right, padding.Bottom);
    }
}
