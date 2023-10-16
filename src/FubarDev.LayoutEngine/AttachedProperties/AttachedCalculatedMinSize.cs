using System.Drawing;
using System.Runtime.CompilerServices;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.AttachedProperties;

public static class AttachedCalculatedMinSize
{
    private static readonly ConditionalWeakTable<ILayoutItem, PropertyInfo> AttachedProperties = new();

    public static Size? GetValue(ILayoutItem item)
    {
        return AttachedProperties.TryGetValue(item, out var value)
            ? value.Size
            : null;
    }

    public static void SetValue(ILayoutItem item, Size? value)
    {
        AttachedProperties.Remove(item);
        if (value is { IsEmpty: false })
        {
            AttachedProperties.Add(item, new PropertyInfo(value.Value));
        }
    }

    private sealed class PropertyInfo
    {
        public PropertyInfo(Size size)
        {
            Size = size;
        }

        public Size Size { get; }
    }
}
