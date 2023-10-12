using System.Diagnostics;

namespace FubarDev.LayoutEngine.AttachedProperties;

public abstract class AttachedSize
{
    public static UnchangedSize Unchanged{ get; } = new();

    public static FixedSize Fixed(int value) => new(value);

    public static FactorSize Factor(double value) => new(value);

    [DebuggerDisplay("Unchanged")]
    public sealed class UnchangedSize : AttachedSize
    {
        public override string ToString() => "Unchanged";
    }

    [DebuggerDisplay("Fixed={Value}")]
    public sealed class FixedSize : AttachedSize
    {
        public FixedSize(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public override string ToString() => $"Fixed={Value}";
    }

    [DebuggerDisplay("Factor={Value}")]
    public sealed class FactorSize : AttachedSize
    {
        public FactorSize(double value)
        {
            Value = value;
        }

        public double Value { get; }

        public override string ToString() => $"Factor={Value}";
    }
}
