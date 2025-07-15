using System.Drawing;
using System.Runtime.CompilerServices;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.AttachedProperties;

/// <summary>
/// Provides attached properties for storing the calculated minimum size of a layout item.
/// </summary>
public static class AttachedCalculatedMinSize
{
    private static readonly ConditionalWeakTable<ILayoutItem, PropertyInfo> AttachedProperties = new();

    /// <summary>
    /// Gets the calculated minimum size for the specified layout item.
    /// </summary>
    /// <param name="item">The layout item.</param>
    /// <returns>The calculated minimum size, or null if not set.</returns>
    public static Size? GetValue(ILayoutItem item)
    {
        return AttachedProperties.TryGetValue(item, out var value)
            ? value.Size
            : null;
    }

    /// <summary>
    /// Sets the calculated minimum size for the specified layout item.
    /// </summary>
    /// <param name="item">The layout item.</param>
    /// <param name="value">The minimum size to set.</param>
    public static void SetValue(ILayoutItem item, Size? value)
    {
        AttachedProperties.Remove(item);
        if (value is { IsEmpty: false })
        {
            AttachedProperties.Add(item, new PropertyInfo(value.Value));
        }
    }

    private sealed class PropertyInfo(Size size)
    {
        public Size Size { get; } = size;
    }
}
