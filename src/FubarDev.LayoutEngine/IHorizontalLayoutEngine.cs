namespace FubarDev.LayoutEngine;

/// <summary>
/// Defines the contract for a horizontal layout engine that arranges items horizontally.
/// </summary>
public interface IHorizontalLayoutEngine : ILayoutEngine
{
    /// <summary>
    /// Gets the default vertical alignment for items in the horizontal layout.
    /// </summary>
    VerticalAlignment DefaultVerticalAlignment { get; }
}
