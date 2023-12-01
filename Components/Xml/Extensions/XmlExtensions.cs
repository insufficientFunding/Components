using Components.DataModels;
using Components.Enums;
namespace Components.Xml.Extensions;

internal static class XmlExtensions
{
    public static PropertyValue.Type ToPropertyType (this PropertyType type)
    {
        switch (type)
        {
            case PropertyType.Boolean:
                return PropertyValue.Type.Boolean;
            case PropertyType.Double:
                return PropertyValue.Type.Numeric;
            case PropertyType.Enum:
                return PropertyValue.Type.String;
            case PropertyType.String:
                return PropertyValue.Type.String;
            default:
                return PropertyValue.Type.Unknown;
        }
    }
}
