namespace Components.Text;

/// <summary>
///     Represents a block of text.
/// </summary>
public class TextRun
{
    /// <summary>
    ///     How the text should be formatted.
    /// </summary>
    public TextRunFormatting Formatting { get; set; }

    /// <summary>
    ///     The text to render.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    ///     Creates a new text run with the specified parameters.
    /// </summary>
    /// <param name="text">The text to render.</param>
    /// <param name="formatting">How the text should be formatted.</param>
    public TextRun (string? text, TextRunFormatting formatting)
    {
        Text = text;
        Formatting = formatting;
    }

    public override bool Equals (object? obj)
    {
        // If parameter is null return false.
        if (obj is null)
            return false;

        // If parameter cannot be cast to TextRun return false.
        TextRun? o = obj as TextRun;
        if (o is null)
            return false;

        // Return true if the fields match:
        return Formatting.Equals (o.Formatting)
               && Text!.Equals (o.Text);
    }

    public override int GetHashCode ()
    {
        return Formatting.GetHashCode ()
               ^ Text!.GetHashCode ();
    }
}
