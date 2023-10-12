using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.AttachedProperties;

public static class AttachedIdentifier
{
    private static readonly ConditionalWeakTable<ILayoutItem, string> AttachedProperties = new();
    private static readonly Dictionary<string, WeakReference<ILayoutItem>> Identifiers = new();

    public static string? GetValue(ILayoutItem item)
    {
        return AttachedProperties.TryGetValue(item, out var value)
            ? value
            : null;
    }

    public static void SetValue(ILayoutItem item, string? value)
    {
        if (AttachedProperties.TryGetValue(item, out var oldValue))
        {
            Identifiers.Remove(oldValue);
            AttachedProperties.Remove(item);
        }

        if (value == null)
        {
            return;
        }

        if (Identifiers.ContainsKey(value))
        {
            throw new InvalidOperationException($"Identifier '{value}' is already in use.");
        }

        Identifiers.Add(value, new WeakReference<ILayoutItem>(item));
        AttachedProperties.Add(item, value);
    }

    public static bool TryFind(string identifier, [NotNullWhen(true)] out ILayoutItem? item)
    {
        if (!Identifiers.TryGetValue(identifier, out var weakReference))
        {
            item = null;
            return false;
        }

        if (!weakReference.TryGetTarget(out item))
        {
            item = null;
            return false;
        }

        return true;
    }
}
