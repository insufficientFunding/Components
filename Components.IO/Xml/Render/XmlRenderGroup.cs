using Components.Base.Enums;
using Components.IO.Xml.Flatten;
using Components.Render.Drawing.RenderCommands;
using Components.Render.TypeDescription;
using Components.Render.TypeDescription.Conditions;
namespace Components.IO.Xml.Render;

internal class XmlRenderGroup : Conditional<List<IXmlRenderCommand>>, IRootFlattenable<RenderDescription>
{
    public XmlRenderGroup (IConditionTreeItem conditions)
        : base (new List<IXmlRenderCommand> (), conditions)
    { }

    public AutoRotateType AutoRotate { get; set; } = AutoRotateType.Off;

    public FlipState AutoRotateFlip { get; set; } = FlipState.None;

    public IEnumerable<RenderDescription> Flatten (FlattenContext context)
    {
        // TODO: Group by/simplify conditions

        ConditionTree? flatConditions = new ConditionTree (ConditionTree.ConditionOperator.AND, context.AncestorConditions, Conditions);

        foreach (Conditional<IRenderCommand>? command in Value.SelectMany (x => x.Flatten (context)))
        {
            ConditionTree? conditions = new ConditionTree (ConditionTree.ConditionOperator.AND, flatConditions, command.Conditions);
            yield return new RenderDescription (conditions, new [] { command.Value });
        }
    }
}
