using System.Runtime.CompilerServices;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.AttachedProperties;

/// <summary>
/// Provides attached properties for storing the vertical alignment of a layout item.
/// </summary>
public static class AttachedVerticalAlignment
{
    private static readonly ConditionalWeakTable<ILayoutItem, PropertyInfo> AttachedProperties = new();

    /// <summary>
    /// Gets the attached vertical alignment value for the specified layout item.
    /// </summary>
    /// <param name="item">The layout item.</param>
    /// <returns>The vertical alignment value, or null if not set.</returns>
    public static VerticalAlignment? GetValue(ILayoutItem item)
    {
        return AttachedProperties.TryGetValue(item, out var value)
            ? value.Alignment
            : null;
    }

    /// <summary>
    /// Sets the attached vertical alignment value for the specified layout item.
    /// </summary>
    /// <param name="item">The layout item.</param>
    /// <param name="value">The vertical alignment value to set.</param>
    public static void SetValue(ILayoutItem item, VerticalAlignment? value)
    {
        AttachedProperties.Remove(item);
        if (value != null)
            AttachedProperties.Add(item, new PropertyInfo(value.Value));
    }

    private sealed class PropertyInfo(VerticalAlignment alignment)
    {
        public VerticalAlignment Alignment { get; } = alignment;
    }
}
