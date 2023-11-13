using Components.Render.TypeDescription.Conditions;
using Components.Xml.Flatten;
namespace Components.Xml.Primitives;

internal class XmlComponentPointOffset : IXmlComponentPointOffset
{
    public double Offset { get; }

    public OffsetAxis Axis { get; }

    public XmlComponentPointOffset (double offset, OffsetAxis axis)
    {
        Offset = offset;
        Axis = axis;
    }

    public IEnumerable<Conditional<ComponentPointOffset>> Flatten (FlattenContext context)
    {
        ComponentPointOffset? offset = new ComponentPointOffset { Axis = Axis, Offset = Offset };

        yield return new Conditional<ComponentPointOffset> (offset, ConditionTree.Empty);
    }
}
