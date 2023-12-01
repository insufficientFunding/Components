using Components.Base.DataModels;
using Components.Base.Enums;
using Components.Base.Primitives;
using Components.IO.Xml.Extensions;
using Components.IO.Xml.Features;
using Components.IO.Xml.Logging;
using Components.IO.Xml.Parsers.Conditions;
using Components.Render.TypeDescription;
using Components.Render.TypeDescription.Conditions;
using Components.Render.TypeDescription.TypeDescription;
using System.Xml.Linq;
namespace Components.IO.Xml.Readers;

internal class DeclarationSectionReader : IXmlSectionReader
{
    private readonly IConditionParser _conditionParser;
    private readonly FeatureSwitcher _featureSwitcher;
    private readonly IXmlLoadLogger _logger;

    public DeclarationSectionReader (
        IConditionParser conditionParser,
        FeatureSwitcher featureSwitcher,
        IXmlLoadLogger logger)
    {
        _conditionParser = conditionParser;
        _featureSwitcher = featureSwitcher;
        _logger = logger;
    }

    public void ReadSection (XElement element, ComponentDescription description)
    {
        foreach (XElement metadataElement in element.Elements (XmlLoader.ComponentNamespace + "Metadata"))
        {
            ReadMetadataNode (metadataElement, ref description);
        }

        if (string.IsNullOrEmpty (description.Metadata.Name))
            _logger.LogError (element, "Component name is not defined.");

        List<ComponentDescriptionProperty> properties = [];
        foreach (XElement propertyElement in element.Elements (XmlLoader.ComponentNamespace + "Property"))
        {
            ComponentDescriptionProperty descriptionDescriptionProperty = ScanPropertyNode (description, propertyElement);
            properties.Add (descriptionDescriptionProperty);
        }
        description.Properties = properties.ToArray ();

        properties.Clear ();
        foreach (XElement propertyElement in element.Elements (XmlLoader.ComponentNamespace + "Property"))
        {
            ComponentDescriptionProperty descriptionDescriptionProperty = ReadPropertyNode (description, propertyElement);
            properties.Add (descriptionDescriptionProperty);
        }
        description.Properties = properties.ToArray ();

        description.Configuration = ReadConfiguration (element.Element (XmlLoader.ComponentNamespace + "Configuration")!, description);
    }

    private ComponentConfiguration ReadConfiguration (XElement configurationElement, ComponentDescription description)
    {
        List<ComponentBounds> bounds = new List<ComponentBounds> ();

        foreach (XElement configuration in configurationElement.Elements ())
        {
            switch (configuration.Name.LocalName)
            {
                case "Bounds":
                    bounds = ReadBoundsNode (configuration, description);
                    break;
            }
        }
        var config = new ComponentConfiguration
        {
            Bounds = bounds.ToArray (),
        };

        return config;
    }

    private ComponentDescriptionProperty ScanPropertyNode (ComponentDescription description, XElement propertyElement)
    {
        string propertyName = propertyElement.Attribute ("Name")!.Value;
        string type = propertyElement.Attribute ("Type")!.Value;
        string defaultValue = propertyElement.Attribute ("Default")!.Value;
        string? serializable = propertyElement.Attribute ("Serializable")?.Value;

        PropertyType propertyType;
        switch (type)
        {
            case "Double":
                propertyType = PropertyType.Double;
                break;
            case "Boolean":
                propertyType = PropertyType.Boolean;
                break;
            default:
                propertyType = PropertyType.String;
                break;
        }

        PropertyValue? propertyDefaultValue = PropertyValue.Parse (defaultValue, propertyType.ToPropertyType ());
        bool showInEditorValue = bool.Parse (serializable ?? "false");

        return new ComponentDescriptionProperty (propertyName, propertyDefaultValue, propertyType, null!, showInEditorValue);
    }

