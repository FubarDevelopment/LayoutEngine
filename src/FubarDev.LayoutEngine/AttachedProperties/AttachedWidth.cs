using System.Runtime.CompilerServices;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.AttachedProperties;

public static class AttachedWidth
{
    private static readonly ConditionalWeakTable<ILayoutItem, AttachedSize> AttachedProperties = new();

    public static AttachedSize GetValue(ILayoutItem item)
    {
        return AttachedProperties.TryGetValue(item, out var value)
            ? value
            : AttachedSize.Unchanged;
    }

    public static void SetValue(ILayoutItem item, AttachedSize value)
    {
        AttachedProperties.Remove(item);
        AttachedProperties.Add(item, value);
    }
}
