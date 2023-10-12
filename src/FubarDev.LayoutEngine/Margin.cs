using System;
using System.Drawing;

namespace FubarDev.LayoutEngine;

public struct Margin
{
    private bool _all;
    private int _top;
    private int _left;
    private int _right;
    private int _bottom;

    public static readonly Margin Empty = new Margin(0);
    public Margin(int all)
    {
        _all = true;
        _top = _left = _right = _bottom = all;
    }

    public Margin(int left, int top, int right, int bottom)
    {
        _top = top;
        _left = left;
        _right = right;
        _bottom = bottom;
        _all = _top == _left && _top == _right && _top == _bottom;
    }

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

    public int Horizontal => Left + Right;

    public int Vertical => Top + Bottom;

    public Size Size => new Size(Horizontal, Vertical);

    public static Margin Add(Margin p1, Margin p2) => p1 + p2;

    public static Margin Subtract(Margin p1, Margin p2) => p1 - p2;

    public override bool Equals(object? other)
    {
        if (!(other is Margin otherPadding))
        {
            return false;
        }

        return this == otherPadding;
    }

    /// <summary>
    ///  Performs vector addition of two <see cref='Margin'/> objects.
    /// </summary>
    public static Margin operator +(Margin p1, Margin p2)
    {
        return new Margin(p1.Left + p2.Left, p1.Top + p2.Top, p1.Right + p2.Right, p1.Bottom + p2.Bottom);
    }

    /// <summary>
    ///  Contracts a <see cref='Size'/> by another <see cref='Size'/>.
    /// </summary>
    public static Margin operator -(Margin p1, Margin p2)
    {
        return new Margin(p1.Left - p2.Left, p1.Top - p2.Top, p1.Right - p2.Right, p1.Bottom - p2.Bottom);
    }

    /// <summary>
    ///  Tests whether two <see cref='Margin'/> objects are identical.
    /// </summary>
    public static bool operator ==(Margin p1, Margin p2)
    {
        return p1.Left == p2.Left && p1.Top == p2.Top && p1.Right == p2.Right && p1.Bottom == p2.Bottom;
    }

    /// <summary>
    ///  Tests whether two <see cref='Margin'/> objects are different.
    /// </summary>
    public static bool operator !=(Margin p1, Margin p2) => !(p1 == p2);

    public override int GetHashCode() => HashCode.Combine(Left, Top, Right, Bottom);

    public override string ToString() => $"{{Left={Left},Top={Top},Right={Right},Bottom={Bottom}}}";
}
