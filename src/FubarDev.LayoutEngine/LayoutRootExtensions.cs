using System.Diagnostics.Contracts;
using System.Drawing;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine;

public static class LayoutRootExtensions
{
    /// <summary>
    /// Calculates the minimum size of the layout.
    /// </summary>
    /// <param name="root">The layout to calculate the minimum size for.</param>
    /// <returns>The minimum size for the layout (excluding its margin)</returns>
    [Pure]
    public static Size GetMinimumClientSize(this ILayoutRoot root)
    {
        return root.GetMinimumSize(root);
    }

    /// <summary>
    /// Calculates and applies the minimum size of the layout.
    /// </summary>
    /// <param name="root">The root of the layout to calculate the minimum size for and apply the calculated value to.</param>
    /// <returns>The calculated minimum size (excluding its margin).</returns>
    public static Size ApplyMinimumSize(this ILayoutRoot root)
    {
        var result = root.ApplyMinimumSize(root);

        if (root is ISettableMinimumSize settableItem)
        {
            settableItem.MinimumSize = result;
        }

        return result;
    }
}
