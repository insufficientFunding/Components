using Components.Enums;
using Components.Interfaces.TypeDescription;
using Components.Render.TypeDescription.Conditions;
using Components.Text;
using Components.Xml.Logging;
using Components.Xml.Parsers.ComponentPoints;
using Components.Xml.Primitives;
using Components.Xml.Readers.RenderCommands;
using Components.Xml.Render;
using Components.Xml.Sections;
using System.Xml.Linq;
namespace Components.Xml.Definitions;

internal class TextCommandWithDefinitionsReader : IRenderCommandReader
{
    private readonly IXmlLoadLogger _logger;
    private readonly IComponentPointParser _componentPointParser;
    private readonly DefinitionsSection _definitionsSection;

    public TextCommandWithDefinitionsReader (
        IXmlLoadLogger logger,
        IComponentPointParser componentPointParser,
        ISectionRegistry sectionRegistry)
    {
        _logger = logger;
        _componentPointParser = componentPointParser;
        _definitionsSection = sectionRegistry.GetSection<DefinitionsSection> ();
    }

    public bool ReadRenderCommand (XElement element, IComponentDescription description, out IXmlRenderCommand command)
    {
        XmlRenderTextWithDefinitions? textCommand = new XmlRenderTextWithDefinitions ();
        command = textCommand;

        if (!ReadTextPosition (element, textCommand))
            return false;

        // Read text alignment, if not found, default to CenterCenter.
        if (!TryParseTextAlignment (element.Attribute ("Alignment"), out ConditionalCollection<TextAlignment> alignment))
            textCommand.Alignment = new ConditionalCollection<TextAlignment> { new Conditional<TextAlignment> (TextAlignment.CenterCenter, ConditionTree.Empty) };
        else
            textCommand.Alignment = alignment;

        // Read text weight, if not found, default to Regular.
        if (!TryParseTextWeight (element.Attribute ("Weight"), out ConditionalCollection<FontWeight> weight))
            textCommand.Weight = new ConditionalCollection<FontWeight> { new Conditional<FontWeight> (FontWeight.Regular, ConditionTree.Empty) };
        else
            textCommand.Weight = weight;

        string? tRotation = "0";
        if (element.Attribute ("Rotate") != null)
            tRotation = element.Attribute ("Rotate")!.Value;

        TextRotation rotation = TextRotation.None;
        switch (tRotation)
        {
            case "0":
                rotation = TextRotation.None;
                break;
            case "90":
                rotation = TextRotation.Rotate90;
                break;
            case "180":
                rotation = TextRotation.Rotate180;
                break;
            case "270":
                rotation = TextRotation.Rotate270;
                break;
            default:
                _logger.LogError (element.Attribute ("Rotate")!, $"Invalid value for text rotation: '{tRotation}'");
                break;
        }
        textCommand.Rotation = new ConditionalCollection<TextRotation> { new Conditional<TextRotation> (rotation, ConditionTree.Empty) };

        double size = 9.0;
        if (element.Attribute ("Size") != null)
        {
            switch (element.Attribute ("Size")!.Value.ToLowerInvariant ())
            {
                case "tiny":
                    size = 4.0;
                    break;
                case "small":
                    size = 6.0;
                    break;
                case "medium":
                    size = 9.0;
                    break;
                case "large":
                    size = 12.0;
                    break;
                case "huge":
                    size = 16.0;
                    break;
                default:
                    _logger.LogWarning (element.Attribute ("Size")!, $"Invalid value for size attribute: '{element.Attribute ("Size")!.Value}'");
                    break;
            }
        }

        XElement? textValueNode = element.Element (XmlLoader.ComponentNamespace + "Value");
        if (textValueNode != null)
        {
            foreach (XElement? spanNode in textValueNode.Elements ())
            {
                string nodeValue = spanNode.Value;
                TextRunFormatting? formatting = new TextRunFormatting (TextRunFormattingType.Normal, size);

                if (spanNode.Name.LocalName == "Sub")
                    formatting.FormattingType = TextRunFormattingType.Subscript;
                else if (spanNode.Name.LocalName == "Sup")
                    formatting.FormattingType = TextRunFormattingType.Superscript;
                else if (spanNode.Name.LocalName != "Span")
                    _logger.LogWarning (spanNode, $"Unknown node '{spanNode.Name}' will be treated as <span>");

                TextRun? textRun = new TextRun (nodeValue, formatting);

                if (!ValidateText (element, description, textRun.Text!))
                    return false;

                textCommand.TextRuns.Add (textRun);
            }
        }
        else if (element.GetAttribute ("Value", _logger, out XAttribute? value))
        {
            TextRun? textRun = new TextRun (value!.Value, new TextRunFormatting (TextRunFormattingType.Normal, size));

            if (!ValidateText (value, description, textRun.Text!))
                return false;

            textCommand.TextRuns.Add (textRun);
        }
        else
        {
            return false;
        }

        return true;
    }

