using System;
using System.Drawing;

using FubarDev.LayoutEngine.AttachedProperties;
using FubarDev.LayoutEngine.Elements;
using FubarDev.LayoutEngine.Engines;

namespace FubarDev.LayoutEngine.LayoutBuilder;

public class LayoutBuilderRoot
{
    private readonly ILayoutRoot _root;

    public LayoutBuilderRoot(ILayoutRoot root)
    {
        _root = root;
    }

    public ILayoutRoot Build()
        => _root;

    public LayoutBuilderRoot Name(string name)
    {
        _root.Name = name;
        return this;
    }

    public LayoutBuilderRoot MinimumSize(Size value)
    {
        ((ISettableMinimumSize)_root).MinimumSize = value;
        return this;
    }

    public LayoutBuilderRoot Margin(Margin value)
    {
        ((ISettableMargin)_root).Margin = value;
        return this;
    }

    public LayoutBuilderRoot Padding(Margin value)
    {
        ((ISettablePadding)_root).Padding = value;
        return this;
    }

    public LayoutBuilderRoot VerticalStackLayout(HorizontalAlignment alignment)
    {
        _root.LayoutEngine = new VerticalStackLayoutEngine(alignment);
        return this;
    }

    public LayoutBuilderRoot HorizontalStackLayout(VerticalAlignment alignment)
    {
        _root.LayoutEngine = new HorizontalStackLayoutEngine(alignment);
        return this;
    }

    public LayoutBuilderRoot Identifier(string identifier)
    {
        AttachedIdentifier.SetValue(_root, identifier);
        return this;
    }
    
    public LayoutBuilderRoot HorizontalAlignment(HorizontalAlignment alignment)
    {
        _root.SetHorizontalAlignment(alignment);
        return this;
    }

    public LayoutBuilderRoot VerticalAlignment(VerticalAlignment alignment)
    {
        _root.SetVerticalAlignment(alignment);
        return this;
    }

    public LayoutBuilderRoot Width(AttachedSize width)
    {
        _root.SetLayoutWidth(width);
        return this;
    }

    public LayoutBuilderRoot Height(AttachedSize height)
    {
        _root.SetLayoutHeight(height);
        return this;
    }

    public LayoutBuilderRoot AddOverlap(string overlappedIdentifier, LayoutBuilderContainer overlapping)
    {
        if (!AttachedIdentifier.TryFind(overlappedIdentifier, out var overlapped))
        {
            throw new InvalidOperationException($"Identifier '{overlappedIdentifier}' not found.");
        }

        _root.AddOverlap(overlapped, overlapping.Build(_root));
        return this;
    }

    public LayoutBuilderRoot AddOverlap(string overlappedIdentifier, LayoutBuilderItem overlapping)
    {
        if (!AttachedIdentifier.TryFind(overlappedIdentifier, out var overlapped))
        {
            throw new InvalidOperationException($"Identifier '{overlappedIdentifier}' not found.");
        }

        _root.AddOverlap(overlapped, overlapping.Build(_root));
        return this;
    }

    public static LayoutBuilderRoot operator <<(LayoutBuilderRoot root, LayoutBuilderContainer item)
    {
        root._root.Add(item.Build(root._root));
        return root;
    }

    public static LayoutBuilderRoot operator <<(LayoutBuilderRoot root, LayoutBuilderItem item)
    {
        root._root.Add(item.Build(root._root));
        return root;
    }
}
