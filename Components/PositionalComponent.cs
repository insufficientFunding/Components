using Components.DataModels;
using Components.Interfaces;
namespace Components;

/// <summary>
///     Represents an electrical component that can be positioned.
/// </summary>
public class PositionalComponent : Component, IPositionalComponent
{
    /// <summary>
    ///     Gets or sets the layout information of this component.
    /// </summary>
    public LayoutInformation Layout { get; set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PositionalComponent"/> class.
    /// </summary>
    /// <param name="name"></param>
    public PositionalComponent (string name)
        : this (name, new LayoutInformation ())
    { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PositionalComponent"/> class.
    /// </summary>
    /// <param name="name">The name of the component.</param>
    /// <param name="layout">The layout information of the component.</param>
    public PositionalComponent (string name, LayoutInformation layout)
        : base (name)
    {
        Layout = layout;
    }

    public override bool TryGetProperty (string propertyName, out IComponentProperty? property)
    {
        // Check if the property name starts with 'Show', if not set the property to the result of the enumerable search, and return true if it is not null, otherwise false.
        if (!propertyName.StartsWith ("Show"))
            return (property = Properties.FirstOrDefault (x => x.Name == propertyName)) is not null;
        
        // If the property name starts with "Show", then we want to get the property it is referring to.
        string subbedPropertyName = propertyName.Substring (4);
        property = Properties.FirstOrDefault (x => x.Name == subbedPropertyName);

        return property is not null;
    }
}
