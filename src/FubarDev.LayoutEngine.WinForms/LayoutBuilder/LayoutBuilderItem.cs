using System;

using FubarDev.LayoutEngine.AttachedProperties;
using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.LayoutBuilder;

public sealed class LayoutBuilderItem
{
    private Func<ILayoutOverlapLookup, ILayoutItem> _itemFactory;
    private ILayoutItem? _item;

    public LayoutBuilderItem(Func<ILayoutOverlapLookup, ILayoutItem> itemFactory)
    {
        _itemFactory = itemFactory;
    }

    internal ILayoutItem Build(ILayoutOverlapLookup overlapLookup) => _item ??= _itemFactory(overlapLookup);

    public LayoutBuilderItem Name(string name)
    {
        var oldFactory = _itemFactory;
        _itemFactory = overlapLookup =>
        {
            var newContainer = oldFactory(overlapLookup);
            newContainer.Name = name;
            return newContainer;
        };

        return this;
    }

    public LayoutBuilderItem Identifier(string identifier)
    {
        if (_item != null)
        {
            throw new InvalidOperationException("Cannot modify after building.");
        }

        var oldFactory = _itemFactory;
        _itemFactory = overlapLookup =>
        {
            var newItem = oldFactory(overlapLookup);
            AttachedIdentifier.SetValue(newItem, identifier);
            return newItem;
        };

        return this;
    }

    public LayoutBuilderItem HorizontalAlignment(HorizontalAlignment alignment)
    {
        if (_item != null)
        {
            throw new InvalidOperationException("Cannot modify after building.");
        }

        var oldFactory = _itemFactory;
        _itemFactory = overlapLookup =>
        {
            var newItem = oldFactory(overlapLookup);
            newItem.SetHorizontalAlignment(alignment);
            return newItem;
        };

        return this;
    }

    public LayoutBuilderItem VerticalAlignment(VerticalAlignment alignment)
    {
        if (_item != null)
        {
            throw new InvalidOperationException("Cannot modify after building.");
        }

        var oldFactory = _itemFactory;
        _itemFactory = overlapLookup =>
        {
            var newItem = oldFactory(overlapLookup);
            newItem.SetVerticalAlignment(alignment);
            return newItem;
        };

        return this;
    }

    public LayoutBuilderItem Width(AttachedSize width)
    {
        if (_item != null)
        {
            throw new InvalidOperationException("Cannot modify after building.");
        }

        var oldFactory = _itemFactory;
        _itemFactory = overlapLookup =>
        {
            var newItem = oldFactory(overlapLookup);
            newItem.SetLayoutWidth(width);
            return newItem;
        };

        return this;
    }

    public LayoutBuilderItem Height(AttachedSize height)
    {
        if (_item != null)
        {
            throw new InvalidOperationException("Cannot modify after building.");
        }

        var oldFactory = _itemFactory;
        _itemFactory = overlapLookup =>
        {
            var newItem = oldFactory(overlapLookup);
            newItem.SetLayoutHeight(height);
            return newItem;
        };

        return this;
    }
}
