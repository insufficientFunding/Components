using Components.Interfaces.Conditions;
using Components.Interfaces.TypeDescription;
using System.Xml.Linq;
namespace Components.Xml.Parsers.Conditions;

internal interface IConditionParser
{
    IConditionTreeItem Parse (IComponentDescription description, string input);
}

internal static class ConditionParserExtension
{
    public static bool Parse (this IConditionParser parser, XAttribute conditionsAttribute, IComponentDescription description, out IConditionTreeItem value)
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
