using Components.DataModels;
using Components.Enums;
using Components.Interfaces;
using Components.Interfaces.TypeDescription;
namespace Components.Render.TypeDescription.TypeDescription;

/// <inheritdoc cref="IComponentDescriptionProperty"/>
public class ComponentDescriptionProperty : IComponentDescriptionProperty
{
    public PropertyName Name { get; set; }
    public PropertyValue Default { get; set; }
    public PropertyType Type { get; set; }
    public string []? EnumOptions { get; set; }
    public bool ShowInEditor { get; set; }
    public IComponentPropertyFormat [] FormatRules { get; set; }

    public ComponentDescriptionProperty (PropertyName name, PropertyValue defaultValue, PropertyType type, IComponentPropertyFormat [] formatRules, bool showInEditor, string []? enumOptions = null)
    {
        Name = name;
        Default = defaultValue;
        Type = type;
        EnumOptions = enumOptions;
        ShowInEditor = showInEditor;
        FormatRules = formatRules;
    }

    public string Format (IPositionalComponent component, IComponentDescription description, PropertyValue value)
    {
        foreach (IComponentPropertyFormat formatRule in FormatRules)
        {
            if (formatRule.Conditions.IsMet (component))
                return formatRule.Format (component, description);
        }

        return value.ToString ();
    }
}