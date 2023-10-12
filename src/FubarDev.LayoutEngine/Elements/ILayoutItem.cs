using System.Drawing;

namespace FubarDev.LayoutEngine.Elements;

public interface ILayoutItem
{
    string? Name { get; set; }

    Point Location { get; }
    
    Rectangle Bounds { get; }
    
    Size Size { get; }
    
    Size MinimumSize { get; }

    Size MaximumSize { get; }
    
    int Width { get; }
    
    int Height { get; }

    Visibility Visibility { get; }
    
    Margin Margin { get; }
    
    Margin Padding { get; }

    void SetBounds(Rectangle bounds);
}
