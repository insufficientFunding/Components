using Components.Base.Enums;
using Components.IO.Xml.Flatten;
using Components.IO.Xml.Primitives;
using Components.Render.Drawing.RenderCommands;
using Components.Render.Drawing.RenderCommands.Path;
using Components.Render.TypeDescription;
using Components.Render.TypeDescription.Conditions;
namespace Components.IO.Xml.Render;

internal class XmlRenderPath : IXmlRenderCommand
{
    public XmlComponentPoint Start { get; internal set; } = null!;
    public double Thickness { get; internal set; }
    public bool Fill { get; internal set; }

    public IList<IPathCommand> Commands { get; internal set; } = null!;

    public IEnumerable<Conditional<IRenderCommand>> Flatten (FlattenContext context)
    {
        List<IPathCommand>? commands = Commands.Select (x =>
        {
            if (context.AutoRotate.Mirror)
                x = x.Reflect ();
            if ((context.AutoRotate.FlipType & FlipType.Horizontal) == FlipType.Horizontal)
                x = x.Flip (true);
            if ((context.AutoRotate.FlipType & FlipType.Vertical) == FlipType.Vertical)
                x = x.Flip (false);
            return x;
        }).ToList ();

        foreach (Conditional<ComponentPoint>? start in Start.Flatten (context))
        {
            RenderPath? command = new RenderPath (
                start.Value,
                commands,
                Thickness,
                Fill);

            yield return new Conditional<IRenderCommand> (command, start.Conditions);
        }
    }
}
