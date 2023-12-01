using Components.Base.Primitives;
using Components.IO.Xml.Flatten;
using Components.IO.Xml.Primitives;
using Components.Render.Drawing.RenderCommands;
using Components.Render.TypeDescription;
using Components.Render.TypeDescription.Conditions;
namespace Components.IO.Xml.Render;

internal class XmlEllipseCommand : IXmlRenderCommand
{
    public XmlComponentPoint Position { get; set; } = null!;
    public Size Size { get; set; }
    public double StrokeThickness { get; set; }
    public bool Fill { get; set; }

    public IEnumerable<Conditional<IRenderCommand>> Flatten (FlattenContext context)
    {
        foreach (Conditional<ComponentPoint>? centre in Position.Flatten (context))
        {
            Ellipse? command = new Ellipse (
                centre.Value,
                Size,
                StrokeThickness,
                Fill);

            yield return new Conditional<IRenderCommand> (command, centre.Conditions);
        }
    }
}
