using System.Diagnostics;
using System.Globalization;
namespace Components.Base.Primitives;

/// <summary>
///     A struct representing a point in 2D space.
/// </summary>
public class Point
{
    /// <summary>
    ///     The X coordinate of the point.
    /// </summary>
    public double X { get; }

    /// <summary>
    ///     The Y coordinate of the point.
    /// </summary>
    public double Y { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Point"/> class.
    /// </summary>
    /// <param name="x">The X coordinate of the point.</param>
    /// <param name="y">The Y coordinate of the point.</param>
    public Point (double x, double y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Point"/> class.
    /// </summary>
    public Point ()
        : this (0, 0)
    { }

    #region Math and transformations
    /// <summary>
    ///     Adds the given point to this point.
    /// </summary>
    /// <param name="point">The point to add.</param>
    /// <returns>The sum of the two points.</returns>
    public Point Add (Point point)
    {
        return new Point (X + point.X, Y + point.Y);
    }

    /// <summary>
    ///     Adds the given double to both the X and Y coordinates of this point.
    /// </summary>
    /// <param name="length">The length to add.</param>
    /// <returns>The sum of the point and the length.</returns>
    public Point Move (double length)
    {
        return new Point (X + length, Y + length);
    }

    /// <summary>
    ///     Multiplies this point by the given point.
    /// </summary>
    /// <param name="scale">The point to multiply by.</param>
    /// <returns>The product of the two points.</returns>
    public Point Multiply (Point scale)
    {
        return new Point (X * scale.X, Y * scale.Y);
    }

    /// <summary>
    ///     Multiplies this point by the given scale.
    /// </summary>
    /// <param name="scale">The scale to multiply by.</param>
    /// <returns>The product of the point and the scale.</returns>
    public Point Multiply (double scale)
    {
        return new Point (X * scale, Y * scale);
    }

    /// <summary>
    ///     Divides this point by the given point.
    /// </summary>
    /// <param name="scale">The point to divide by.</param>
    /// <returns>The quotient of the two points.</returns>
    public Point Divide (Point scale)
    {
        return new Point (X / scale.X, Y / scale.Y);
    }

    /// <summary>
    ///     Divides this point by the given scale.
    /// </summary>
    /// <param name="scale">The scale to divide by.</param>
    /// <returns>The quotient of the point and the scale.</returns>
    public Point Divide (double scale)
    {
        return new Point (X / scale, Y / scale);
    }

    /// <summary>
    ///     Returns a copy of this point with the <b>X</b> coordinate set to the given value.
    /// </summary>
    /// <param name="x">The new <b>X</b> coordinate.</param>
    /// <returns>A copy of this point with the <b>X</b> coordinate set to the given value.</returns>
    public Point WithX (double x)
    {
        return new Point (x, Y);
    }

    /// <summary>
    ///     Returns a copy of this point with the <b>Y</b> coordinate set to the given value.
    /// </summary>
    /// <param name="y">The new <b>Y</b> coordinate.</param>
    /// <returns>A copy of this point with the <b>Y</b> coordinate set to the given value.</returns>
    public Point WithY (double y)
    {
        return new Point (X, y);
    }
    #endregion

    #region Operators
    public bool Equals (Point other)
    {
        return X.Equals (other.X) && Y.Equals (other.Y);
    }

    public override bool Equals (object? obj)
    {
        if (ReferenceEquals (null, obj)) return false;

        return obj is Point other && Equals (other);
    }

    public static bool operator == (Point left, Point right)
    {
        return left.Equals (right);
    }

    public static bool operator != (Point left, Point right)
    {
        return !left.Equals (right);
    }
    #endregion

    /// <summary>
    ///     Parses a string into a <see cref="Point"/>.
    /// </summary>
    /// <param name="toParse">The string to parse.</param>
    /// <returns>The parsed <see cref="Point"/>.</returns>
    /// <exception cref="FormatException">Thrown if the string is not in the correct format.</exception>
    public static Point Parse (string toParse)
    {
        // Remove all spaces
        toParse = toParse.Replace (" ", string.Empty);

        // Find the comma separator, and check if it exists
        int commaIndex = toParse.IndexOf (",", StringComparison.Ordinal);
        if (commaIndex == -1)
            throw new FormatException ($"Invalid string '{toParse}' for Point.");

        // Split the string into its X and Y components
        string xString = toParse.Substring (0, commaIndex);
        string yString = toParse.Substring (commaIndex + 1);

        // Parse the strings into doubles
        double xParsed = double.Parse (xString, CultureInfo.InvariantCulture);
        double yParsed = double.Parse (yString, CultureInfo.InvariantCulture);

        // Return the parsed point
        return new Point (xParsed, yParsed);
    }

    /// <summary>
    ///     Tries to parse a string into a <see cref="Point"/>.
    /// </summary>
    /// <param name="toParse">The string to parse.</param>
    /// <param name="point">The parsed <see cref="Point"/>, null if the parse was unsuccessful.</param>
    /// <returns>True if the string was parsed successfully, false otherwise.</returns>
    public static bool TryParse (string toParse, out Point? point)
    {
        // Initialize the point to null
        point = null;

        // Try to parse the string
        try
        {
            point = Parse (toParse);

            return true;
        }

        // If the string is not in the correct format, log the exception and return false
        catch (FormatException innerException)
        {
            Debug.WriteLine(innerException.Message);
            Debug.WriteLine(innerException.StackTrace);

            return false;
        }
    }

    /// <summary>
    ///     Returns the hash code for this <see cref="Point"/>.
    /// </summary>
    /// <returns>The hash code for this <see cref="Point"/>.</returns>
    public override int GetHashCode ()
    {
        unchecked
        {
            return (X.GetHashCode () * 397) ^ Y.GetHashCode ();
        }
    }
}
