using Components.IO.Xml.Logging;
using System.Xml.Linq;
namespace Components.IO.Xml;

internal static class XElementExtensions
{
    public static bool GetAttribute (this XElement element, string name, IXmlLoadLogger logger, out XAttribute? attribute)
    {
        attribute = element.Attribute (name);
        if (attribute is null)
        {
            XmlLoadLoggerExtensions.LogError (logger, element, $"Missing attribute '{name}' for <{element.Name.LocalName}> tag");
            return false;
        }

        return true;
    }

    public static bool GetAttributeNullable (this XElement element, string name, IXmlLoadLogger logger, out XAttribute? attribute)
    {
        attribute = element.Attribute (name);

        return attribute is not null;

    }

    public static bool GetAttributeValue (this XElement element, string name, IXmlLoadLogger logger, out string? value)
    {
        if (element.GetAttribute (name, logger, out XAttribute? attribute))
        {
            value = attribute!.Value;
            return true;
        }

        value = null;
        return false;
    }

    public static bool GetAttributeValueNullable (this XElement element, string name, IXmlLoadLogger logger, out string? value)
    {
        if (element.GetAttributeNullable (name, logger, out XAttribute? attribute))
        {
            value = attribute!.Value;
            return true;
        }

        value = null;
        return false;
    }
}
