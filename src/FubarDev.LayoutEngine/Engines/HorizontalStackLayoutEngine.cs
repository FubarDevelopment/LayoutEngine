using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using FubarDev.LayoutEngine.AttachedProperties;
using FubarDev.LayoutEngine.Elements;
using FubarDev.LayoutEngine.LayoutCalculation;

namespace FubarDev.LayoutEngine.Engines;

public sealed class HorizontalStackLayoutEngine(
    VerticalAlignment defaultAlignment = LayoutPane.DefaultVerticalAlignment)
    : IHorizontalLayoutEngine
{
    public VerticalAlignment DefaultVerticalAlignment { get; } = defaultAlignment;

    public void Layout(ILayoutContainer container, Rectangle bounds)
    {
        var children = container.GetUncollapsedChildren()
            .ToList();
        var calculator = new HorizontalElementSizeCalculator(bounds, children);
        var elementSizes = calculator.Calculate(children);

        var position = bounds.Location;
        var height = bounds.Height;
        foreach (var control in children)
        {
            var newSize = elementSizes[control];
            newSize += control.Margin.Horizontal;

            var newBounds = new Rectangle(position.X, position.Y,newSize, height)
                .Shrink(control.Margin);

            var verticalLayout = AttachedVerticalAlignment.GetValue(control) ?? DefaultVerticalAlignment;
            newBounds = control.ApplyLayout(newBounds, verticalLayout);

            // Respect min & max size
            var size = control.EnsureMinimumSize(control.EnsureMaximumSize(newBounds.Size, control.MaximumSize), control.GetEffectiveMinimumSize());
            newBounds = new Rectangle(newBounds.Location, size);
            newSize = size.Width + control.Margin.Horizontal;

            control.SetBounds(newBounds);
            control.TryLayout(newBounds);

            position.Offset(newSize, 0);
        }
    }

    private class HorizontalElementSizeCalculator : ElementSizeCalculator
    {
        public HorizontalElementSizeCalculator(
            Rectangle bounds,
            IReadOnlyCollection<ILayoutItem> children)
            : this(CalculateFactorInfo(bounds, children))
        {
        }

        private HorizontalElementSizeCalculator((int RemainingFactorSize, double FactorSum) factorInfo)
            : base(factorInfo.RemainingFactorSize, factorInfo.FactorSum)
        {
        }

        protected override ElementInfo CalculateElementInfo(ILayoutItem item, int remainingSize, double remainingFactors)
        {
            var itemMinSize = item.GetEffectiveMinimumSize();
            var itemMaxSize = item.MaximumSize;
            int? minSize = itemMinSize.Width != 0 ? itemMinSize.Width : null;
            int? maxSize = itemMaxSize.Width != 0 ? itemMaxSize.Width : null;
            var attachedSize = AttachedWidth.GetValue(item);
            var result = attachedSize switch
            {
                AttachedSize.UnchangedSize => new ElementInfo(item, item.Width)
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
                .Sum(control => AttachedWidth.GetValue(control) switch
                {
                    AttachedSize.UnchangedSize => control.EnsureMinimumSize(new Size(control.Width, 0), control.GetEffectiveMinimumSize()).Width + control.Margin.Horizontal,
                    AttachedSize.FixedSize s => control.EnsureMinimumSize(new Size(s.Value, 0), control.GetEffectiveMinimumSize()).Width + control.Margin.Horizontal,
                    AttachedSize.FactorSize => control.Margin.Horizontal,
                    _ => throw new NotSupportedException(),
                });
        }

        private static (int RemainingFactorSize, double FactorSum) CalculateFactorInfo(
            Rectangle bounds,
            IReadOnlyCollection<ILayoutItem> children)
        {
            var usedSize = GetUsedSize(children);
            var factorSumRemaining = children
                .Select(AttachedWidth.GetValue)
                .OfType<AttachedSize.FactorSize>()
                .Select(x => x.Value)
                .Sum();
            var factorSizeRemaining = Math.Max(bounds.Width - usedSize, 0);
            return (factorSizeRemaining, factorSumRemaining);
        }
    }
}
