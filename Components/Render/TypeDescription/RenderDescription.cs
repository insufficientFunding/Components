using Components.DataModels;
using Components.Interfaces.Conditions;
using Components.Interfaces.Render;
using Components.Interfaces.TypeDescription;
using Components.Render.TypeDescription.Conditions;
namespace Components.Render.TypeDescription;

public class RenderDescription : Conditional<IRenderCommand []>, IRenderDescription
{
    internal protected RenderDescription (IConditionTreeItem conditions, IRenderCommand [] commands)
        : base (commands, conditions)
    { }

    public void Render (IDrawingContext context, ILayoutContext layoutContext, LayoutInformation layout)
    {
        foreach (IRenderCommand? command in Value)
        {
            command.Render (context, layoutContext, layout);
        }
    }
}
