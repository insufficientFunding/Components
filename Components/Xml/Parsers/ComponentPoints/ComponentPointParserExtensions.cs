using Components.Xml.Logging;
using Components.Xml.Primitives;
using System.Xml.Linq;
namespace Components.Xml.Parsers.ComponentPoints;

internal static class ComponentPointParserExtensions
{
    public static bool TryParse (this IComponentPointParser parser, XAttribute x, XAttribute y, out XmlComponentPoint componentPoint)
    {
        return parser.TryParse (x.Value, y.Value, x.GetFileRange (), y.GetFileRange (), out componentPoint!);
    }

    public static bool TryParse (this IComponentPointParser parser, XAttribute position, out XmlComponentPoint componentPoint)
    {
        return parser.TryParse (position.Value, position.GetFileRange (), out componentPoint!);
    }
}
