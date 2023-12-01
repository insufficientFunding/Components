namespace Components.Base.Models;

/// <summary>
///     Represents an electrical component.
/// </summary>
public interface IElectricalComponent : IDisposable
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

    /// <summary>
    ///     Attempts to set the specified property of this component.
    /// </summary>
    /// <param name="propertyName">The name of the property to set.</param>
    /// <param name="value">The value to set the property to.</param>
    /// <typeparam name="T">The type of the value of the property to set.</typeparam>
    /// <returns>True if the property was set, otherwise false.</returns>
    bool TrySetProperty<T> (string propertyName, T? value);
}
