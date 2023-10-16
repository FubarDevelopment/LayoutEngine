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
        Assert.Equal(expectedMinSize, root.GetEffectiveMinimumSize());
        Assert.Equal(expectedMinSize, container.GetEffectiveMinimumSize());
        Assert.Equal(expectedMinSize, item.GetEffectiveMinimumSize());
    }

    [Fact]
    public void TestCanShrinkIfElementBecomesInvisible()
    {
        var itemMinSize = new Size(100, 100);

        ILayoutContainer container;
        TestItem item;
        var root = new TestRoot(HorizontalLayoutEngine)
        {
            (container = new TestContainer(VerticalLayoutEngine)
            {
                (item = new TestItem()
                {
                    MinimumSize = itemMinSize,
                }),
            }.SetLayoutWidth(Factor(1))),
        };

        var result = root.ApplyMinimumSize();

        Assert.Equal(itemMinSize, result);
        Assert.Equal(itemMinSize, root.GetEffectiveMinimumSize());
        Assert.Equal(itemMinSize, container.GetEffectiveMinimumSize());
        Assert.Equal(itemMinSize, item.GetEffectiveMinimumSize());

        var expectedCollapsedSize = new Size();
        item.Visibility = Visibility.Collapsed;
        result = root.ApplyMinimumSize();
        Assert.Equal(expectedCollapsedSize, result);
        Assert.Equal(expectedCollapsedSize, root.GetEffectiveMinimumSize());
        Assert.Equal(expectedCollapsedSize, container.GetEffectiveMinimumSize());
        Assert.Equal(itemMinSize, item.GetEffectiveMinimumSize());
    }

    [Fact]
    public void TestCanShrinkIfElementBecomesInvisibleWithLayoutPane()
    {
        var itemMinSize = new Size(100, 100);

        ILayoutContainer container;
        TestItem item;
        var root = new TestRoot(HorizontalLayoutEngine)
        {
            (container = new LayoutPane(VerticalLayoutEngine)
            {
                (item = new TestItem()
                {
                    MinimumSize = itemMinSize,
                }),
            }.SetLayoutWidth(Factor(1))),
        };

        var result = root.ApplyMinimumSize();

        Assert.Equal(itemMinSize, result);
        Assert.Equal(itemMinSize, root.GetEffectiveMinimumSize());
        Assert.Equal(itemMinSize, container.GetEffectiveMinimumSize());
        Assert.Equal(itemMinSize, item.GetEffectiveMinimumSize());

        var expectedCollapsedSize = new Size();
        item.Visibility = Visibility.Collapsed;
        result = root.ApplyMinimumSize();
        Assert.Equal(expectedCollapsedSize, result);
        Assert.Equal(expectedCollapsedSize, root.GetEffectiveMinimumSize());

        // The LayoutPane becomes collapsed if all its children are collapsed!
        Assert.Equal(itemMinSize, container.GetEffectiveMinimumSize());
        Assert.Equal(itemMinSize, item.GetEffectiveMinimumSize());
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
