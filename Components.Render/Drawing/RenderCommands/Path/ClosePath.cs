using Components.Base.Enums;
using Components.Base.Primitives;
namespace Components.Render.Drawing.RenderCommands.Path;

/// <inheritdoc cref="IPathCommand"/>
/// <summary>
///     Represents a command that closes the current path, by drawing a line from the current cursor position to the start of the path.
/// </summary>
public class ClosePath : IPathCommand
{
    public bool Relative => false;

    public Point End => new Point ();

    public CommandType Type => CommandType.ClosePath;

    public string Shorthand () => "Z";

    public IPathCommand Flip (bool horizontal) => this;
    
    public IPathCommand Reflect () => this;
}
