﻿using FubarDev.LayoutEngine.AttachedProperties;
using FubarDev.LayoutEngine.Elements;
using FubarDev.LayoutEngine.Engines;
using FubarDev.LayoutEngine.Test.TestElements;

namespace FubarDev.LayoutEngine.Test;

public class VerticalLayoutTests
{
    private static readonly ILayoutEngine VerticalLayoutEngine = new VerticalStackLayoutEngine();

    [Fact]
    public void SingleItemFullWidth()
    {
        ILayoutItem testItem1;

        var root = new TestRoot(VerticalLayoutEngine)
        {
            (testItem1 = new TestItem().SetHorizontalAlignment(HorizontalAlignment.Fill)).SetLayoutHeight(AttachedSize.Factor(1)),
        };

        root.LayoutEngine = VerticalLayoutEngine;
        root.SetBounds(new Rectangle(0, 0, 100, 100));

        root.Layout();

        Assert.Equal(0, testItem1.Location.X);
        Assert.Equal(0, testItem1.Location.Y);
        Assert.Equal(100, testItem1.Width);
        Assert.Equal(100, testItem1.Height);
    }

    [Fact]
    public void SingleItemFullWidthWithIncreaseAndDecrease()
    {
        ILayoutItem testItem1;

        var root = new TestRoot(VerticalLayoutEngine)
        {
            (testItem1 = new TestItem().SetHorizontalAlignment(HorizontalAlignment.Fill)).SetLayoutHeight(AttachedSize.Factor(1)),
        };

        root.SetBounds(new Rectangle(0, 0, 100, 100));
        root.Layout();

        root.SetBounds(new Rectangle(0, 0, 120, 120));
        root.Layout();

        root.SetBounds(new Rectangle(0, 0, 90, 90));
        root.Layout();

        Assert.Equal(0, testItem1.Location.X);
        Assert.Equal(0, testItem1.Location.Y);
        Assert.Equal(90, testItem1.Width);
        Assert.Equal(90, testItem1.Height);
    }

    [Fact]
    public void SingleItemFixedWidthAndHeightWithTopAlignment()
    {
        ILayoutItem testItem1 = new TestItem().SetHorizontalAlignment(HorizontalAlignment.Left);
        testItem1.SetBounds(new Rectangle(0, 0, 90, 90));

        var root = new TestRoot(VerticalLayoutEngine)
        {
            testItem1,
        };

        root.SetBounds(new Rectangle(0, 0, 100, 100));
        root.Layout();

        Assert.Equal(0, testItem1.Location.X);
        Assert.Equal(0, testItem1.Location.Y);
        Assert.Equal(90, testItem1.Width);
        Assert.Equal(90, testItem1.Height);
    }

    [Fact]
    public void SingleItemFixedWidthAndHeightWithCenterAlignment()
    {
        ILayoutItem testItem1 = new TestItem().SetHorizontalAlignment(HorizontalAlignment.Center);
        testItem1.SetBounds(new Rectangle(0, 0, 90, 90));

        var root = new TestRoot(VerticalLayoutEngine)
        {
            testItem1,
        };

        root.SetBounds(new Rectangle(0, 0, 100, 100));
        root.Layout();

        Assert.Equal(5, testItem1.Location.X);
        Assert.Equal(0, testItem1.Location.Y);
        Assert.Equal(90, testItem1.Width);
        Assert.Equal(90, testItem1.Height);
    }

    [Fact]
    public void SingleItemFixedWidthAndHeightWithBottomAlignment()
    {
        ILayoutItem testItem1 = new TestItem().SetHorizontalAlignment(HorizontalAlignment.Right);
        testItem1.SetBounds(new Rectangle(0, 0, 90, 90));

        var root = new TestRoot(VerticalLayoutEngine)
        {
            testItem1,
        };

        root.SetBounds(new Rectangle(0, 0, 100, 100));
        root.Layout();

        Assert.Equal(10, testItem1.Location.X);
        Assert.Equal(0, testItem1.Location.Y);
        Assert.Equal(90, testItem1.Width);
        Assert.Equal(90, testItem1.Height);
    }
}
