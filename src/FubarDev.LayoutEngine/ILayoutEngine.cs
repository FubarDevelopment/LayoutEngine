using System.Drawing;

using FubarDev.LayoutEngine.Elements;

namespace FubarDev.LayoutEngine;

public interface ILayoutEngine
{
    void Layout(ILayoutContainer container, Rectangle bounds);
}
