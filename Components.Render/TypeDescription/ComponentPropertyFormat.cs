using Components.Base.DataModels;
using Components.Base.Extensions;
using Components.Base.Models;
using Components.Render.TypeDescription.Conditions;
using Components.Render.TypeDescription.TypeDescription;
using System.Globalization;
using System.Text.RegularExpressions;
namespace Components.Render.TypeDescription;

public class ComponentPropertyFormat
{
    public IConditionTreeItem Conditions { get; set; }
    public string Value { get; set; }

    public ComponentPropertyFormat (string value, IConditionTreeItem conditions)
    {
        Value = value;
        Conditions = conditions;
    }

    public string Format (IElectricalComponent component, ComponentDescription description)
    {
        Regex variable = new Regex ("\\$[a-zA-z]+ ");
        string plainVars = variable.Replace (Value, delegate (Match match)
        {
            string propertyName = match.Value.Replace ("$", "").Trim ();
            bool foundProperty = component.TryGetProperty (propertyName, out IComponentProperty? propertyValue);
            if (foundProperty)
                return propertyValue!.Value.ToString ();

            throw new Exception ($"Could not find property '{propertyName}' on component '{component.Name}'.");
        });

        variable = new Regex (@"\$[a-zA-Z]+[\(\)A-z_0-9]+ ");
        string formattedVars = variable.Replace (plainVars, delegate (Match match)
        {
            Regex propertyNameRegex = new Regex ("\\$[a-zA-z]+");
            string propertyName = propertyNameRegex.Match (match.Value).Value.Replace ("$", "").Trim ();
            
            bool foundProperty = component.TryGetProperty (propertyName, out IComponentProperty? propertyValue);
            if (!foundProperty)
                throw new Exception ($"Could not find property '{propertyName}' on component '{component.Name}'.");
            
            return ApplySpecialFormatting (propertyValue!.Value, match.Value.Replace (propertyNameRegex.Match (match.Value).Value, "").Trim ());
        });

        Regex regex = new Regex (@"\\[uU]([0-9A-F]{4})");
        return regex.Replace (formattedVars, match => ((char)int.Parse (match.Value.Substring (2), NumberStyles.HexNumber)).ToString ());

    }

    private static string ApplySpecialFormatting (PropertyValue property, string formatting)
    {
        string [] formatTasks = formatting.Split (new []
        {
            '(',
            ')',
        }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string formatTask in formatTasks)
        {
            if (string.IsNullOrEmpty (formatTask))
                continue;

            string [] parameters = formatTask.Split ('_');
            string task = parameters [0];
            string option = parameters [1];

            if (!property.IsNumeric ())
                continue;

            switch (task)
            {
                case "Divide":
                    property = new PropertyValue (property.NumericValue / option.ParseDouble ());
                    break;
                case "Multiply":
                    property = new PropertyValue (property.NumericValue * option.ParseDouble ());
                    break;
                case "Round":
                    property = new PropertyValue (Math.Round (property.NumericValue, int.Parse (option)));
                    break;
            }
        }

        return property.ToString ();
    }
}
