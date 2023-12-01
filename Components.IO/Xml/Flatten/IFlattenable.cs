using Components.Render.TypeDescription.Conditions;
namespace Components.IO.Xml.Flatten;

internal interface IFlattenable<T>
{
    IEnumerable<Conditional<T>> Flatten (FlattenContext context);
}
