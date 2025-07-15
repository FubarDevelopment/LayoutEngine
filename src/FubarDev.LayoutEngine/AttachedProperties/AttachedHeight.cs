using System.Runtime.CompilerServices;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.AttachedProperties;

/// <summary>
/// Provides attached properties for storing the height of a layout item.
/// </summary>
public static class AttachedHeight
{
    private static readonly ConditionalWeakTable<ILayoutItem, AttachedSize> AttachedProperties = new();

    /// <summary>
    /// Gets the attached height value for the specified layout item.
    /// </summary>
    /// <param name="item">The layout item.</param>
    /// <returns>The attached height value.</returns>
    public static AttachedSize GetValue(ILayoutItem item)
    {
        return AttachedProperties.TryGetValue(item, out var value)
            ? value
            : AttachedSize.Unchanged;
    }

    /// <summary>
    /// Sets the attached height value for the specified layout item.
    /// </summary>
    /// <param name="item">The layout item.</param>
    /// <param name="value">The height value to set.</param>
    public static void SetValue(ILayoutItem item, AttachedSize value)
    {
        AttachedProperties.Remove(item);
        AttachedProperties.Add(item, value);
    }
}
