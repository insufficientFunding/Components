using Components.Base.DataModels;
using Components.Base.Enums;
using Components.Base.Primitives;
using Components.Render.Drawing.DrawingContext;
using Components.Render.TypeDescription;
namespace Components.Render.Drawing.RenderCommands;

/// <inheritdoc cref="IRenderCommand"/>
/// <summary>
///     Represents a command to draw a line between two points.
/// </summary>
public class Line : IRenderCommand
{
    /// <summary>
    ///     The start point of the line.
    /// </summary>
    public ComponentPoint Start { get; }
    
    /// <summary>
    ///     The end point of the line.
    /// </summary>
    public ComponentPoint End { get; }
    
    /// <summary>
    ///     The thickness of the line.
    /// </summary>
    public double Thickness { get; } = 1D;

    public RenderCommandType Type => RenderCommandType.Line;

    public Line (ComponentPoint start, ComponentPoint end, double thickness = 0.1D)
    {
        Start = start;
        End = end;
        if (thickness > 0D)
            Thickness = thickness;
    }

    public void Render (IDrawingContext context, ILayoutContext layoutContext, LayoutInformation layout)
    {
        Point start = Start.Resolve (layout);
        Point end = End.Resolve (layout);

        context.DrawLine (start, end, Thickness);
    }
}
