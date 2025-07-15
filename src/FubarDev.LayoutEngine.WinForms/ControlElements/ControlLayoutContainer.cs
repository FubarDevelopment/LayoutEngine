using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine.ControlElements;

/// <summary>
/// Represents a layout container for Windows Forms controls.
/// </summary>
/// <param name="control">The control to be wrapped as a layout container.</param>
/// <param name="hiddenVisibility">The visibility state to use when the control is not visible.</param>
public class ControlLayoutContainer(Control control, Visibility hiddenVisibility = Visibility.Collapsed)
    : ControlLayoutItem(control, hiddenVisibility), ILayoutContainer
{
    private List<ILayoutItem> _children = [];

    /// <inheritdoc />
    public ILayoutEngine? LayoutEngine { get; set; }

    /// <inheritdoc />
    public IReadOnlyCollection<ILayoutItem> GetChildren()
    {
        return _children;
    }

    /// <inheritdoc />
    public void SetChildren(IReadOnlyCollection<ILayoutItem> children)
    {
        _children = children.ToList();
    }

    /// <inheritdoc />
    public virtual void Add(ILayoutItem item)
    {
        _children.Add(item);
    }
}
