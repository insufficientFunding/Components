using Components.DataModels;
using Components.Enums;
namespace Components.Interfaces.TypeDescription;

/// <summary>
///     Represents a component property's default values.
/// </summary>
public interface IComponentDescriptionProperty
{
    /// <summary>
    ///     Gets the name of the property.
    /// </summary>
    public PropertyName Name { get; }
    
    /// <summary>
    ///     Gets the default value of the property.
    /// </summary>
    public PropertyValue Default { get; }
    
    /// <summary>
    ///     Gets the type of the property.
    /// </summary>
    public PropertyType Type { get; }
    
    /// <summary>
    ///     Gets the enum options of the property, if the property is set as <see cref="PropertyType.Enum"/>.
    /// </summary>
    public string []? EnumOptions { get; }
    
    /// <summary>
    ///     Gets a boolean value indicating whether the property can be visible in the editor.
    /// </summary>
    public bool ShowInEditor { get; }
    
    /// <summary>
    ///     Gets the format rules of the property.
    /// </summary>
    public IComponentPropertyFormat [] FormatRules { get; }
}
