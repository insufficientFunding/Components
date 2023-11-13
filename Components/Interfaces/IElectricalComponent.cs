using Components.DataModels;
namespace Components.Interfaces;

/// <summary>
///     Represents an electrical component.
/// </summary>
public interface IElectricalComponent
{
    /// <summary>
    ///     Gets the name of the component.
    /// </summary>
    string Name { get; }
    
    /// <summary>
    ///     Gets the properties of the component.
    /// </summary>
    IEnumerable<IComponentProperty> Properties { get; set; }

    /// <summary>
    ///     Attempts to get the specified property from this component.
    /// </summary>
    /// <param name="propertyName">The name of the property to get.</param>
    /// <param name="property">The property, or null if the property was not found.</param>
    /// <returns>True if the property was found, otherwise false.</returns>
    bool TryGetProperty (string propertyName, out IComponentProperty? property);
}
