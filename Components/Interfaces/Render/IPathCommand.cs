using Components.Enums;
using Components.Primitives;
namespace Components.Interfaces.Render;

/// <summary>
///     Represents a command in an svg path.
/// </summary>
public interface IPathCommand
{
    /// <summary>
    ///     Gets a boolean value indicating whether the command is relative to the previous command's end point, if not the command is relative to the component canvas' origin.
    /// </summary>
    bool Relative { get; }

    /// <summary>
    ///     Gets the end point of the command.
    /// </summary>
    Point End { get; }

    /// <summary>
    ///     Gets the type of the command.
    /// </summary>
    CommandType Type { get; }

    /// <summary>
    ///     Gets the shorthand representation of the command.
    /// </summary>
    /// <returns>A string representation of the command, equivalent to the shorthand representation of the command in an svg path.</returns>
    string Shorthand ();

    /// <summary>
    ///     Flips the command horizontally or vertically.
    /// </summary>
    /// <param name="horizontal">A boolean value indicating whether the command should be flipped horizontally.</param>
    /// <returns>A new <see cref="IPathCommand"/> instance with the flipped command.</returns>
    IPathCommand Flip (bool horizontal);

    /// <summary>
    ///     Reflects the command, by swapping the X and Y coordinates.
    /// </summary>
    /// <returns>A new <see cref="IPathCommand"/> instance with the X and Y coordinates swapped.</returns>
    IPathCommand Reflect ();
}
