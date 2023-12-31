﻿using Components.Base.DataModels;
using Components.Base.Enums;
using Components.Base.Extensions;
using Components.Base.Models;
namespace Components;

/// <inheritdoc cref="IComponentProperty"/>
internal class ComponentProperty : IComponentProperty
{
    public PropertyName Name { get; set; }
    
    public PropertyValue Value { get; set; }
    
    public PropertyType Type { get; set; }
    
    public string []? EnumOptions { get; set; }
    
    public bool Serializable { get; init; }
    
    public bool IsVisible { get; set; }

    /// <summary>
    ///     Creates a new instance of the <see cref="ComponentProperty"/> class.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <param name="value">The default value of the property.</param>
    /// <param name="serializable">Whether or not the property can be serialized.</param>
    /// <param name="enumOptions">If the type is an enum, these are the different options. If this is not null, the <see cref="Type"/> property will be set to <see cref="PropertyType.Enum"/>.</param>
    public ComponentProperty (PropertyName name, PropertyValue value, bool serializable, string[]? enumOptions = null)
    {
        Name = name;
        Value = value;
        
        Serializable = serializable;
        
        EnumOptions = enumOptions;

        if (EnumOptions is null)
            Type = value.PropertyType.ToPropertyType ();
        else
            Type = PropertyType.Enum;
    }
}
