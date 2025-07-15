using System;
using System.Drawing;

using FubarDev.LayoutEngine.AttachedProperties;
using FubarDev.LayoutEngine.Elements;
using FubarDev.LayoutEngine.Engines;

namespace FubarDev.LayoutEngine.LayoutBuilder;

/// <summary>
/// Provides a builder for creating and configuring layout root elements.
/// </summary>
/// <param name="root">The layout root to build.</param>
public class LayoutBuilderRoot(ILayoutRoot root)
{
    private readonly ILayoutRoot _root = root;

    /// <summary>
    /// Builds and returns the layout root.
    /// </summary>
    /// <returns>The built layout root.</returns>
    public ILayoutRoot Build()
        => _root;

    /// <summary>
    /// Sets the name of the root.
    /// </summary>
    public LayoutBuilderRoot Name(string name)
    {
        _root.Name = name;
        return this;
    }

    /// <summary>
    /// Sets the minimum size of the root.
    /// </summary>
    public LayoutBuilderRoot MinimumSize(Size value)
    {
        ((ISettableMinimumSize)_root).MinimumSize = value;
        return this;
    }

    /// <summary>
    /// Sets the margin of the root.
    /// </summary>
    public LayoutBuilderRoot Margin(Margin value)
    {
        ((ISettableMargin)_root).Margin = value;
        return this;
    }

    /// <summary>
    /// Sets the padding of the root.
    /// </summary>
    public LayoutBuilderRoot Padding(Margin value)
    {
        ((ISettablePadding)_root).Padding = value;
        return this;
    }

    /// <summary>
    /// Sets the vertical stack layout engine for the root.
    /// </summary>
    public LayoutBuilderRoot VerticalStackLayout(HorizontalAlignment alignment)
    {
        _root.LayoutEngine = new VerticalStackLayoutEngine(alignment);
        return this;
    }

    /// <summary>
    /// Sets the horizontal stack layout engine for the root.
    /// </summary>
    public LayoutBuilderRoot HorizontalStackLayout(VerticalAlignment alignment)
    {
        _root.LayoutEngine = new HorizontalStackLayoutEngine(alignment);
        return this;
    }

    /// <summary>
    /// Sets the identifier for the root.
    /// </summary>
    public LayoutBuilderRoot Identifier(string identifier)
    {
        AttachedIdentifier.SetValue(_root, identifier);
        return this;
    }
    
    /// <summary>
    /// Sets the horizontal alignment for the root.
    /// </summary>
    public LayoutBuilderRoot HorizontalAlignment(HorizontalAlignment alignment)
    {
        _root.SetHorizontalAlignment(alignment);
        return this;
    }

    /// <summary>
    /// Sets the vertical alignment for the root.
    /// </summary>
    public LayoutBuilderRoot VerticalAlignment(VerticalAlignment alignment)
    {
        _root.SetVerticalAlignment(alignment);
        return this;
    }

    /// <summary>
    /// Sets the width for the root.
    /// </summary>
    public LayoutBuilderRoot Width(AttachedSize width)
    {
        _root.SetLayoutWidth(width);
        return this;
    }

    /// <summary>
    /// Sets the height for the root.
    /// </summary>
    public LayoutBuilderRoot Height(AttachedSize height)
    {
        _root.SetLayoutHeight(height);
        return this;
    }

    /// <summary>
    /// Adds an overlap for the specified identifier using a container.
    /// </summary>
    /// <param name="overlappedIdentifier">The identifier of the item to overlap.</param>
    /// <param name="overlapping">The overlapping container.</param>
    /// <returns>The updated root builder.</returns>
    public LayoutBuilderRoot AddOverlap(string overlappedIdentifier, LayoutBuilderContainer overlapping)
    {
        if (!AttachedIdentifier.TryFind(overlappedIdentifier, out var overlapped))
        {
            throw new InvalidOperationException($"Identifier '{overlappedIdentifier}' not found.");
        }

        _root.AddOverlap(overlapped, overlapping.Build(_root));
        return this;
    }

    /// <summary>
    /// Adds an overlap for the specified identifier using an item.
    /// </summary>
    /// <param name="overlappedIdentifier">The identifier of the item to overlap.</param>
    /// <param name="overlapping">The overlapping item.</param>
    /// <returns>The updated root builder.</returns>
    public LayoutBuilderRoot AddOverlap(string overlappedIdentifier, LayoutBuilderItem overlapping)
    {
        if (!AttachedIdentifier.TryFind(overlappedIdentifier, out var overlapped))
        {
            throw new InvalidOperationException($"Identifier '{overlappedIdentifier}' not found.");
        }

        _root.AddOverlap(overlapped, overlapping.Build(_root));
        return this;
    }

    /// <summary>
    /// Adds a root item to the root using the left shift operator.
    /// </summary>
    /// <param name="root">The root builder.</param>
    /// <param name="item">The root item to add.</param>
    /// <returns>The updated root builder.</returns>
    public static LayoutBuilderRoot operator <<(LayoutBuilderRoot root, LayoutBuilderRoot item)
    {
        root._root.Add(item.Build());
        return root;
    }

    /// <summary>
    /// Adds a container item to the root using the left shift operator.
    /// </summary>
    /// <param name="root">The root builder.</param>
    /// <param name="item">The container item to add.</param>
    /// <returns>The updated root builder.</returns>
    public static LayoutBuilderRoot operator <<(LayoutBuilderRoot root, LayoutBuilderContainer item)
    {
        root._root.Add(item.Build(root._root));
        return root;
    }

    /// <summary>
    /// Adds an item to the root using the left shift operator.
    /// </summary>
    /// <param name="root">The root builder.</param>
    /// <param name="item">The item to add.</param>
    /// <returns>The updated root builder.</returns>
    public static LayoutBuilderRoot operator <<(LayoutBuilderRoot root, LayoutBuilderItem item)
    {
        root._root.Add(item.Build(root._root));
        return root;
    }
}
