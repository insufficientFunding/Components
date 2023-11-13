using Components.DataModels;
using Components.Enums;
using Components.Interfaces;
using Components.Interfaces.TypeDescription;
namespace Components.Render.TypeDescription.TypeDescription;

/// <inheritdoc cref="IComponentDescriptionProperty"/>
public class ComponentDescriptionProperty : IComponentDescriptionProperty
{
    public PropertyName Name { get; }
    public PropertyValue Default { get; }
    public PropertyType Type { get; }
    public string []? EnumOptions { get; }
    public bool ShowInEditor { get; }
    public IComponentPropertyFormat [] FormatRules { get; }

    internal protected ComponentDescriptionProperty (PropertyName name, PropertyValue defaultValue, PropertyType type, IComponentPropertyFormat [] formatRules, bool showInEditor, string []? enumOptions = null)
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
            if (formatRule.Conditions.IsMet (component, description))
                return formatRule.Format (component, description);
        }

        return value.ToString ();
    }
}