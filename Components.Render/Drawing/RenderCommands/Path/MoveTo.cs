using Components.Base.Enums;
using Components.Base.Primitives;
using System.Globalization;
namespace Components.Render.Drawing.RenderCommands.Path;

/// <inheritdoc cref="IPathCommand"/>
/// <summary>
///     Represents a command that moves the cursor to a specified point.
/// </summary>
public class MoveTo : IPathCommand
{
    public bool Relative { get; }

    public Point End { get; }

    public CommandType Type => CommandType.MoveTo;

    public MoveTo (Point end, bool relative = false)
    {
        End = end;
        Relative = relative;
    }

    public string Shorthand ()
    {
        string prefix = Relative ? "m " : "M ";

        return prefix + End.X.ToString (CultureInfo.InvariantCulture) + ", " + End.Y.ToString (CultureInfo.InvariantCulture);
    }

    public IPathCommand Flip (bool horizontal)
    {
        if (horizontal)
            return new MoveTo (new Point (-End.X, End.Y), Relative);

        return new MoveTo (new Point (End.X, -End.Y), Relative);
    }

    public IPathCommand Reflect ()
    {
        return new MoveTo (new Point (End.Y, End.X), Relative);
    }
}