    private bool TryParseTextWeight (XAttribute? weightAttribute, out ConditionalCollection<FontWeight> weight)
    {
        string tWeight = "Regular";
        if (weightAttribute is not null)
            tWeight = weightAttribute.Value;

        if (!tWeight.StartsWith ("$"))
        {
            if (!Enum.TryParse (tWeight, out FontWeight singleWeight))
            {
                _logger.LogError (weightAttribute!, $"Invalid value for text weight: '{tWeight}'");
                weight = null!;
                return false;
            }

            weight = new ConditionalCollection<FontWeight> { new Conditional<FontWeight> (singleWeight, ConditionTree.Empty) };
            return true;
        }

        // Check variable exists
        string variableName = tWeight.Substring (1);
        if (!_definitionsSection.Definitions.TryGetValue (variableName, out ConditionalCollection<string>? variableValues))
        {
            _logger.LogError (weightAttribute!, $"Variable '{tWeight}' does not exist");
            weight = null!;
            return false;
        }

        weight = new ConditionalCollection<FontWeight> ();
        foreach (Conditional<string>? variableValue in variableValues)
        {
            if (!Enum.TryParse (variableValue.Value, out FontWeight parsedValue) || !Enum.IsDefined (typeof (FontWeight), parsedValue))
            {
                _logger.LogError (weightAttribute!, $"Value '{variableValue.Value}' for ${variableName} is not a valid text weight");
                return false;
            }

            weight.Add (new Conditional<FontWeight> (parsedValue, variableValue.Conditions));
        }

        return true;
    }

    private bool TryParseTextAlignment (XAttribute? alignmentAttribute, out ConditionalCollection<TextAlignment> alignment)
    {
        string tAlignment = "CenterCenter";
        if (alignmentAttribute != null!)
            tAlignment = alignmentAttribute.Value;

        if (!tAlignment.StartsWith ("$"))
        {
            if (!Enum.TryParse (tAlignment, out TextAlignment singleAlignment))
            {
                _logger.LogError (alignmentAttribute!, $"Invalid value for text alignment: '{tAlignment}'");
                alignment = null!;
                return false;
            }

            alignment = new ConditionalCollection<TextAlignment> { new Conditional<TextAlignment> (singleAlignment, ConditionTree.Empty) };
            return true;
        }
        else
        {
            // Check variable exists
            string? variableName = tAlignment.Substring (1);
            if (!_definitionsSection.Definitions.TryGetValue (variableName, out ConditionalCollection<string>? variableValues))
            {
                _logger.LogError (alignmentAttribute!, $"Variable '{tAlignment}' does not exist");
                alignment = null!;
                return false;
            }

            // Check all possible values are valid
            alignment = new ConditionalCollection<TextAlignment> ();
            foreach (Conditional<string>? variableValue in variableValues)
            {
                if (!Enum.TryParse (variableValue.Value, out TextAlignment parsedValue) || !Enum.IsDefined (typeof (TextAlignment), parsedValue))
                {
                    _logger.LogError (alignmentAttribute!, $"Value '{variableValue.Value}' for ${variableName} is not a valid text alignment");
                    return false;
                }

                alignment.Add (new Conditional<TextAlignment> (parsedValue, variableValue.Conditions));
            }

            return true;
        }
    }

    private bool ValidateText (XAttribute attribute, IComponentDescription description, string text)
    {
        if (ValidateText (description, text, out string? errorMessage))
            return true;

        _logger.LogError (attribute, errorMessage);
        return false;
    }

    private bool ValidateText (XElement element, IComponentDescription description, string text)
    {
        if (ValidateText (description, text, out string? errorMessage))
            return true;

        _logger.LogError (element, errorMessage);
        return false;
    }

    protected virtual bool ValidateText (IComponentDescription description, string text, out string errorMessage)
    {
        if (!text.StartsWith ("$"))
        {
            errorMessage = null!;
            return true;
        }

        string? propertyName = text.Substring (1);
        if (description.Properties.Any (x => x.Name == propertyName))
        {
            errorMessage = null!;
            return true;
        }

        errorMessage = $"Property {propertyName} used for text value does not exist";
        return false;
    }


    protected virtual bool ReadTextPosition (XElement element, XmlRenderText command)
    {
        if (!element.GetAttribute ("X", _logger, out XAttribute? xAttribute) ||
            !element.GetAttribute ("Y", _logger, out XAttribute? yAttribute))
            return false;

        if (!_componentPointParser.TryParse (xAttribute!, yAttribute!, out XmlComponentPoint position))
            return false;

        command.Position = position;

        return true;
    }
}
