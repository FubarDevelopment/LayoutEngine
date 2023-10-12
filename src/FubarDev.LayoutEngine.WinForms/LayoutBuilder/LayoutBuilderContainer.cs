using System;

using FubarDev.LayoutEngine.AttachedProperties;
using FubarDev.LayoutEngine.Elements;
using FubarDev.LayoutEngine.Engines;

namespace FubarDev.LayoutEngine.LayoutBuilder;

public sealed class LayoutBuilderContainer
{
    private Func<ILayoutOverlapLookup, ILayoutContainer> _containerFactory;
    private ILayoutContainer? _container;

    public LayoutBuilderContainer(Func<ILayoutOverlapLookup, ILayoutContainer> containerFactory)
    {
        _containerFactory = containerFactory;
    }

    internal ILayoutContainer Build(ILayoutOverlapLookup overlapLookup) => _container ??= _containerFactory(overlapLookup);

    public LayoutBuilderContainer Name(string name)
    {
        var oldFactory = _containerFactory;
        _containerFactory = overlapLookup =>
        {
            var newContainer = oldFactory(overlapLookup);
            newContainer.Name = name;
            return newContainer;
        };

        return this;
    }

    public LayoutBuilderContainer VerticalStackLayout(HorizontalAlignment alignment)
    {
        var oldFactory = _containerFactory;
        _containerFactory = overlapLookup =>
        {
            var newContainer = oldFactory(overlapLookup);
            newContainer.LayoutEngine = new VerticalStackLayoutEngine(alignment);
            return newContainer;
        };

        return this;
    }

    public LayoutBuilderContainer HorizontalStackLayout(VerticalAlignment alignment)
    {
        var oldFactory = _containerFactory;
        _containerFactory = overlapLookup =>
        {
            var newContainer = oldFactory(overlapLookup);
            newContainer.LayoutEngine = new HorizontalStackLayoutEngine(alignment);
            return newContainer;
        };

        return this;
    }

    public LayoutBuilderContainer Identifier(string identifier)
    {
        if (_container != null)
        {
            throw new InvalidOperationException("Cannot modify after building.");
        }

        var oldFactory = _containerFactory;
        _containerFactory = overlapLookup =>
        {
            var newContainer = oldFactory(overlapLookup);
            AttachedIdentifier.SetValue(newContainer, identifier);
            return newContainer;
        };

        return this;
    }

    public LayoutBuilderContainer HorizontalAlignment(HorizontalAlignment alignment)
    {
        if (_container != null)
        {
            throw new InvalidOperationException("Cannot modify after building.");
        }

        var oldFactory = _containerFactory;
        _containerFactory = overlapLookup =>
        {
            var newContainer = oldFactory(overlapLookup);
            newContainer.SetHorizontalAlignment(alignment);
            return newContainer;
        };

        return this;
    }

    public LayoutBuilderContainer VerticalAlignment(VerticalAlignment alignment)
    {
        if (_container != null)
        {
            throw new InvalidOperationException("Cannot modify after building.");
        }

        var oldFactory = _containerFactory;
        _containerFactory = overlapLookup =>
        {
            var newContainer = oldFactory(overlapLookup);
            newContainer.SetVerticalAlignment(alignment);
            return newContainer;
        };

        return this;
    }

    public LayoutBuilderContainer Width(AttachedSize width)
    {
        if (_container != null)
        {
            throw new InvalidOperationException("Cannot modify after building.");
        }

        var oldFactory = _containerFactory;
        _containerFactory = overlapLookup =>
        {
            var newContainer = oldFactory(overlapLookup);
            newContainer.SetLayoutWidth(width);
            return newContainer;
        };

        return this;
    }

    public LayoutBuilderContainer Height(AttachedSize height)
    {
        if (_container != null)
        {
            throw new InvalidOperationException("Cannot modify after building.");
        }

        var oldFactory = _containerFactory;
        _containerFactory = overlapLookup =>
        {
            var newContainer = oldFactory(overlapLookup);
            newContainer.SetLayoutHeight(height);
            return newContainer;
        };

        return this;
    }

    public static LayoutBuilderContainer operator <<(LayoutBuilderContainer container, LayoutBuilderContainer item)
    {
        if (container._container != null)
        {
            throw new InvalidOperationException("Cannot modify after building.");
        }

        var oldFactory = container._containerFactory;
        container._containerFactory = overlapLookup =>
        {
            var newContainer = oldFactory(overlapLookup);
            newContainer.Add(item.Build(overlapLookup));
            return newContainer;
        };

        return container;
    }

    public static LayoutBuilderContainer operator <<(LayoutBuilderContainer container, LayoutBuilderItem item)
    {
        if (container._container != null)
        {
            throw new InvalidOperationException("Cannot modify after building.");
        }

        var oldFactory = container._containerFactory;
        container._containerFactory = overlapLookup =>
        {
            var newContainer = oldFactory(overlapLookup);
            newContainer.Add(item.Build(overlapLookup));
            return newContainer;
        };

        return container;
    }
}
