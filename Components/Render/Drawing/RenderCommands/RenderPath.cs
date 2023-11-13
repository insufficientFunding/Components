using Components.DataModels;
using Components.Enums;
using Components.Interfaces.Render;
using Components.Primitives;
using Components.Render.TypeDescription;
namespace Components.Render.Drawing.RenderCommands;

/// <inheritdoc cref="IRenderCommand"/>
/// <summary>
///     Represents a command to draw a path.
/// </summary>
public class RenderPath : IRenderCommand
{
    /// <summary>
    ///     The starting position of the path.
    /// </summary>
    public ComponentPoint Position { get; }
    
    /// <summary>
    ///     The thickness of the path's stroke.
    /// </summary>
    public double StrokeThickness { get; } = 1D;
    
    /// <summary>
    ///     Whether or not the path should be filled.
    /// </summary>
    public bool Fill { get; }
    
    /// <summary>
    ///     A list of commands to draw the path.
    /// </summary>
    public IList<IPathCommand> Commands { get; }

    public RenderCommandType Type => RenderCommandType.Path;

    public RenderPath (ComponentPoint position, IList<IPathCommand> commands, double strokeThickness = 0.1D, bool fill = false)
    {
        Position = position;
        if (strokeThickness > 0D)
            StrokeThickness = strokeThickness;
        Fill = fill;
        Commands = commands;
    }

    public void Render (IDrawingContext context, ILayoutContext layoutContext, LayoutInformation layout)
    {
        Point position = Position.Resolve (layout);
        List<IPathCommand> commands = new List<IPathCommand> (Commands.Count);
        FlipType flipType = layout.GetFlipType ();

        foreach (IPathCommand command in Commands)
        {
            IPathCommand? flippedCommand = command;
            if ((flipType & FlipType.Horizontal) == FlipType.Horizontal)
                flippedCommand = flippedCommand.Flip (true);
            if ((flipType & FlipType.Vertical) == FlipType.Vertical)
                flippedCommand = flippedCommand.Flip (false);
            commands.Add (flippedCommand);
        }

        context.DrawPath (position, commands, StrokeThickness, Fill);
    }
}
