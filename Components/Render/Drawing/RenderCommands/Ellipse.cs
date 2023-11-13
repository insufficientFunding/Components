using Components.DataModels;
using Components.Enums;
using Components.Interfaces.Render;
using Components.Primitives;
using Components.Render.TypeDescription;
namespace Components.Render.Drawing.RenderCommands;

/// <inheritdoc cref="IRenderCommand"/>
/// <summary>
///     Represents a command to draw an ellipse.
/// </summary>
public class Ellipse : IRenderCommand
{
    /// <summary>
    ///     The position of the ellipse.
    /// </summary>
    public ComponentPoint Position { get; }
    
    /// <summary>
    ///     The radius of the ellipse.
    /// </summary>
    public Size Radius { get; set; }
    
    /// <summary>
    ///     The stroke thickness of the ellipse.
    /// </summary>
    public double StrokeThickness { get; set; } = 1D;
    
    /// <summary>
    ///     Whether or not the ellipse should be filled.
    /// </summary>
    public bool Fill { get; set; }

    public RenderCommandType Type => RenderCommandType.Ellipse;

    public Ellipse (ComponentPoint position, Size size, double strokeThickness = 0.1D, bool fill = false)
    {
        Position = position;
        Radius = size;
        if (strokeThickness > 0D)
            StrokeThickness = strokeThickness;
        Fill = fill;
    }

    public void Render (IDrawingContext context, ILayoutContext layoutContext, LayoutInformation layout)
    {
        Point start = Position.Resolve (layout);

        context.DrawEllipse (start, Radius, StrokeThickness, Fill);
    }
}
