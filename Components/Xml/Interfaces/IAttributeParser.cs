using Components.Xml.Logging;
using System.Xml.Linq;
namespace Components.Xml.Interfaces;

/// <summary>
///     Interface for classes that parse <see cref="XAttribute"/>s
/// </summary>
internal interface IAttributeParser
{
    /// <summary>
    ///     Parses an attribute as a double.
    /// </summary>
    /// <param name="attribute">The <see cref="XAttribute"/> to parse.</param>
    /// <param name="logger">The <see cref="IXmlLoadLogger"/> to log errors to.</param>
    /// <param name="value">The parsed value, null if parsing failed, or the default value if parsing failed and a default value was provided.</param>
    /// <param name="defaultValue">The default value to use if parsing fails, if null, parsing will fail if the attribute value is not present.</param>
    /// <returns>True if parsing succeeded, or false if parsing failed and no default value was provided.</returns>
    bool ParseDouble (XAttribute? attribute, IXmlLoadLogger logger, out double? value, double? defaultValue = null);
    
    /// <summary>
    ///     Parses an attribute from an <see cref="XElement"/> as a double.
    /// </summary>
    /// <param name="element">The <see cref="XElement"/> to parse the attribute from.</param>
    /// <param name="attributeName">The name of the attribute to parse.</param>
    /// <param name="logger">The <see cref="IXmlLoadLogger"/> to log errors to.</param>
    /// <param name="value">The parsed value, null if parsing failed, or the default value if parsing failed and a default value was provided.</param>
    /// <param name="defaultValue">The default value to use if parsing fails, if null, parsing will fail if the attribute value is not present.</param>
    /// <returns>True if parsing succeeded, or false if parsing failed and no default value was provided.</returns>
    bool ParseDouble (XElement element, string attributeName, IXmlLoadLogger logger, out double? value, double? defaultValue = null);
    
    /// <summary>
    ///     Parses an attribute as a bool.
    /// </summary>
    /// <param name="attribute">The <see cref="XAttribute"/> to parse.</param>
    /// <param name="logger">The <see cref="IXmlLoadLogger"/> to log errors to.</param>
    /// <param name="value">The parsed value, null if parsing failed, or the default value if parsing failed and a default value was provided.</param>
    /// <param name="defaultValue">The default value to use if parsing fails, if null, parsing will fail if the attribute value is not present.</param>
    /// <returns>True if parsing succeeded, or false if parsing failed and no default value was provided.</returns>
    bool ParseBool (XAttribute? attribute, IXmlLoadLogger logger, out bool? value, bool? defaultValue = null);
    
    /// <summary>
    ///     Parses an attribute from an <see cref="XElement"/> as a bool.
    /// </summary>
    /// <param name="element">The <see cref="XElement"/> to parse the attribute from.</param>
    /// <param name="attributeName">The name of the attribute to parse.</param>
    /// <param name="logger">The <see cref="IXmlLoadLogger"/> to log errors to.</param>
    /// <param name="value">The parsed value, null if parsing failed, or the default value if parsing failed and a default value was provided.</param>
    /// <param name="defaultValue">The default value to use if parsing fails, if null, parsing will fail if the attribute value is not present.</param>
    /// <returns>True if parsing succeeded, or false if parsing failed and no default value was provided.</returns>
    bool ParseBool (XElement element, string attributeName, IXmlLoadLogger logger, out bool? value, bool? defaultValue = null);
}
