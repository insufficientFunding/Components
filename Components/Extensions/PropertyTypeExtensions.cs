using Components.DataModels;
using Components.Enums;
namespace Components.Extensions;

public static class PropertyTypeExtensions
{
    public static PropertyValue.Type ToSimplePropertyType (this PropertyType type)
    {
        switch (type)
        {
            case PropertyType.Boolean:
                return PropertyValue.Type.Boolean;
            case PropertyType.Double:
            case PropertyType.Integer:
                return PropertyValue.Type.Numeric;
            case PropertyType.Enum:
            case PropertyType.String:
                return PropertyValue.Type.String;
            default:
                throw new NotSupportedException ($"Unsupported property type: '{type}'");
        }
    }
    
    public static PropertyType ToPropertyType (this PropertyValue.Type type)
    {
        switch (type)
        {
            case PropertyValue.Type.Boolean:
                return PropertyType.Boolean;
            case PropertyValue.Type.Numeric:
                return PropertyType.Double;
            case PropertyValue.Type.String:
                return PropertyType.String;
            default:
                throw new NotSupportedException ($"Unsupported property type: '{type}'");
        }
    }
}
