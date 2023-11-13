using Components.Interfaces.Conditions;
namespace Components.Interfaces.TypeDescription;

/// <summary>
///     Defines a rule for formatting a component property.
/// </summary>
public interface IComponentPropertyFormat
{
    /// <summary>
    ///     The conditions that must be met for the property to be formatted by this rule.
    /// </summary>
    IConditionTreeItem Conditions { get; }
    
    /// <summary>
    ///     The value to format the property with.
    /// </summary>
    string Value { get; }

    /// <summary>
    ///     Formats the property value, by getting the value of the property from the component, and formatting it according to the rule set by the <see cref="Value"/> property.
    /// </summary>
    /// <param name="component">The component to get the property value from.</param>
    /// <param name="description">The description of the component.</param>
    /// <returns>The formatted property value.</returns>
    public string Format (IElectricalComponent component, IComponentDescription description);
}
