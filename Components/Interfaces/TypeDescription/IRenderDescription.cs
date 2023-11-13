using Components.DataModels;
using Components.Interfaces.Conditions;
using Components.Interfaces.Render;
namespace Components.Interfaces.TypeDescription;

/// <summary>
///     Defines a conditional array of render commands, which can be used to render a component.
/// </summary>
public interface IRenderDescription : IConditional<IRenderCommand []>
{
    /// <summary>
    ///     Renders the component to the drawing context using the layout context and layout information.
    /// </summary>
    /// <param name="context">The drawing context to render to.</param>
    /// <param name="layoutContext">The layout context to use.</param>
    /// <param name="layout">The layout information of the component, used to define it's position, size, and rotation.</param>
    void Render (IDrawingContext context, ILayoutContext layoutContext, LayoutInformation layout);
}
