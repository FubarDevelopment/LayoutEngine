using System.Runtime.CompilerServices;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.AttachedProperties;

/// <summary>
/// Provides attached properties for storing the horizontal alignment of a layout item.
/// </summary>
public static class AttachedHorizontalAlignment
{
    private static readonly ConditionalWeakTable<ILayoutItem, PropertyInfo> AttachedProperties = new();

    /// <summary>
    /// Gets the attached horizontal alignment value for the specified layout item.
    /// </summary>
    /// <param name="item">The layout item.</param>
    /// <returns>The horizontal alignment value, or null if not set.</returns>
    public static HorizontalAlignment? GetValue(ILayoutItem item)
    {
        return AttachedProperties.TryGetValue(item, out var value)
            ? value.Alignment
            : null;
    }

    /// <summary>
    /// Sets the attached horizontal alignment value for the specified layout item.
    /// </summary>
    /// <param name="item">The layout item.</param>
    /// <param name="value">The horizontal alignment value to set.</param>
    public static void SetValue(ILayoutItem item, HorizontalAlignment? value)
    {
        AttachedProperties.Remove(item);
        if (value != null)
            AttachedProperties.Add(item, new PropertyInfo(value.Value));
    }

    private sealed class PropertyInfo(HorizontalAlignment alignment)
    {
        public HorizontalAlignment Alignment { get; } = alignment;
    }
}
