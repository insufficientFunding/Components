using Components.DataModels;
using Components.Enums;
namespace Components.Interfaces;

/// <summary>
///     Represents a component property.
/// </summary>
public interface IComponentProperty
{
    /// <summary>
    ///     Gets the name of the property.
    /// </summary>
    PropertyName Name { get; }
    
    /// <summary>
    ///     Gets the value of the property.
    /// </summary>
    PropertyValue Value { get; internal protected set; }
    
    /// <summary>
    ///     Gets the type of the property.
    /// </summary>
    PropertyType Type { get; }
    
    /// <summary>
    ///     Gets the enum options of the property, if the property is set as <see cref="PropertyType.Enum"/>.
    /// </summary>
    string []? EnumOptions { get; }
    
    /// <summary>
    ///     Gets a boolean value indicating whether the property can be serialized.
    /// </summary>
    public bool Serializable { get; init; }
    
    /// <summary>
    ///     Gets a boolean value indicating whether the property is currently visible in the editor.
    /// </summary>
    bool IsVisible { get; internal protected set; }
}
