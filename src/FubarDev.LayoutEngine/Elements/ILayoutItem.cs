using System.Drawing;

namespace FubarDev.LayoutEngine.Elements;

/// <summary>
/// Represents an item that can be laid out by the layout engine.
/// </summary>
public interface ILayoutItem
{
    /// <summary>
    /// Gets or sets the name of the layout item.
    /// </summary>
    string? Name { get; set; }

    /// <summary>
    /// Gets the location of the layout item.
    /// </summary>
    Point Location { get; }
    
    /// <summary>
    /// Gets the bounds of the layout item.
    /// </summary>
    Rectangle Bounds { get; }
    
    /// <summary>
    /// Gets the size of the layout item.
    /// </summary>
    Size Size { get; }
    
    /// <summary>
    /// Gets the minimum size of the layout item.
    /// </summary>
    Size MinimumSize { get; }

    /// <summary>
    /// Gets the maximum size of the layout item.
    /// </summary>
    Size MaximumSize { get; }
    
    /// <summary>
    /// Gets the width of the layout item.
    /// </summary>
    int Width { get; }
    
    /// <summary>
    /// Gets the height of the layout item.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Gets the visibility of the layout item.
    /// </summary>
    Visibility Visibility { get; }
    
    /// <summary>
    /// Gets the margin of the layout item.
    /// </summary>
    Margin Margin { get; }
    
    /// <summary>
    /// Gets the padding of the layout item.
    /// </summary>
    Margin Padding { get; }

    /// <summary>
    /// Sets the bounds of the layout item.
    /// </summary>
    /// <param name="bounds">The bounds to set.</param>
    void SetBounds(Rectangle bounds);
}
