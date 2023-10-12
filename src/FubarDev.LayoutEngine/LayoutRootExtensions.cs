using System.Diagnostics.Contracts;
using System.Drawing;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine;

public static class LayoutRootExtensions
{
    [Pure]
    public static Size GetMinimumClientSize(this ILayoutRoot root)
    {
        return root.GetMinimumSize(root) + root.Margin.Size;
    }
}
