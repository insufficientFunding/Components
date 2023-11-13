using Components.Enums;
using Components.Interfaces.TypeDescription;
using Components.Text;
using Components.Xml.Logging;
using Components.Xml.Parsers.ComponentPoints;
using Components.Xml.Primitives;
using Components.Xml.Render;
using System.Xml.Linq;
namespace Components.Xml.Readers.RenderCommands;

internal class TextCommandReader : IRenderCommandReader
{
    private readonly IXmlLoadLogger _logger;
    private readonly IComponentPointParser _componentPointParser;

    public TextCommandReader (IXmlLoadLogger logger, IComponentPointParser componentPointParser)
    {
        _logger = logger;
        _componentPointParser = componentPointParser;
    }

    public bool ReadRenderCommand (XElement element, IComponentDescription description, out IXmlRenderCommand command)
    {
        XmlRenderText? textCommand = new XmlRenderText ();
        command = textCommand;

        if (!ReadTextPosition (element, textCommand))
            return false;

        // Parse alignment.
        string tAlignment = "CenterCenter";
        if (element.GetAttributeNullable ("Alignment", _logger, out XAttribute? alignmentAttribute))
            tAlignment = alignmentAttribute!.Value;

        if (!Enum.TryParse (tAlignment, out TextAlignment alignment))
            return _logger.LogErrorReturn (element.Attribute ("Alignment"), $"Invalid value for text alignment: '{tAlignment}'");

        textCommand.Alignment = alignment;

        // Parse weight.
        string tWeight = "Regular";
        if (element.GetAttributeNullable ("Weight", _logger, out XAttribute? weightAttribute))
            tWeight = weightAttribute!.Value;

        if (!Enum.TryParse (tWeight, out FontWeight weight))
            return _logger.LogErrorReturn (element.Attribute ("Weight"), $"Invalid value for text weight: '{tWeight}'");

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
                _logger.LogError (element.Attribute ("Rotate") ?? null, $"Invalid value for text rotation: '{tRotation}'");
                break;
        }
        textCommand.Rotation = rotation;

        double size = 9.0;

        if (element.GetAttributeNullable ("Size", _logger, out XAttribute? sizeAttribute))
        {
            switch (sizeAttribute!.Value.ToLowerInvariant ())
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
                    Console.Error.WriteLine ($"Error: {size}, Invalid value for text size: {sizeAttribute.Value}");
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
                else if (spanNode.Name.LocalName == "Super")
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
            TextRun textRun = new TextRun (value!.Value, new TextRunFormatting (TextRunFormattingType.Normal, size));

            if (!ValidateText (value, description, textRun.Text!))
                return false;

            textCommand.TextRuns.Add (textRun);
        }
        else
            return false;

        return true;
    }

    private bool ValidateText (XAttribute attribute, IComponentDescription description, string text)
    {
        if (ValidateText (description, text, out string? errorMessage))
            return true;

        Console.Error.WriteLine ($"Error: {attribute},  {errorMessage}");
        return false;
    }

    private bool ValidateText (XElement element, IComponentDescription description, string text)
    {
        if (ValidateText (description, text, out string? errorMessage))
            return true;

        Console.Error.WriteLine ($"Error: {element},  {errorMessage}");
        return false;
    }

    protected virtual bool ValidateText (IComponentDescription description, string text, out string? errorMessage)
    {
        if (!text.StartsWith ("$"))
        {
            errorMessage = null;
            return true;
        }

        string propertyName = text.Substring (1);
        if (description.Properties.Any (x => x.Name == propertyName))
        {
            errorMessage = null;
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
