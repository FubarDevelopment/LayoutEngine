using System;

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

    public LayoutBuilderRoot SetDefaultAlignment(HorizontalAlignment alignment)
    {
        _root.LayoutEngine = new VerticalStackLayoutEngine(alignment);
        return this;
    }

    public LayoutBuilderRoot SetDefaultAlignment(VerticalAlignment alignment)
    {
        _root.LayoutEngine = new HorizontalStackLayoutEngine(alignment);
        return this;
    }

    public LayoutBuilderRoot Identifier(string identifier)
    {
        AttachedIdentifier.SetValue(_root, identifier);
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
