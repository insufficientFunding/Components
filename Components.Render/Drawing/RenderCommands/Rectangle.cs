using Components.Base.DataModels;
using Components.Base.Enums;
using Components.Base.Primitives;
using Components.Render.Drawing.DrawingContext;
using Components.Render.TypeDescription;
namespace Components.Render.Drawing.RenderCommands;

/// <inheritdoc cref="IRenderCommand"/>
/// <summary>
///     Represents a command to draw a rectangle.
/// </summary>
public class Rectangle : IRenderCommand
{
    /// <summary>
    ///     The position of the rectangle.
    /// </summary>
    public ComponentPoint Position { get; }
    
    /// <summary>
    ///     The size of the rectangle.
    /// </summary>
    public Size Size { get; }
    
    /// <summary>
    ///     The thickness of the rectangle's stroke.
    /// </summary>
    public double StrokeThickness { get; } = 1D;
    
    /// <summary>
    ///     Whether or not the rectangle should be filled.
    /// </summary>
    public bool Fill { get; }

    public RenderCommandType Type => RenderCommandType.Rectangle;

    public Rectangle (ComponentPoint position, Size size, double strokeThickness = 0.1D, bool fill = false)
    {
        Position = position;
        Size = size;
        if (strokeThickness > 0D)
            StrokeThickness = strokeThickness;
        Fill = fill;
    }

    public void Render (IDrawingContext context, ILayoutContext layoutContext, LayoutInformation layout)
    {
        Point start = Position.Resolve (layout);

        Rect drawRect = new Rect (start, Size);

        switch (layout.GetFlipType ())
        {
            case FlipType.Horizontal:
                drawRect = new Rect (drawRect.X - Size.Width, drawRect.Y, Size.Width, Size.Height);
                break;
            case FlipType.Vertical:
                drawRect = new Rect (drawRect.X, drawRect.Y - Size.Height, Size.Width, Size.Height);
                break;
            case FlipType.Both:
                drawRect = new Rect (drawRect.X - Size.Width, drawRect.Y - Size.Height, Size.Width, Size.Height);
                break;
        }

        context.DrawRectangle (drawRect.TopLeft, drawRect.Size, StrokeThickness, Fill);
    }
}
