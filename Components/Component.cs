using Components.DataModels;
using Components.Interfaces;
namespace Components;

/// <summary>
///     Represents an electrical component, note that this class is abstract, and cannot be instantiated.
/// </summary>
public abstract class Component : IElectricalComponent
{
    public string Name { get; }

    public IEnumerable<IComponentProperty> Properties { get; set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Component"/> class.
    /// </summary>
    protected Component (string name)
    {
        Name = name;
        Properties = new List<ComponentProperty> ();
    }

    public abstract bool TryGetProperty (string propertyName, out IComponentProperty? property);

    public abstract bool TrySetProperty<T> (string propertyName, T? value);
    
    public abstract void Dispose ();
}
