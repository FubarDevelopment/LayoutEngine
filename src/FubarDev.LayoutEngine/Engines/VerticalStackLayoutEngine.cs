using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using FubarDev.LayoutEngine.AttachedProperties;
using FubarDev.LayoutEngine.Elements;
using FubarDev.LayoutEngine.LayoutCalculation;

namespace FubarDev.LayoutEngine.Engines;

public class VerticalStackLayoutEngine : IVerticalLayoutEngine
{
    public VerticalStackLayoutEngine(HorizontalAlignment defaultAlignment = LayoutPane.DefaultHorizontalAlignment)
    {
        DefaultHorizontalAlignment = defaultAlignment;
    }

    public HorizontalAlignment DefaultHorizontalAlignment { get; }

    public void Layout(ILayoutContainer container, Rectangle bounds)
    {
        var children = container.GetUncollapsedChildren()
            .ToList();
        var calculator = new VerticalElementSizeCalculator(bounds, children);
        var elementSizes = calculator.Calculate(children);

        var position = bounds.Location;
        var width = bounds.Width;
        foreach (var control in children)
        {
            var newSize = elementSizes[control];
            newSize += control.Margin.Vertical;

            var newBounds = new Rectangle(position.X, position.Y, width, newSize)
                .Shrink(control.Margin);

            var horizontalLayout = AttachedHorizontalAlignment.GetValue(control) ?? DefaultHorizontalAlignment;
            newBounds = control.ApplyLayout(newBounds, horizontalLayout);

            // Respect min & max size
            var size = control.EnsureMinimumSize(control.EnsureMaximumSize(newBounds.Size));
            newBounds = new Rectangle(newBounds.Location, size);
            newSize = size.Height + control.Margin.Vertical;

            control.SetBounds(newBounds);
            control.TryLayout(newBounds);

            position.Offset(0, newSize);
        }
    }

    private class VerticalElementSizeCalculator : ElementSizeCalculator
    {
        public VerticalElementSizeCalculator(
            Rectangle bounds,
            IReadOnlyCollection<ILayoutItem> children)
            : this(CalculateFactorInfo(bounds, children))
        {
        }

        private VerticalElementSizeCalculator((int RemainingFactorSize, double FactorSum) factorInfo)
            : base(factorInfo.RemainingFactorSize, factorInfo.FactorSum)
        {
        }

        protected override ElementInfo CalculateElementInfo(ILayoutItem item, int remainingSize, double remainingFactors)
        {
            var itemMinSize = item.MinimumSize;
            var itemMaxSize = item.MaximumSize;
            int? minSize = itemMinSize.Height != 0 ? itemMinSize.Height : null;
            int? maxSize = itemMaxSize.Height != 0 ? itemMaxSize.Height : null;
            var attachedSize = AttachedHeight.GetValue(item);
            var result = attachedSize switch
            {
                AttachedSize.UnchangedSize => new ElementInfo(item, item.Height)
                { MinSize = minSize, MaxSize = maxSize },
                AttachedSize.FixedSize s => new ElementInfo(item, s.Value)
                { MinSize = minSize, MaxSize = maxSize },
                AttachedSize.FactorSize s => new ElementInfo(item, (int)(remainingSize * s.Value / remainingFactors), s.Value)
                { MinSize = minSize, MaxSize = maxSize },
                _ => throw new NotSupportedException()
            };

            return result.EnsureLimits();
        }

        private static int GetUsedSize(IEnumerable<ILayoutItem> controls)
        {
            return controls
                .Sum(control => AttachedHeight.GetValue(control) switch
                {
                    AttachedSize.UnchangedSize => control.EnsureMinimumSize(new Size(0, control.Height)).Height + control.Margin.Vertical,
                    AttachedSize.FixedSize s => control.EnsureMinimumSize(new Size(0, s.Value)).Height + control.Margin.Vertical,
                    AttachedSize.FactorSize => control.Margin.Vertical,
                    _ => throw new NotSupportedException(),
                });
        }

        private static (int RemainingFactorSize, double FactorSum) CalculateFactorInfo(
            Rectangle bounds,
            IReadOnlyCollection<ILayoutItem> children)
        {
            var usedSize = GetUsedSize(children);
            var factorSumRemaining = children.Select(AttachedHeight.GetValue).OfType<AttachedSize.FactorSize>().Select(x => x.Value).Sum();
            var factorSizeRemaining = Math.Max(bounds.Height - usedSize, 0);
            return (factorSizeRemaining, factorSumRemaining);
        }
    }
}
