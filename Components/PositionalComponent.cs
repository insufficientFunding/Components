using Components.Base.DataModels;
using Components.Base.Models;
using Components.Base.Properties;
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
        // If the property name is a property of this component, then we want to get the property.
        if ((property = Properties.FirstOrDefault (x => x.Name == propertyName)) is not null)
            return true;

        // If the property name does not start with "Show", then we do not want to get the property.
        if (!propertyName.StartsWith ("Show"))
            return false;

        // If the property name starts with "Show", then we want to get the property it is referring to.
        string subbedPropertyName = propertyName.Substring (4);
        property = Properties.FirstOrDefault (x => x.Name == subbedPropertyName);

        return property is not null;
    }

    public override bool TrySetProperty<T> (string propertyName, T? value) where T : default
    {
        // If the property name ends with '.IsVisible', then we want to set the visibility of the property.
        bool isSettingVisibility = propertyName.EndsWith (".IsVisible");
        if (isSettingVisibility)
            propertyName = propertyName.Replace (".IsVisible", "");

        // If the property name doesn't match any of the properties of this component, then we return false.
        if (!TryGetProperty (propertyName, out IComponentProperty? property))
            return false;

        // If we are setting the visibility of the property, then we set the visibility and return true.
        if (property is ISerializableProperty visibleProperty)
        {
            visibleProperty.IsVisible = (bool)((object?)value ?? false);
            return true;
        }

        // Get the type of the value we are setting.
        Type type = typeof (T);

        // Set the value of the property, depending on the type of the value, if the value is null, then we set the value to a default value, and if the type is not supported, then we return false.
        if (value is null)
            property!.Value = new PropertyValue ();
        else if (type == typeof (string))
            property!.Value = new PropertyValue (value as string ?? " ");
        else if (type == typeof (double))
            property!.Value = new PropertyValue ((double)(object)value);
        else if (type == typeof (bool))
            property!.Value = new PropertyValue ((bool)(object)value);
        else
            return false;

        return true;
    }

    public override void Dispose ()
    {
        Console.WriteLine ("Disposing PositionalComponent");
    }
}
