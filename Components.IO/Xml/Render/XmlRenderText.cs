using Components.Base.Enums;
using Components.IO.Xml.Flatten;
using Components.IO.Xml.Primitives;
using Components.Render.Drawing.RenderCommands;
using Components.Render.Text;
using Components.Render.TypeDescription;
using Components.Render.TypeDescription.Conditions;
namespace Components.IO.Xml.Render;

internal class XmlRenderText : IXmlRenderCommand
{
    public XmlComponentPoint Position { get; set; } = null!;

    public TextAlignment Alignment { get; set; }

    public FontWeight Weight { get; set; }

    public TextRotation Rotation { get; set; }

    public List<TextRun> TextRuns { get; } = new List<TextRun> ();

    public virtual IEnumerable<Conditional<IRenderCommand>> Flatten (FlattenContext context)
    {
        foreach (Conditional<ComponentPoint>? location in Position.Flatten (context))
        {
            RenderText? command = new RenderText (
                location.Value,
                Alignment,
                Weight,
                Rotation,
                TextRuns);

            yield return new Conditional<IRenderCommand> (command, location.Conditions);
        }
    }
}
