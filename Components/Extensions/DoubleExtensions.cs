using System.Globalization;
namespace Components.Extensions;

public static class DoubleExtensions
{
    /// <summary>
    ///     Parses a string to a double using the invariant culture.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <returns>The parsed string as a double, allowing for decimal points.</returns>
    public static double ParseDecimal (this string value)
    {
        return double.Parse (value ?? "0", NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///     Tries to parse a string to a double using the invariant culture.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <param name="result">The parsed string as a double, allowing for decimal points.</param>
    /// <returns>True if the string was parsed successfully, false otherwise.</returns>
    public static bool TryParseDecimal (this string? value, out double result)
    {
        return double.TryParse (value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out result);
    }
}