    private ComponentDescriptionProperty ReadPropertyNode (ComponentDescription description, XElement propertyElement)
    {
        string? propertyName = propertyElement.Attribute ("Name")?.Value;
        string? type = propertyElement.Attribute ("Type")?.Value;
        string? defaultValue = propertyElement.Attribute ("Default")?.Value;
        string? serializable = propertyElement.Attribute ("Serializable")?.Value;

        if (propertyName == null || defaultValue == null)
            throw new ArgumentException ("Not all properties were found.");

        PropertyType propertyType;
        switch (type)
        {
            case "Double":
                propertyType = PropertyType.Double;
                break;
            case "Boolean":
                propertyType = PropertyType.Boolean;
                break;
            default:
                propertyType = PropertyType.String;
                break;
        }

        PropertyValue propertyDefaultValue = PropertyValue.Parse (defaultValue, propertyType.ToPropertyType ());

        List<string>? propertyOptions = null;
        if (type == "Enum")
        {
            propertyOptions = new List<string> ();
            propertyType = PropertyType.Enum;
            IEnumerable<XElement> optionNodes = propertyElement.Elements (XmlLoader.ComponentNamespace + "Option");
            foreach (XElement optionNode in optionNodes)
            {
                propertyOptions.Add (optionNode.Value);
            }
        }

        List<ComponentPropertyFormat> formatRules = [];
        if (propertyElement.Attribute ("Format") != null)
            formatRules.Add (new ComponentPropertyFormat (propertyElement.Attribute ("Format")!.Value, ConditionTree.Empty));
        else
        {
            IEnumerable<XElement> formatRuleNodes = propertyElement.Elements (XmlLoader.ComponentNamespace + "Formatting")
                .SelectMany (x => x.Elements (XmlLoader.ComponentNamespace + "Format"));
            foreach (XElement formatNode in formatRuleNodes)
            {
                IConditionTreeItem conditionCollection = ConditionTree.Empty;

                XAttribute? conditionsAttribute = formatNode.Attribute ("Conditions");
                if (conditionsAttribute != null)
                    _conditionParser.Parse (conditionsAttribute, description, out conditionCollection);

                formatRules.Add (new ComponentPropertyFormat (formatNode.Attribute ("Value")!.Value, conditionCollection));
            }
        }

        bool showInEditorValue = bool.Parse (serializable ?? "false");

        ComponentDescriptionProperty descriptionDescriptionProperty = new ComponentDescriptionProperty (propertyName, propertyDefaultValue, propertyType, formatRules.ToArray (), showInEditorValue, propertyOptions?.ToArray ());

        return descriptionDescriptionProperty;
    }

    private List<ComponentBounds> ReadBoundsNode (XElement boundsElement, ComponentDescription description)
    {
        List<ComponentBounds> conditions = new List<ComponentBounds> ();

        IEnumerable<XElement> boundsNodes = boundsElement.Elements (XmlLoader.ComponentNamespace + "BoundsOption");

        foreach (XElement boundsNode in boundsNodes)
        {
            IConditionTreeItem conditionCollection = ConditionTree.Empty;

            XAttribute? conditionsAttribute = boundsNode.Attribute ("Conditions");
            if (conditionsAttribute != null)
                _conditionParser.Parse (conditionsAttribute, description, out conditionCollection);

            Point offset = new Point ();
            if (boundsNode.Attribute ("Offset") != null)
                if (!Point.TryParse(boundsNode.Attribute ("Offset")!.Value, out offset!))
                    offset = new Point ();

            Size.TryParse (boundsNode.Attribute ("Value")!.Value, out Size? size);

            conditions.Add (new ComponentBounds (size!, offset, conditionCollection));
        }

        return conditions;
    }

    private void ReadMetadataNode (XElement metadataElement, ref ComponentDescription description)
    {
        if (!metadataElement.GetAttributeValue ("Name", _logger, out string? metadataName))
            return;
        if (!metadataElement.GetAttributeValue ("Value", _logger, out string? metadataValue))
            return;
        
        switch (metadataName)
        {
            case "Name":
                ((ComponentDescriptionMetadata)description.Metadata).Name = metadataValue!;
                break;
            case "Size":
                ((ComponentDescriptionMetadata)description.Metadata).Size = double.TryParse (metadataValue!, out double size) ? size : default;
                break;
            default:
                description.Metadata.Entries.Add (metadataName!, metadataValue!);

                if (metadataValue!.ToLowerInvariant () == "true")
                    _featureSwitcher.EnableFeatureCandidate (metadataName!, metadataElement);
                return;
        }
    }
}
