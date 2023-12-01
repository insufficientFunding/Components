using Components.Base.DataModels;
using Components.Base.Enums;
using Components.Base.Models;
namespace Components.Render.TypeDescription.TypeDescription;

public class ComponentDescriptionProperty
{
    public PropertyName Name { get; set; }
    public PropertyValue Default { get; set; }
    public PropertyType Type { get; set; }
    public string []? EnumOptions { get; set; }
    public bool ShowInEditor { get; set; }
    public ComponentPropertyFormat [] FormatRules { get; set; }

    public ComponentDescriptionProperty (PropertyName name, PropertyValue defaultValue, PropertyType type, ComponentPropertyFormat [] formatRules, bool showInEditor, string []? enumOptions = null)
    {
        Name = name;
        Default = defaultValue;
        Type = type;
        EnumOptions = enumOptions;
        ShowInEditor = showInEditor;
        FormatRules = formatRules;
    }

    public string Format (IPositionalComponent component, ComponentDescription description, PropertyValue value)
    {
        foreach (ComponentPropertyFormat formatRule in FormatRules)
        {
            if (formatRule.Conditions.IsMet (component))
                return formatRule.Format (component, description);
        }

        return value.ToString ();
    }
}