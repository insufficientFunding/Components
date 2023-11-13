using Components.Enums;
using Components.Interfaces.Render;
using Components.Primitives;
using System.Globalization;
namespace Components.Render.Drawing.RenderCommands.Path;

/// <inheritdoc cref="IPathCommand"/>
/// <summary>
///     Represents a command that draws a line from the current cursor position to a specified point.
/// </summary>
public class LineTo : IPathCommand
{
    public Point End { get; }
    public bool Relative { get; }

    public CommandType Type => CommandType.LineTo;

    public LineTo (Point end, bool relative = false)
    {
        End = end;
        Relative = relative;
    }

    public string Shorthand ()
    {
        string prefix = Relative ? "l " : "L ";

        return prefix + End.X.ToString (CultureInfo.InvariantCulture) + ", " + End.Y.ToString (CultureInfo.InvariantCulture);
    }

    public IPathCommand Flip (bool horizontal)
    {
        if (horizontal)
            return new LineTo (new Point (-End.X, End.Y), Relative);

        return new LineTo (new Point (End.X, -End.Y), Relative);
    }

    public IPathCommand Reflect ()
    {
        return new LineTo (new Point (End.Y, End.X), Relative);
    }
}
