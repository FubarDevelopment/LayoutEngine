using System.Runtime.CompilerServices;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.AttachedProperties;

public static class AttachedVerticalAlignment
{
    private static readonly ConditionalWeakTable<ILayoutItem, PropertyInfo> AttachedProperties = new();

    public static VerticalAlignment? GetValue(ILayoutItem item)
    {
        return AttachedProperties.TryGetValue(item, out var value)
            ? value.Alignment
            : null;
    }

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
