using Components.Render.TypeDescription.Conditions;
namespace Components.Xml.Flatten;

internal interface IFlattenable<T>
{
    IEnumerable<Conditional<T>> Flatten (FlattenContext context);
}
