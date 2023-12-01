using System.Globalization;
namespace Components.VisualEditor.Parsers;

public static class DoubleParser
{
    /// <summary>
    ///     Tries to parse the given <see cref="string"/> as a <see cref="double"/>.
    /// </summary>
    /// <param name="value">The nullable <see cref="string"/> to parse.</param>
    /// <param name="result">The parsed <see cref="double"/>, this value is <b>0</b> if parsing failed.</param>
    /// <returns><b>true</b> if parsing was successful, <b>false</b> otherwise.</returns>
    public static bool TryParseDouble (this string? value, out double result)
    {
        return double.TryParse (value, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
    }
    /// <summary>
    ///     Parses the given <see cref="string"/> as a <see cref="double"/>, and
    ///     returns the given <paramref name="fallbackValue"/> if parsing failed.
    /// </summary>
    /// <param name="value">The nullable <see cref="string"/> to parse.</param>
    /// <param name="fallbackValue">The value to return if parsing failed, defaults to <b><c>0</c></b> if not supplied</param>
    /// <returns></returns>
    public static double ParseDouble (this string? value, double fallbackValue = 0)
    {
        return value.TryParseDouble (out double result) ? result : fallbackValue;
    }
}
