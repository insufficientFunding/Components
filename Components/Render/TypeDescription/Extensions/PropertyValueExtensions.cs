using Components.DataModels;
namespace Components.Render.TypeDescription.Extensions;

public static class PropertyValueExtensions
{
    public static bool IsTruthy (this PropertyValue value)
    {
        bool isTruthy = false;
        value.Match (stringValue => isTruthy = !string.IsNullOrEmpty (stringValue),
                     numericValue => isTruthy = Math.Abs ((double)numericValue) > 0.0,
                     boolValue => isTruthy = boolValue);
        return isTruthy;
    }
}
