using Components.Enums;
using Components.Primitives;
using Components.Text;
namespace Components.Interfaces.Render;

/// <summary>
///     Represents a drawing context, used to draw shapes and text.
/// </summary>
public interface IDrawingContext : IDisposable
{
    /// <summary>
    ///     Draws a line from <paramref name="start"/> to <paramref name="end"/> with the specified <paramref name="thickness"/>.
    /// </summary>
    /// <param name="start">The line's starting point</param>
    /// <param name="end">The line's end point.</param>
    /// <param name="thickness">The line's thickness.</param>
    void DrawLine (Point start, Point end, double thickness);

    /// <summary>
    ///     Draws a rectangle with the specified parameters.
    /// </summary>
    /// <param name="start">The rectangle's origin point.</param>
    /// <param name="size">The rectangle's size</param>
    /// <param name="thickness">The rectangle's stroke thickness.</param>
    /// <param name="fill">If the rectangle should be filled, default is false.</param>
    void DrawRectangle (Point start, Size size, double thickness, bool fill = false);

    /// <summary>
    ///     Draws an ellipse at the specified point, with the specified radius.
    /// </summary>
    /// <param name="center">The ellipse's starting point.</param>
    /// <param name="radius">The ellipse's radius.</param>
    /// <param name="thickness">The ellipse's stroke thickness.</param>
    /// <param name="fill">If the ellipse should be filled, default is false.</param>
    void DrawEllipse (Point center, Size radius, double thickness, bool fill = false);

    /// <summary>
    ///     Draws a path with the specified parameters.
    /// </summary>
    /// <param name="start">The path's starting point. <i>(<b>NOTE:</b> this parameter is currently an empty Vector, so you should position it with an initial MoveTo command)</i></param>
    /// <param name="commands">A List containing the path's <see cref="IPathCommand"/> instances.</param>
    /// <param name="thickness">The path's stroke thickness.</param>
    /// <param name="fill">If the path should be filled, default is false.</param>
    void DrawPath (Point start, List<IPathCommand> commands, double thickness, bool fill = false);

    /// <summary>
    ///     Draws text at the specified anchor point, with the specified alignment.
    /// </summary>
    /// <param name="anchor">The text's anchor point.</param>
    /// <param name="alignment">The text's alignment.</param>
    /// <param name="weight">The text's font.</param>
    /// <param name="rotation">The text's rotation in degrees.</param>
    /// <param name="textRuns">A List containing the text's <see cref="TextRun"/> instances.</param>
    void DrawText (Point anchor, TextAlignment alignment, FontWeight weight, double rotation, IList<TextRun> textRuns);
}
