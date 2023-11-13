using Components.Interfaces.Render;
using Components.Render.Drawing.RenderCommands;
using Components.Render.TypeDescription;
using Components.Render.TypeDescription.Conditions;
using Components.Xml.Flatten;
using Components.Xml.Primitives;
namespace Components.Xml.Render;

internal class XmlLineCommand : IXmlRenderCommand
{
    public XmlComponentPoint Start { get; set; } = null!;
    public XmlComponentPoint End { get; set; } = null!;
    public double Thickness { get; set; }

    public IEnumerable<Conditional<IRenderCommand>> Flatten (FlattenContext context)
    {
        foreach (Conditional<ComponentPoint>? start in Start.Flatten (context))
        {
            foreach (Conditional<ComponentPoint>? end in End.Flatten (context))
            {
                Line? command = new Line (start.Value, end.Value, Thickness);
                ConditionTree? conditions = new ConditionTree (
                    ConditionTree.ConditionOperator.AND,
                    start.Conditions,
                    end.Conditions);

                yield return new Conditional<IRenderCommand> (command, conditions);
            }
        }
    }
}
