using System;
using System.Drawing;

using FubarDev.LayoutEngine.AttachedProperties;
using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.LayoutBuilder;

/// <summary>
/// Provides a builder for creating and configuring layout items.
/// </summary>
/// <param name="itemFactory">The factory function to create the item.</param>
public sealed class LayoutBuilderItem(Func<ILayoutOverlapLookup, ILayoutItem> itemFactory)
{
    private Func<ILayoutOverlapLookup, ILayoutItem> _itemFactory = itemFactory;
    private ILayoutItem? _item;

    internal ILayoutItem Build(ILayoutOverlapLookup overlapLookup) => _item ??= _itemFactory(overlapLookup);

    /// <summary>
    /// Sets the name of the item.
    /// </summary>
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

    /// <summary>
    /// Sets the minimum size of the item.
    /// </summary>
    public LayoutBuilderItem MinimumSize(Size value)
    {
        var oldFactory = _itemFactory;
        _itemFactory = overlapLookup =>
        {
            var newContainer = oldFactory(overlapLookup);
            ((ISettableMinimumSize)newContainer).MinimumSize = value;
            return newContainer;
        };

        return this;
    }

    /// <summary>
    /// Sets the margin of the item.
    /// </summary>
    public LayoutBuilderItem Margin(Margin value)
    {
        var oldFactory = _itemFactory;
        _itemFactory = overlapLookup =>
        {
            var newContainer = oldFactory(overlapLookup);
            ((ISettableMargin)newContainer).Margin = value;
            return newContainer;
        };

        return this;
    }

    /// <summary>
    /// Sets the padding of the item.
    /// </summary>
    public LayoutBuilderItem Padding(Margin value)
    {
        var oldFactory = _itemFactory;
        _itemFactory = overlapLookup =>
        {
            var newContainer = oldFactory(overlapLookup);
            ((ISettablePadding)newContainer).Padding = value;
            return newContainer;
        };

        return this;
    }

    /// <summary>
    /// Sets the identifier for the item.
    /// </summary>
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

    /// <summary>
    /// Sets the horizontal alignment for the item.
    /// </summary>
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

    /// <summary>
    /// Sets the vertical alignment for the item.
    /// </summary>
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

    /// <summary>
    /// Sets the width for the item.
    /// </summary>
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

    /// <summary>
    /// Sets the height for the item.
    /// </summary>
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
