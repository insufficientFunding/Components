using Components.Xml.Interfaces;
using Components.Xml.Logging;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Xml.Linq;
namespace Components.Xml.Parsers;

/// <inheritdoc cref="IAttributeParser"/>
/// <summary>
///     A class that parses <see cref="XAttribute"/>s
/// </summary>
internal class AttributeParser : IAttributeParser
{
    public bool ParseDouble (XAttribute? attribute, IXmlLoadLogger logger, out double? value, double? defaultValue = null)
    {
        // Initialize the value to the default value.
        value = defaultValue;

        // If the attribute is null, return true if a default value was provided, false otherwise.
        switch (attribute)
        {
            case null when defaultValue is null:
                return logger.LogReturn (false, LogLevel.Error, attribute, $"Error: Attribute {attribute} is not present and no default value was provided, returning null.");
            case null:
                return logger.LogReturn (true, LogLevel.Warning, attribute, $"Warning: Attribute {attribute} is not present, using default value {defaultValue}.");
        }

        // Try to parse the attribute value as a double, return true if successful, false otherwise.
        if (double.TryParse (attribute.Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double result))
        {
            value = result;
            return true;
        }

        // If a default value was provided, return true and log a warning, otherwise return false and log an error.
        if (defaultValue is null)
            return logger.LogReturn (false, LogLevel.Error, attribute, $"Error: Attribute {attribute} could not be parsed as a double.");

        return logger.LogReturn (true, LogLevel.Warning, attribute, $"Warning: Attribute {attribute} could not be parsed as a double, using default value {defaultValue}.");
    }

    public bool ParseDouble (XElement element, string attributeName, IXmlLoadLogger logger, out double? value, double? defaultValue = null)
    {
        // Initialize the value to the default value.
        value = defaultValue;

        // If the attribute is null, return true if a default value was provided, false otherwise.
        if (!element.GetAttributeNullable (attributeName, logger, out XAttribute? attribute))
            return defaultValue is not null || logger.LogReturn (false, LogLevel.Error, attribute, $"Error: Attribute {attributeName} is not present and no default value was provided, returning null.");

        // Try to parse the attribute value as a double, return true if successful.
        if (ParseDouble (attribute, logger, out value, defaultValue))
            return true;

        // If the value could not be parsed, return true if a default value was provided, false otherwise.
        return defaultValue is not null
                   ? logger.LogReturn (true, LogLevel.Warning, attribute, $"Warning: Attribute {attributeName} could not be parsed as a double, using default value {defaultValue}.")
                   : logger.LogReturn (false, LogLevel.Error, attribute, $"Error: Attribute {attributeName} could not be parsed as a double.");
    }

    public bool ParseBool (XAttribute? attribute, IXmlLoadLogger logger, out bool? value, bool? defaultValue = null)
    {
        // Initialize the value to the default value.
        value = defaultValue;

        // If the attribute is null, return true if a default value was provided, false otherwise.
        switch (attribute)
        {
            case null when defaultValue is null:
                return logger.LogReturn (false, LogLevel.Error, attribute, $"Error: Attribute {attribute} is not present and no default value was provided, returning null.");
            case null:
                return logger.LogReturn (true, LogLevel.Warning, attribute, $"Warning: Attribute {attribute} is not present, using default value {defaultValue}.");
        }

        // Try to parse the attribute value as a double, return true if successful, false otherwise.
        if (bool.TryParse (attribute.Value, out bool result))
        {
            value = result;
            return true;
        }

        // If a default value was provided, return true and log a warning, otherwise return false and log an error.
        if (defaultValue is null)
            return logger.LogReturn (false, LogLevel.Error, attribute, $"Error: Attribute {attribute} could not be parsed as a bool.");

        return logger.LogReturn (true, LogLevel.Warning, attribute, $"Warning: Attribute {attribute} could not be parsed as a bool, using default value {defaultValue}.");
    }

    public bool ParseBool (XElement element, string attributeName, IXmlLoadLogger logger, out bool? value, bool? defaultValue = null)
    {
        // Initialize the value to the default value.
        value = defaultValue;

        // If the attribute is null, return true if a default value was provided, false otherwise.
        if (!element.GetAttributeNullable (attributeName, logger, out XAttribute? attribute))
            return defaultValue is not null || logger.LogReturn (false, LogLevel.Error, attribute, $"Error: Attribute {attributeName} is not present and no default value was provided, returning null.");

        // Try to parse the attribute value as a bool, return true if successful.
        if (ParseBool (attribute, logger, out value, defaultValue))
            return true;

        // If the value could not be parsed, return true if a default value was provided, false otherwise.
        return defaultValue is not null
                   ? logger.LogReturn (true, LogLevel.Warning, attribute, $"Warning: Attribute {attributeName} could not be parsed as a bool, using default value {defaultValue}.")
                   : logger.LogReturn (false, LogLevel.Error, attribute, $"Error: Attribute {attributeName} could not be parsed as a bool.");
    }
}
