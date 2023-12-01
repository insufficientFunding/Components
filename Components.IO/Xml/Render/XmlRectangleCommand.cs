using Components.Base.Enums;
using Components.Base.Primitives;
using Components.IO.Xml.Flatten;
using Components.IO.Xml.Primitives;
using Components.Render.Drawing.RenderCommands;
using Components.Render.TypeDescription;
using Components.Render.TypeDescription.Conditions;
namespace Components.IO.Xml.Render;

internal class XmlRectangleCommand : IXmlRenderCommand
{
    public XmlComponentPoint Position { get; set; } = null!;
    public Size Size { get; set; }
    public double StrokeThickness { get; set; }
    public bool Fill { get; set; }

    public virtual IEnumerable<Conditional<IRenderCommand>> Flatten (FlattenContext context)
    {
        double width = context.AutoRotate.Mirror ? Size.Height : Size.Width;
        double height = context.AutoRotate.Mirror ? Size.Width : Size.Height;

        foreach (Conditional<ComponentPoint>? locationConditional in Position.Flatten (context))
        {
            ComponentPoint? location = locationConditional.Value;
            if ((context.AutoRotate.FlipType & FlipType.Horizontal) != 0)
            {
                location.Offset = new Point (location.Offset.X - width, location.Offset.Y);
            }
            if ((context.AutoRotate.FlipType & FlipType.Vertical) != 0)
            {
                location.Offset = new Point (location.Offset.X, location.Offset.Y - height);
            }

            Rectangle? command = new Rectangle (
                location,
                new Size (width, height),
                StrokeThickness,
                Fill);

            yield return new Conditional<IRenderCommand> (command, locationConditional.Conditions);
        }
    }
}
