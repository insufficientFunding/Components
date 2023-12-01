using Components.Base.DataModels;
using Components.Base.Enums;
namespace Components.Base.Models;

/// <summary>
///     Represents a component property.
/// </summary>
public interface IComponentProperty
{
    /// <summary>
    ///     Gets the name of the property.
    /// </summary>
    public PropertyName Name { get; set; }
    
    /// <summary>
    ///     Gets the default value of the property.
    /// </summary>
    public PropertyValue Value { get; set; }
    
    /// <summary>
    ///     Gets the type of the property.
    /// </summary>
    public PropertyType Type { get; set; }
}
