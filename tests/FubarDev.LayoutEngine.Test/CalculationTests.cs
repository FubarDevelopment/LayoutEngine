using FubarDev.LayoutEngine.Elements;
using FubarDev.LayoutEngine.Engines;
using FubarDev.LayoutEngine.Test.TestElements;

using static FubarDev.LayoutEngine.AttachedProperties.AttachedSize;

namespace FubarDev.LayoutEngine.Test;

public class CalculationTests
{
    private static readonly ILayoutEngine HorizontalLayoutEngine = new HorizontalStackLayoutEngine();
    private static readonly ILayoutEngine VerticalLayoutEngine = new VerticalStackLayoutEngine();

    [Fact]
    public void TestMinimumSizeApplication()
    {
        var expectedMinSize = new Size(100, 100);
        ILayoutContainer container;
        ILayoutItem item;
        var root = new TestRoot(HorizontalLayoutEngine)
        {
            (container = new LayoutPane(VerticalLayoutEngine)
            {
                (item = new TestItem()
                {
                    MinimumSize = expectedMinSize,
                }),
            }.SetLayoutWidth(Factor(1))),
        };

        var result = root.ApplyMinimumSize();

        Assert.Equal(expectedMinSize, result);
        Assert.Equal(expectedMinSize, root.MinimumSize);
        Assert.Equal(expectedMinSize, container.MinimumSize);
        Assert.Equal(expectedMinSize, item.MinimumSize);
    }

    [Fact]
    public void TestChildMargin()
    {
        ILayoutItem appMenu, appDialog;
        var root = new TestRoot(HorizontalLayoutEngine)
        {
            (appMenu = new TestItem()
            {
                MinimumSize = new Size(20, 60),
                Margin = new Margin(3, 4),
            }),
            (appDialog = new TestItem()
            {
                MinimumSize = new Size(40, 60),
                Margin = new Margin(3, 4),
            }).SetLayoutWidth(Factor(1)),
        };

        root.Layout();
        Assert.Equal(new Rectangle(3, 4, 20, 60), appMenu.Bounds);
        Assert.Equal(new Rectangle(29, 4, 40, 60), appDialog.Bounds);
    }
}
