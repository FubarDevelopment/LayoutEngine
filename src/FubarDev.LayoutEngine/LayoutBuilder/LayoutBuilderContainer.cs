using System;
using System.Drawing;

using FubarDev.LayoutEngine.AttachedProperties;
using FubarDev.LayoutEngine.Elements;
using FubarDev.LayoutEngine.Engines;

namespace FubarDev.LayoutEngine.LayoutBuilder;

/// <summary>
/// Provides a builder for creating and configuring layout containers.
/// </summary>
/// <param name="containerFactory">The factory function to create the container.</param>
public sealed class LayoutBuilderContainer(Func<ILayoutOverlapLookup, ILayoutContainer> containerFactory)
{
    private Func<ILayoutOverlapLookup, ILayoutContainer> _containerFactory = containerFactory;
    private ILayoutContainer? _container;

    internal ILayoutContainer Build(ILayoutOverlapLookup overlapLookup) => _container ??= _containerFactory(overlapLookup);

    /// <summary>
    /// Sets the name of the container.
    /// </summary>
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

    /// <summary>
    /// Sets the minimum size of the container.
    /// </summary>
    public LayoutBuilderContainer MinimumSize(Size value)
    {
        var oldFactory = _containerFactory;
        _containerFactory = overlapLookup =>
        {
            var newContainer = oldFactory(overlapLookup);
            ((ISettableMinimumSize)newContainer).MinimumSize = value;
            return newContainer;
        };

        return this;
    }

    /// <summary>
    /// Sets the margin of the container.
    /// </summary>
    public LayoutBuilderContainer Margin(Margin value)
    {
        var oldFactory = _containerFactory;
        _containerFactory = overlapLookup =>
        {
            var newContainer = oldFactory(overlapLookup);
            ((ISettableMargin)newContainer).Margin = value;
            return newContainer;
        };

        return this;
    }

    /// <summary>
    /// Sets the padding of the container.
    /// </summary>
    public LayoutBuilderContainer Padding(Margin value)
    {
        var oldFactory = _containerFactory;
        _containerFactory = overlapLookup =>
        {
            var newContainer = oldFactory(overlapLookup);
            ((ISettablePadding)newContainer).Padding = value;
            return newContainer;
        };

        return this;
    }

    /// <summary>
    /// Sets the vertical stack layout engine for the container.
    /// </summary>
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

    /// <summary>
    /// Sets the horizontal stack layout engine for the container.
    /// </summary>
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

    /// <summary>
    /// Sets the identifier for the container.
    /// </summary>
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

    /// <summary>
    /// Sets the horizontal alignment for the container.
    /// </summary>
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

    /// <summary>
    /// Sets the vertical alignment for the container.
    /// </summary>
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

    /// <summary>
    /// Sets the width for the container.
    /// </summary>
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

    /// <summary>
    /// Sets the height for the container.
    /// </summary>
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

    /// <summary>
    /// Adds a root item to the container using the left shift operator.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="item">The root item to add.</param>
    /// <returns>The updated container.</returns>
    public static LayoutBuilderContainer operator <<(LayoutBuilderContainer container, LayoutBuilderRoot item)
    {
        if (container._container != null)
        {
            throw new InvalidOperationException("Cannot modify after building.");
        }

        var oldFactory = container._containerFactory;
        container._containerFactory = overlapLookup =>
        {
            var newContainer = oldFactory(overlapLookup);
            newContainer.Add(item.Build());
            return newContainer;
        };

        return container;
    }

    /// <summary>
    /// Adds a container item to the container using the left shift operator.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="item">The container item to add.</param>
    /// <returns>The updated container.</returns>
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

    /// <summary>
    /// Adds an item to the container using the left shift operator.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="item">The item to add.</param>
    /// <returns>The updated container.</returns>
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
