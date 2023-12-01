using System.Xml;
using System.Xml.Linq;
namespace Components.IO.Xml.Logging;

internal static class XmlExtensions
{
    public static FileRange GetFileRange (this XElement element)
    {
        IXmlLineInfo? line = (IXmlLineInfo)element;
        return new FileRange (line.LineNumber, line.LinePosition, line.LineNumber, line.LinePosition + element.Name.LocalName.Length);
    }

    public static FileRange GetFileRange (this XAttribute attribute)
    {
        IXmlLineInfo? line = (IXmlLineInfo)attribute;
        int start = line.LinePosition + attribute.Name.LocalName.Length + 2;
        return new FileRange (line.LineNumber, line.LinePosition, line.LineNumber, start + attribute.Value.Length + 1);
    }
}
