using Components.Interfaces.Conditions;
using Components.Primitives;
using Components.Render.TypeDescription;
using Components.Render.TypeDescription.Conditions;
using Components.Xml.Flatten;
namespace Components.Xml.Primitives;

internal class XmlComponentPoint : IFlattenable<ComponentPoint>
{
    public ComponentPosition RelativeToX { get; }

    public ComponentPosition RelativeToY { get; }

    public IReadOnlyList<IXmlComponentPointOffset> Offsets { get; }

    public XmlComponentPoint (
        ComponentPosition relativeToX,
        ComponentPosition relativeToY,
        IEnumerable<IXmlComponentPointOffset> offsets)
    {
        RelativeToX = relativeToX;
        RelativeToY = relativeToY;
        Offsets = offsets.ToList ();
    }

    public IEnumerable<Conditional<ComponentPoint>> Flatten (FlattenContext context)
    {
        IEnumerable<IEnumerable<Conditional<ComponentPointOffset>>>? allOffsets = Offsets.Select (x => x.Flatten (context));

        foreach (IEnumerable<Conditional<ComponentPointOffset>>? offsets in allOffsets.CartesianProduct ())
        {
            Conditional<ComponentPointOffset> []? conditionals = offsets as Conditional<ComponentPointOffset> [] ?? offsets.ToArray ();
            double xOffset = conditionals.Where (X => X.Value.Axis == OffsetAxis.X != context.AutoRotate.Mirror).Sum (X => X.Value.Offset);
            double yOffset = conditionals.Where (X => X.Value.Axis == OffsetAxis.Y != context.AutoRotate.Mirror).Sum (X => X.Value.Offset);

            ComponentPoint? point = new ComponentPoint (RelativeToX, RelativeToY, new Point (xOffset, yOffset)).Flip (context.AutoRotate.FlipType, context.AutoRotate.FlipState);

            IConditionTreeItem? conditions = ConditionTreeBuilder.And (conditionals.Select (x => x.Conditions));

            yield return new Conditional<ComponentPoint> (point, conditions);
        }
    }

    public override string ToString ()
    {
        return $"({RelativeToX}, {RelativeToY}), Offsets: {string.Join (", ", Offsets)}";
    }
}
public static class EnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> CartesianProduct<T> (this IEnumerable<IEnumerable<T>> sequences)
    {
        if (sequences == null!)
            return null!;

        IEnumerable<IEnumerable<T>> emptyProduct = new [] { Enumerable.Empty<T> () };

        return sequences.Aggregate (
            emptyProduct,
            (accumulator, sequence) => accumulator.SelectMany (
                _ => sequence,
                (accseq, item) => accseq.Concat (new [] { item })));
    }
}
