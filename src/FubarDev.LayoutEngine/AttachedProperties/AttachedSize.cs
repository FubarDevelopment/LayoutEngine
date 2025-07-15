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
    public sealed class FixedSize(int value) : AttachedSize
    {
        public int Value { get; } = value;

        public override string ToString() => $"Fixed={Value}";
    }

    [DebuggerDisplay("Factor={Value}")]
    public sealed class FactorSize(double value) : AttachedSize
    {
        public double Value { get; } = value;

        public override string ToString() => $"Factor={Value}";
    }
}
