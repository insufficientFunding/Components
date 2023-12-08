using System.Text.Json;
namespace Components.VisualEditor.Serialization.Extensions;

public static class Utf8JsonReaderExtensions
{
    public static string ReadString (this ref Utf8JsonReader reader, string fallback = "")
    {
        if (reader.TokenType == JsonTokenType.PropertyName)
            reader.Read ();

        if (reader.TokenType != JsonTokenType.String)
            return fallback;

        return reader.GetString () ?? fallback;
    }

    public static bool ReadBoolean (this ref Utf8JsonReader reader)
    {
        if (reader.TokenType == JsonTokenType.PropertyName)
            reader.Read ();

        if (reader.TokenType != JsonTokenType.False && reader.TokenType != JsonTokenType.True)
            return false;

        return reader.GetBoolean ();
    }
}
