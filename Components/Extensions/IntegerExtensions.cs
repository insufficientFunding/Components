using System.Globalization;
namespace Components.Extensions;

public static class IntegerExtensions
{
    /// <summary>
    ///     Parses a string to an integer using the invariant culture.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <returns>The parsed string as an integer.</returns>
    public static double ParseInt (this string value)
    {
        return int.Parse (value ?? "0", CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///     Tries to parse a string to an integer using the invariant culture.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <param name="result">The parsed string as an integer.</param>
    /// <returns>True if the string was parsed successfully, false otherwise.</returns>
    public static bool TryParseInt (this string? value, out int result)
    {
        return int.TryParse (value, CultureInfo.InvariantCulture, out result);
    }

}
