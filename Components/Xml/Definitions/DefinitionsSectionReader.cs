using Components.Interfaces.Conditions;
using Components.Interfaces.TypeDescription;
using Components.Render.TypeDescription.Conditions;
using Components.Xml.Logging;
using Components.Xml.Parsers.Conditions;
using Components.Xml.Readers;
using Components.Xml.Sections;
using System.Xml.Linq;
namespace Components.Xml.Definitions;

internal class DefinitionsSectionReader : IXmlSectionReader
{
    private readonly IXmlLoadLogger _logger;
    private readonly IConditionParser _conditionParser;
    private readonly ISectionRegistry _sectionRegistry;

    public DefinitionsSectionReader (IXmlLoadLogger logger, IConditionParser conditionParser, ISectionRegistry sectionRegistry)
    {
        _logger = logger;
        _conditionParser = conditionParser;
        _sectionRegistry = sectionRegistry;
    }

    public void ReadSection (XElement element, IComponentDescription description)
    {
        Dictionary<string, ConditionalCollection<string>>? definitions = new Dictionary<string, ConditionalCollection<string>> ();

        foreach (XElement definitionNode in element.Elements (XmlLoader.ComponentNamespace + "Definition"))
        {
            if (!definitionNode.GetAttributeValue ("Name", _logger, out string? name))
                continue;

            ConditionalCollection<string>? values = new ConditionalCollection<string> ();
            foreach (XElement valueWhen in definitionNode.Elements (XmlLoader.ComponentNamespace + "When"))
            {
                valueWhen.GetAttribute ("Conditions", _logger, out XAttribute? conditionsAttribute);
                valueWhen.GetAttributeValue ("Value", _logger, out string? whenValue);

                if (conditionsAttribute is null || whenValue is null)
                    continue;

                _conditionParser.Parse (conditionsAttribute, description, out IConditionTreeItem conditions);

                values.Add (new Conditional<string> (whenValue, conditions));
            }

            definitions.Add (name!, values);
        }

        foreach (XElement? constantNode in element.Elements (XmlLoader.ComponentNamespace + "Constant"))
        {
            if (!constantNode.GetAttributeValue ("Name", _logger, out string? name) ||
                !constantNode.GetAttributeValue ("Value", _logger, out string? value))
                continue;

            ConditionalCollection<string> values = new ConditionalCollection<string> { new Conditional<string> (value!, ConditionTree.Empty) };

            definitions.Add (name!, values);
        }

        _sectionRegistry.RegisterSection (new DefinitionsSection (definitions));
    }
}
