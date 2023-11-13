using Components.Xml.Logging;
using Components.Xml.Primitives;
namespace Components.Xml.Parsers.ComponentPoints;

internal interface IComponentPointParser
{
    bool TryParse (string x, string y, FileRange xRange, FileRange yRange, out XmlComponentPoint? componentPoint);

    bool TryParse (string position, FileRange range, out XmlComponentPoint? componentPoint);
}
