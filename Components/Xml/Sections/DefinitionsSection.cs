using System.Collections.ObjectModel;
namespace Components.Xml.Sections;

internal class DefinitionsSection
{
    public DefinitionsSection (IDictionary<string, ConditionalCollection<string>> definitions)
    {
        Definitions = new ReadOnlyDictionary<string, ConditionalCollection<string>> (definitions);
    }

    public IReadOnlyDictionary<string, ConditionalCollection<string>> Definitions { get; }
}
