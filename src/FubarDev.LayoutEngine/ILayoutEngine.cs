using System.Drawing;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine;

/// <summary>
/// Defines the contract for a layout engine that arranges layout items within a container.
/// </summary>
public interface ILayoutEngine
{
    /// <summary>
    /// Arranges the child items of the specified container within the given bounds.
    /// </summary>
    /// <param name="container">The container whose children should be arranged.</param>
    /// <param name="bounds">The bounds within which to arrange the items.</param>
    void Layout(ILayoutContainer container, Rectangle bounds);
}
