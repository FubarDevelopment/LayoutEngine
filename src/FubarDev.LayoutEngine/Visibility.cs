namespace FubarDev.LayoutEngine;

/// <summary>
/// Specifies the visibility state of a layout item.
/// </summary>
public enum Visibility
{
    /// <summary>
    /// The item is not visible and does not occupy any space in the layout.
    /// </summary>
    Collapsed,
    /// <summary>
    /// The item is not visible but still occupies space in the layout.
    /// </summary>
    Hidden,
    /// <summary>
    /// The item is visible.
    /// </summary>
    Visible,
}
