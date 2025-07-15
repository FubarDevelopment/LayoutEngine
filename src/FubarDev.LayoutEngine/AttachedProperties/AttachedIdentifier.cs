using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.AttachedProperties;

/// <summary>
/// Provides attached properties for storing a unique identifier for a layout item.
/// </summary>
public static class AttachedIdentifier
{
    private static readonly ConditionalWeakTable<ILayoutItem, string> AttachedProperties = new();
    private static readonly Dictionary<string, WeakReference<ILayoutItem>> Identifiers = new();

    /// <summary>
    /// Gets the attached identifier value for the specified layout item.
    /// </summary>
    /// <param name="item">The layout item.</param>
    /// <returns>The identifier value, or null if not set.</returns>
    public static string? GetValue(ILayoutItem item)
    {
        return AttachedProperties.TryGetValue(item, out var value)
            ? value
            : null;
    }

    /// <summary>
    /// Sets the attached identifier value for the specified layout item.
    /// </summary>
    /// <param name="item">The layout item.</param>
    /// <param name="value">The identifier value to set.</param>
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

    /// <summary>
    /// Tries to find a layout item by its attached identifier.
    /// </summary>
    /// <param name="identifier">The identifier to search for.</param>
    /// <param name="item">The found layout item, or null if not found.</param>
    /// <returns>True if the item was found; otherwise, false.</returns>
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
