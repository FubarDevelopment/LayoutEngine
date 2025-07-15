using System.Diagnostics;

namespace FubarDev.LayoutEngine.AttachedProperties;

/// <summary>
/// Provides attached size properties for layout items, such as fixed or factor-based sizing.
/// </summary>
public abstract class AttachedSize
{
    /// <summary>
    /// Gets an instance representing unchanged size.
    /// </summary>
    public static UnchangedSize Unchanged{ get; } = new();

    /// <summary>
    /// Creates a fixed size value.
    /// </summary>
    /// <param name="value">The fixed size value.</param>
    /// <returns>An instance representing the fixed size.</returns>
    public static FixedSize Fixed(int value) => new(value);

    /// <summary>
    /// Creates a factor size value.
    /// </summary>
    /// <param name="value">The factor value.</param>
    /// <returns>An instance representing the factor size.</returns>
    public static FactorSize Factor(double value) => new(value);

    /// <summary>
    /// Represents an unchanged size value.
    /// </summary>
    [DebuggerDisplay("Unchanged")]
    public sealed class UnchangedSize : AttachedSize
    {
        /// <inheritdoc />
        public override string ToString() => "Unchanged";
    }

    /// <summary>
    /// Represents a fixed size value.
    /// </summary>
    [DebuggerDisplay("Fixed={Value}")]
    public sealed class FixedSize(int value) : AttachedSize
    {
        /// <summary>
        /// Gets the fixed size value.
        /// </summary>
        public int Value { get; } = value;

        /// <inheritdoc />
        public override string ToString() => $"Fixed={Value}";
    }

    /// <summary>
    /// Represents a factor size value.
    /// </summary>
    [DebuggerDisplay("Factor={Value}")]
    public sealed class FactorSize(double value) : AttachedSize
    {
        /// <summary>
        /// Gets the factor size value.
        /// </summary>
        public double Value { get; } = value;

        /// <inheritdoc />
        public override string ToString() => $"Factor={Value}";
    }
}
