namespace FubarDev.LayoutEngine.AttachedProperties;

public abstract record AttachedSize
{
    public static UnchangedSize Unchanged{ get; } = new();

    public static FixedSize Fixed(int value) => new(value);

    public static FactorSize Factor(double value) => new(value);

    public sealed record UnchangedSize : AttachedSize;

    public sealed record FixedSize(int Value) : AttachedSize;
    
    public sealed record FactorSize(double Value) : AttachedSize;
}
