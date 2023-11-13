using Components.DataModels;
using Components.Enums;
namespace Components.Interfaces.Render;

/// <summary>
///     Represents an svg render command.
/// </summary>
public interface IRenderCommand
{
    /// <summary>
    ///     Gets the type of the render command.
    /// </summary>
    RenderCommandType Type { get; }

    /// <summary>
    ///     Renders the command to the given <see cref="IDrawingContext"/>
    /// </summary>
    /// <param name="context">The drawing context.</param>
    /// <param name="layoutContext">The layout context.</param>
    /// <param name="layout">The components layout information.</param>
    void Render (IDrawingContext context, ILayoutContext layoutContext, LayoutInformation layout);
}
