using System.Globalization;
namespace Components.Primitives;

/// <summary>
///     A struct representing a size in 2D space.
/// </summary>
public class Size
{
    /// <summary>
    ///     The width of the size.
    /// </summary>
    public double Width { get; }

    /// <summary>
    ///     The height of the size.
    /// </summary>
    public double Height { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Size"/> struct.
    /// </summary>
    /// <param name="width">The width of the size.</param>
    /// <param name="height">The height of the size.</param>
    public Size (double width, double height)
    {
        Width = width;
        Height = height;
    }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="Size"/> struct.
    /// </summary>
    public Size ()
        : this (0, 0)
    { }

    #region Math and transformations
    /// <summary>
    ///     Adds the given size to this size.
    /// </summary>
    /// <param name="size">The size to add.</param>
    /// <returns>The sum of the two sizes.</returns>
    public Size Add (Size size)
    {
        return new Size (Width + size.Width, Height + size.Height);
    }

    /// <summary>
    ///     Multiplies this size by the given size.
    /// </summary>
    /// <param name="scale">The size to multiply by.</param>
    /// <returns>The product of the two sizes.</returns>
    public Size Multiply (Size scale)
    {
        return new Size (Width * scale.Width, Height * scale.Height);
    }

    /// <summary>
    ///     Multiplies this size by the given double.
    /// </summary>
    /// <param name="scale">The double to multiply by.</param>
    /// <returns>The product of the size and the double.</returns>
    public Size Multiply (double scale)
    {
        return new Size (Width * scale, Height * scale);
    }

    /// <summary>
    ///     Divides this size by the given size.
    /// </summary>
    /// <param name="scale">The size to divide by.</param>
    /// <returns>The quotient of the two sizes.</returns>
    public Size Divide (Size scale)
    {
        return new Size (Width / scale.Width, Height / scale.Height);
    }

    /// <summary>
    ///     Divides this size by the given double.
    /// </summary>
    /// <param name="scale">The double to divide by.</param>
    /// <returns>The quotient of the size and the double.</returns>
    public Size Divide (double scale)
    {
        return new Size (Width / scale, Height / scale);
    }
    #endregion


    #region Operators
    public bool Equals (Size other)
    {
        return Width.Equals (other.Width) && Height.Equals (other.Height);
    }

    public override bool Equals (object? obj)
    {
        if (ReferenceEquals (null, obj)) return false;

        return obj is Size other && Equals (other);
    }

    public static bool operator == (Size left, Size right)
    {
        return left.Equals (right);
    }

    public static bool operator != (Size left, Size right)
    {
        return !left.Equals (right);
    }
    #endregion

    public static Size Parse (string toParse)
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
        return new Size (xParsed, yParsed);
    }

    public static bool TryParse (string toParse, out Size? size)
    {
        // Initialize the size to null
        size = null;

        // Try to parse the string
        try
        {
            size = Parse (toParse);

            return true;
        }

        // If the string is not in the correct format, log the exception and return false
        catch (FormatException innerException)
        {
            Console.WriteLine (innerException.Message);
            Console.WriteLine (innerException.StackTrace);

            return false;
        }
    }

    public override int GetHashCode ()
    {
        return HashCode.Combine (Width, Height);
    }
}
