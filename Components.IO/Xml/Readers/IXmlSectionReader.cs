using Components.Render.TypeDescription.TypeDescription;
using System.Xml.Linq;
namespace Components.IO.Xml.Readers;

/// <summary>
///     Represents a reader that reads a section from an XML element.
/// </summary>
internal interface IXmlSectionReader
{
    /// <summary>
    ///     Reads the given section from the given XML element.
    /// </summary>
    /// <param name="element">The XML element to read from.</param>
    /// <param name="description">The component description to read into.</param>
    void ReadSection (XElement element, ComponentDescription description);
}
