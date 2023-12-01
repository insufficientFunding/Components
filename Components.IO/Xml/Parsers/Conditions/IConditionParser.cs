using Components.Render.TypeDescription.Conditions;
using Components.Render.TypeDescription.TypeDescription;
using System.Xml.Linq;
namespace Components.IO.Xml.Parsers.Conditions;

public interface IConditionParser
{
    IConditionTreeItem Parse (ComponentDescription description, string input);
}

public static class ConditionParserExtension
{
    public static bool Parse (this IConditionParser parser, XAttribute conditionsAttribute, ComponentDescription description, out IConditionTreeItem value)
    {
        try
        {
            string spacesRemovedValue = conditionsAttribute.Value.Replace (" ", string.Empty);
            value = parser.Parse (description, spacesRemovedValue);
            
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine (e);
            value = null!;
            
            return false;
        }
    }
}
