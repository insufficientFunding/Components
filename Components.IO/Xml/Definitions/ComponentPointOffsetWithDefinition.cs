using Components.IO.Xml.Flatten;
using Components.IO.Xml.Primitives;
using Components.Render.TypeDescription.Conditions;
namespace Components.IO.Xml.Definitions;

internal class ComponentPointOffsetWithDefinition : IXmlComponentPointOffset
{
    public bool Negative { get; }

    public ConditionalCollection<double> Values { get; }

    public OffsetAxis Axis { get; }

    public ComponentPointOffsetWithDefinition (bool negative, ConditionalCollection<double> values, OffsetAxis axis)
    {
        Negative = negative;
        Values = values;
        Axis = axis;
    }

    public IEnumerable<Conditional<ComponentPointOffset>> Flatten (FlattenContext context)
    {
        foreach (Conditional<double>? value in Values)
        {
            ComponentPointOffset offset = new ComponentPointOffset
            {
                Axis = Axis,
                Offset = value.Value * (Negative ? -1 : 1),
            };
            yield return new Conditional<ComponentPointOffset> (offset, value.Conditions);
        }
    }
}
