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

    public static Size ApplyMinimumSize(this ILayoutRoot root)
    {
        var result = root.ApplyMinimumSize(root) + root.Margin.Size;

        if (root is ISettableMinimumSize settableItem)
        {
            settableItem.MinimumSize = result;
        }

        return result;
    }
}
