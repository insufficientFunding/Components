namespace Components.VisualEditor.Parsers;

public static class IntParser
{
    public static bool TryParseInt (this string? value, out int result)
    {
        return int.TryParse (value, out result);
    }
    
    public static int ParseInt (this string? value, int fallbackValue = 0)
    {
        return value.TryParseInt (out int result) ? result : fallbackValue;
    }
}
