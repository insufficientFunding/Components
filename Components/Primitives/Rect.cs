namespace Components.Primitives;

/// <summary>
///     Represents a positioned rectangle with a width and height.
/// </summary>
public sealed class Rect : IEquatable<Rect>
{
    /// <summary>
    ///     Gets the x-coordinate of the left side of the rectangle.
    /// </summary>
    public double X { get; }

    /// <summary>
    ///     Gets the y-coordinate of the top side of the rectangle.
    /// </summary>
    public double Y { get; }

    /// <summary>
    ///     Gets the width of the rectangle.
    /// </summary>
    public double Width { get; }

    /// <summary>
    ///     Gets the height of the rectangle.
    /// </summary>
    public double Height { get; }

    /// <summary>
    ///     Gets the top left point of the rectangle.
    /// </summary>
    public Point TopLeft => new Point (X, Y);

    /// <summary>
    ///     Gets the bottom right point of the rectangle.
    /// </summary>
    public Point BottomRight => new Point (X + Width, Y + Height);

    /// <summary>
    ///     Gets the size of the rectangle.
    /// </summary>
    public Size Size => new Size (Width, Height);

    #region Constructors
    public Rect ()
        : this (0, 0, 0, 0)
    { }

    public Rect (Point location, Size size)
    {
        X = location.X;
        Y = location.Y;
        Width = size.Width;
        Height = size.Height;
    }

    public Rect (Point topLeft, Point bottomRight)
    {
        X = topLeft.X;
        Y = topLeft.Y;
        Width = bottomRight.X - topLeft.X;
        Height = bottomRight.Y - topLeft.Y;
    }

    public Rect (double x, double y, double width, double height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }
    #endregion

    public Rect Union (Rect other)
    {
        return new Rect (new Point (Math.Min (X, other.X), Math.Min (Y, other.Y)),
                         new Point (Math.Max (BottomRight.X, other.BottomRight.X), Math.Max (BottomRight.Y, other.BottomRight.Y)));
    }

    #region Equality
    public bool Equals (Rect? other)
    {
        if (ReferenceEquals (null, other))
            return false;
        if (ReferenceEquals (this, other))
            return true;

        return X.Equals (other.X) && Y.Equals (other.Y) && Width.Equals (other.Width) && Height.Equals (other.Height);
    }

    public override bool Equals (object? obj)
    {
        if (ReferenceEquals (null, obj))
            return false;
        if (ReferenceEquals (this, obj))
            return true;

        return obj is Rect && Equals ((Rect)obj);
    }

    public static bool operator == (Rect left, Rect right)
    {
        return Equals (left, right);
    }

    public static bool operator != (Rect left, Rect right)
    {
        return !Equals (left, right);
    }

    public override int GetHashCode ()
    {
        unchecked
        {
            var hashCode = X.GetHashCode ();
            hashCode = (hashCode * 397) ^ Y.GetHashCode ();
            hashCode = (hashCode * 397) ^ Width.GetHashCode ();
            hashCode = (hashCode * 397) ^ Height.GetHashCode ();
            return hashCode;
        }
    }
  #endregion
}
