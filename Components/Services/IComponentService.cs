using Components.Base.Models;
using Components.Render.TypeDescription.TypeDescription;
namespace Components.Services;

/// <summary>
///     Represents a service for reading and creating components.
/// </summary>
public interface IComponentService
{
    #region Components 
    /// <summary>
    ///     Creates a component from a <see cref="ComponentDescription"/> matching the given name.
    /// </summary>
    /// <param name="name">The name of the component description to create a component from.</param>
    /// <returns>A component created from the given description, or null if no such description exists.</returns>
    IPositionalComponent? CreateComponent (string name);

    /// <summary>
    ///     Creates the given component's properties, from a <see cref="ComponentDescription"/> matching the component's name.
    /// </summary>
    /// <param name="component">The component to create properties for.</param>
    /// <returns>A collection of properties created from the given component's description.</returns>
    IEnumerable<IComponentProperty> CreateProperties (IPositionalComponent component);
    
    /// <summary>
    ///     Gets all active components created by this service.
    /// </summary>
    /// <returns>A collection of all active components created by this service.</returns>
    HashSet<IPositionalComponent> GetComponents ();
    
    /// <summary>
    ///     Removes the given component from this service, and disposes of it.
    /// </summary>
    /// <param name="component"></param>
    void RemoveComponent (IPositionalComponent component);
    
    #endregion
    /// <summary>
    ///     Attempts to get a <see cref="ComponentDescription"/> matching the given name.
    /// </summary>
    /// <param name="name">The name of the component description to get.</param>
    /// <param name="description">An <see cref="ComponentDescription"/> matching the given name, or null if no such description exists.</param>
    /// <returns><b>True</b> if a description matching the given name was found, <b>false</b> otherwise.</returns>
    bool TryGetDescription (string name, out ComponentDescription? description);
    
    /// <summary>
    ///     Gets all component descriptions.
    /// </summary>
    HashSet<ComponentDescription> GetDescriptions ();
    
    /// <summary>
    ///     Reads the component descriptions from the given library path.
    /// </summary>
    /// <param name="libraryPath">The path to the library to read the component descriptions from.</param>
    void ReadDescriptions (string libraryPath);
}
