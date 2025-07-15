namespace FubarDev.LayoutEngine;

/// <summary>
/// Defines the contract for a vertical layout engine that arranges items vertically.
/// </summary>
public interface IVerticalLayoutEngine : ILayoutEngine
{
    /// <summary>
    /// Gets the default horizontal alignment for items in the vertical layout.
    /// </summary>
    HorizontalAlignment DefaultHorizontalAlignment { get; }
}
