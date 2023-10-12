using FubarDev.LayoutEngine.AttachedProperties;
using FubarDev.LayoutEngine.Elements;
using FubarDev.LayoutEngine.Engines;
using FubarDev.LayoutEngine.Test.TestElements;

namespace FubarDev.LayoutEngine.Test;

public class ComplexLayoutTests
{
    private static readonly ILayoutEngine HorizontalLayoutEngine = new HorizontalStackLayoutEngine();
    private static readonly ILayoutEngine VerticalLayoutEngine = new VerticalStackLayoutEngine();

    [Fact]
    public void LayoutMustNotHaveInvisibleItems()
    {
        ILayoutContainer globalView, header;
        ILayoutItem logo, dashboard, moduleSelector;
        var root = new TestRoot(HorizontalLayoutEngine)
        {
            (globalView = new LayoutPane(VerticalLayoutEngine)
            {
                (header = new LayoutPane(HorizontalLayoutEngine)
                {
                    (logo = new TestItem()
                    {
                        Bounds = new Rectangle(0, 0, 10, 10),
                    }).SetVerticalAlignment(VerticalAlignment.Top),
                    (moduleSelector = new TestItem()
                    {
                        Bounds = new Rectangle(0, 0, 10, 50),
                    }).SetLayoutWidth(AttachedSize.Factor(1)),
                }),
            }).SetLayoutWidth(AttachedSize.Factor(1)),
            (dashboard = new TestItem()
            {
                Bounds = new Rectangle(0, 0, 10, 100),
            }),
        };

        root.SetBounds(new Rectangle(0, 0, 100, 100));
        root.Layout();

        Assert.Equal(new Rectangle(0, 0, 100, 100), root.Bounds);
        Assert.Equal(new Rectangle(0, 0, 90, 100), globalView.Bounds);
        Assert.Equal(new Rectangle(90, 0, 10, 100), dashboard.Bounds);
        Assert.Equal(new Rectangle(0, 0, 90, 10), header.Bounds);
        Assert.Equal(new Rectangle(0, 0, 10, 10), logo.Bounds);
        Assert.Equal(new Rectangle(10, 0, 80, 10), moduleSelector.Bounds);
    }

    [Fact]
    public void MinSizeMustBe70X70()
    {
        ILayoutContainer globalView, header, spacerTop, spacerBottom, coreAppView, spacerLeft, spacerRight;
        ILayoutItem logo, dashboard, moduleSelector, appMenu, appDialog;
        var root = new TestRoot(HorizontalLayoutEngine)
        {
            (globalView = new LayoutPane(VerticalLayoutEngine)
            {
                (header = new LayoutPane(HorizontalLayoutEngine)
                {
                    (logo = new TestItem()
                    {
                        Bounds = new Rectangle(0, 0, 10, 10),
                    }).SetVerticalAlignment(VerticalAlignment.Top),
                    (moduleSelector = new TestItem()
                    {
                        Bounds = new Rectangle(0, 0, 10, 50),
                    }).SetLayoutWidth(AttachedSize.Factor(1)),
                }),
                (spacerTop = new LayoutPane()).SetLayoutHeight(AttachedSize.Factor(1)),
                (coreAppView = new LayoutPane(HorizontalLayoutEngine)
                {
                    (spacerLeft = new LayoutPane()).SetLayoutWidth(AttachedSize.Factor(1)),
                    (appMenu = new TestItem()
                    {
                        Bounds = new Rectangle(0, 0, 20, 60),
                    }).SetVerticalAlignment(VerticalAlignment.Center),
                    (appDialog= new TestItem()
                    {
                        Bounds = new Rectangle(0, 0, 40, 60),
                    }).SetVerticalAlignment(VerticalAlignment.Center),
                    (spacerRight = new LayoutPane()).SetLayoutWidth(AttachedSize.Factor(1)),
                }),
                (spacerBottom = new LayoutPane()).SetLayoutHeight(AttachedSize.Factor(1)),
            }).SetLayoutWidth(AttachedSize.Factor(1)),
            (dashboard = new TestItem()
            {
                Bounds = new Rectangle(0, 0, 10, 100),
            }),
        };

        var minSize = root.GetMinimumClientSize();
        Assert.Equal(70, minSize.Width);
        Assert.Equal(70, minSize.Height);
    }

    [Fact]
    public void MinSizeWithOverlappedItemMustBe70X60()
    {
        ILayoutContainer globalView, header, spacerTop, spacerBottom, coreAppView, spacerLeft, spacerRight, globalViewOverlap;
        ILayoutItem logo, dashboard, moduleSelector, appMenu, appDialog;
        var root = new TestRoot(HorizontalLayoutEngine)
        {
            (globalView = new LayoutPane(VerticalLayoutEngine)
            {
                (header = new LayoutPane(HorizontalLayoutEngine)
                {
                    (logo = new TestItem()
                    {
                        Bounds = new Rectangle(0, 0, 10, 10),
                    }).SetVerticalAlignment(VerticalAlignment.Top),
                    (moduleSelector = new TestItem()
                    {
                        Bounds = new Rectangle(0, 0, 10, 50),
                    }).SetLayoutWidth(AttachedSize.Factor(1)),
                }),
            }).SetLayoutWidth(AttachedSize.Factor(1)),
            (dashboard = new TestItem()
            {
                Bounds = new Rectangle(0, 0, 10, 100),
            }),
        };

        root.AddOverlap(
            globalView,
            (globalViewOverlap = new LayoutPane(VerticalLayoutEngine)
            {
                (spacerTop = new LayoutPane()).SetLayoutHeight(AttachedSize.Factor(1)),
                (coreAppView = new LayoutPane(HorizontalLayoutEngine)
                {
                    (spacerLeft = new LayoutPane()).SetLayoutWidth(AttachedSize.Factor(1)),
                    (appMenu = new TestItem()
                    {
                        Bounds = new Rectangle(0, 0, 20, 60),
                    }).SetVerticalAlignment(VerticalAlignment.Center),
                    (appDialog= new TestItem()
                    {
                        Bounds = new Rectangle(0, 0, 40, 60),
                    }).SetVerticalAlignment(VerticalAlignment.Center),
                    (spacerRight = new LayoutPane()).SetLayoutWidth(AttachedSize.Factor(1)),
                }),
                (spacerBottom = new LayoutPane()).SetLayoutHeight(AttachedSize.Factor(1)),
            }));

        var minSize = root.GetMinimumClientSize();
        Assert.Equal(70, minSize.Width);
        Assert.Equal(60, minSize.Height);
    }
}
