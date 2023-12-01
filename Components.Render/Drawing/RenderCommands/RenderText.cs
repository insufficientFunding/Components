using Components.Base.DataModels;
using Components.Base.Enums;
using Components.Base.Primitives;
using Components.Render.Drawing.DrawingContext;
using Components.Render.Drawing.Extensions;
using Components.Render.Text;
using Components.Render.TypeDescription;
namespace Components.Render.Drawing.RenderCommands;

/// <inheritdoc cref="IRenderCommand"/>
/// <summary>
///     Represents a command to draw text.
/// </summary>
public class RenderText : IRenderCommand
{
    /// <summary>
    ///     The position of the text.
    /// </summary>
    public ComponentPoint Position { get; }

    /// <summary>
    ///     The alignment of the text.
    /// </summary>
    public TextAlignment Alignment { get; }

    /// <summary>
    ///     The font weight of the text.
    /// </summary>
    public FontWeight Weight { get; }

    /// <summary>
    ///     The rotation of the text.
    /// </summary>
    public TextRotation Rotation { get; }

    /// <summary>
    ///     The text runs of the text.
    /// </summary>
    public List<TextRun> TextRuns { get; }

    public RenderCommandType Type => RenderCommandType.Text;

    /// <summary>
    ///     Creates a new instance of the <see cref="RenderText"/> class.
    /// </summary>
    /// <param name="position">The text's position.</param>
    /// <param name="alignment">The text's alignment.</param>
    /// <param name="weight">The text's font weight.</param>
    /// <param name="rotation">The text's rotation.</param>
    /// <param name="textRuns">The text's TextRuns.</param>
    public RenderText (ComponentPoint position, TextAlignment alignment, FontWeight weight, TextRotation rotation, IEnumerable<TextRun> textRuns)
    {
        Position = position;
        Alignment = alignment;
        Weight = weight;
        Rotation = rotation;
        TextRuns = new List<TextRun> (textRuns);
    }

    public void Render (IDrawingContext context, ILayoutContext layoutContext, LayoutInformation layout)
    {
        Point renderLocation = Position.Resolve (layout);

        TextAlignment textAlignment = Alignment.Flip (layout.GetFlipType ());

        List<TextRun> renderTextRuns = new List<TextRun> (TextRuns.Count);
        // Build runs
        foreach (TextRun run in TextRuns)
        {
            // Resolve value
            string renderValue;
            if (run.Text!.StartsWith ("$"))
                renderValue = layoutContext.GetFormattedVariable (run.Text)!;
            else
                renderValue = run.Text;

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex (@"\\[uU]([0-9A-F]{4})");
            renderValue = regex.Replace (renderValue, match => ((char)int.Parse (match.Value.Substring (2), System.Globalization.NumberStyles.HexNumber)).ToString ());

            renderTextRuns.Add (new TextRun (renderValue, run.Formatting));
        }

        context.DrawText (renderLocation, textAlignment, Weight, (int)Rotation * 90, renderTextRuns);

    }

    public override bool Equals (object? obj)
    {
        if (obj is not RenderText o)
            return false;

        bool textRunsEqual = TextRuns.SequenceEqual (o.TextRuns);

        return Position.Equals (o.Position)
               && Alignment.Equals (o.Alignment)
               && textRunsEqual;
    }

    public override int GetHashCode ()
    {
        return Position.GetHashCode ()
               ^ Alignment.GetHashCode ()
               ^ TextRuns.GetHashCode ();
    }

    public object Clone ()
    {
        return new RenderText (Position, Alignment, Weight, Rotation, TextRuns);
    }
}
