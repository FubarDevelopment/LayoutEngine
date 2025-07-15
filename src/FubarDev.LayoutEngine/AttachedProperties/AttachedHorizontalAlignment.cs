using System.Runtime.CompilerServices;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.AttachedProperties;

public static class AttachedHorizontalAlignment
{
    private static readonly ConditionalWeakTable<ILayoutItem, PropertyInfo> AttachedProperties = new();

    public static HorizontalAlignment? GetValue(ILayoutItem item)
    {
        return AttachedProperties.TryGetValue(item, out var value)
            ? value.Alignment
            : null;
    }

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
