using System.Drawing;

using Equatable.Attributes;

namespace FubarDev.LayoutEngine;

/// <summary>
/// Represents the margin around a layout item.
/// </summary>
[Equatable]
public partial struct Margin
{
    private bool _all;
    private int _top;
    private int _left;
    private int _right;
    private int _bottom;

    /// <summary>
    /// Gets an empty margin.
    /// </summary>
    public static readonly Margin Empty = new Margin(0);

    /// <summary>
    /// Initializes a new instance of the <see cref="Margin"/> struct with the same value for all sides.
    /// </summary>
    /// <param name="all">The margin value for all sides.</param>
    public Margin(int all)
    {
        _all = true;
        _top = _left = _right = _bottom = all;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Margin"/> struct with the same value for left/right and top/bottom.
    /// </summary>
    /// <param name="leftRight">The margin value for left and right.</param>
    /// <param name="topBottom">The margin value for top and bottom.</param>
    public Margin(int leftRight, int topBottom)
    {
        _left = _right = leftRight;
        _top = _bottom = topBottom;
        _all = _top == _left && _top == _right && _top == _bottom;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Margin"/> struct with individual values for each side.
    /// </summary>
    /// <param name="left">The left margin value.</param>
    /// <param name="top">The top margin value.</param>
    /// <param name="right">The right margin value.</param>
    /// <param name="bottom">The bottom margin value.</param>
    public Margin(int left, int top, int right, int bottom)
    {
        _top = top;
        _left = left;
        _right = right;
        _bottom = bottom;
        _all = _top == _left && _top == _right && _top == _bottom;
    }

    /// <summary>
    /// Gets or sets the margin value for all sides.
    /// </summary>
    [IgnoreEquality]
    public int All
    {
        get => _all ? _top : -1;
        set
        {
            if (_all && _top == value)
            {
                return;
            }

            _all = true;
            _top = _left = _right = _bottom = value;
        }
    }

    /// <summary>
    /// Gets or sets the bottom margin value.
    /// </summary>
    public int Bottom
    {
        get => _all ? _top : _bottom;
        set
        {
            if (_all || _bottom != value)
            {
                _all = false;
                _bottom = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the left margin value.
    /// </summary>
    public int Left
    {
        get => _all ? _top : _left;
        set
        {
            if (_all || _left != value)
            {
                _all = false;
                _left = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the right margin value.
    /// </summary>
    public int Right
    {
        get => _all ? _top : _right;
        set
        {
            if (_all || _right != value)
            {
                _all = false;
                _right = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the top margin value.
    /// </summary>
    public int Top
    {
        get => _top;
        set
        {
            if (_all || _top != value)
            {
                _all = false;
                _top = value;
            }
        }
    }

    /// <summary>
    /// Gets the sum of the left and right margin values.
    /// </summary>
    [IgnoreEquality]
    public int Horizontal => Left + Right;

    /// <summary>
    /// Gets the sum of the top and bottom margin values.
    /// </summary>
    [IgnoreEquality]
    public int Vertical => Top + Bottom;

    /// <summary>
    /// Gets the total margin size as a <see cref="Size"/>.
    /// </summary>
    [IgnoreEquality]
    public Size Size => new Size(Horizontal, Vertical);

    /// <summary>
    /// Adds two <see cref="Margin"/> values together.
    /// </summary>
    /// <param name="p1">The first margin.</param>
    /// <param name="p2">The second margin.</param>
    /// <returns>The sum of the two margins.</returns>
    public static Margin Add(Margin p1, Margin p2) => p1 + p2;

    /// <summary>
    /// Subtracts one <see cref="Margin"/> value from another.
    /// </summary>
    /// <param name="p1">The first margin.</param>
    /// <param name="p2">The second margin.</param>
    /// <returns>The result of the subtraction.</returns>
    public static Margin Subtract(Margin p1, Margin p2) => p1 - p2;

    /// <summary>
    /// Performs vector addition of two <see cref='Margin'/> objects.
    /// </summary>
    /// <param name="p1">The first margin.</param>
    /// <param name="p2">The second margin.</param>
    /// <returns>The sum of the two margins.</returns>
    public static Margin operator +(Margin p1, Margin p2)
    {
        return new Margin(p1.Left + p2.Left, p1.Top + p2.Top, p1.Right + p2.Right, p1.Bottom + p2.Bottom);
    }

    /// <summary>
    /// Contracts a <see cref='Size'/> by another <see cref='Size'/>.
    /// </summary>
    /// <param name="p1">The first margin.</param>
    /// <param name="p2">The second margin.</param>
    /// <returns>The result of the subtraction.</returns>
    public static Margin operator -(Margin p1, Margin p2)
    {
        return new Margin(p1.Left - p2.Left, p1.Top - p2.Top, p1.Right - p2.Right, p1.Bottom - p2.Bottom);
    }

    /// <summary>
    /// Returns a string representation of the margin.
    /// </summary>
    /// <returns>A string describing the margin.</returns>
    public override string ToString() => $"{{Left={Left},Top={Top},Right={Right},Bottom={Bottom}}}";
}
