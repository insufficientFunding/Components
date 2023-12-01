using Components.Base.DataModels;
using Components.Render.Drawing;
using Components.Render.Drawing.DrawingContext;
using Components.Render.Drawing.RenderCommands;
using Components.Render.TypeDescription.Conditions;
namespace Components.Render.TypeDescription;

public class RenderDescription : Conditional<IRenderCommand []>
{
    public RenderDescription (IConditionTreeItem conditions, IRenderCommand [] commands)
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
