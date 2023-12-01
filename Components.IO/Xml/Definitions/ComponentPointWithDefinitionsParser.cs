using Components.IO.Xml.Logging;
using Components.IO.Xml.Parsers.ComponentPoints;
using Components.IO.Xml.Primitives;
using Components.IO.Xml.Sections;
using Components.Render.TypeDescription.Conditions;
using Microsoft.Extensions.Logging;
using System.Globalization;
namespace Components.IO.Xml.Definitions;

internal class ComponentPointWithDefinitionsParser : ComponentPointParser
{
    private readonly IXmlLoadLogger _logger;
    private readonly DefinitionsSection _definitionsSection;

    public ComponentPointWithDefinitionsParser (IXmlLoadLogger logger, ISectionRegistry sectionRegistry)
        : base (logger)
    {
        _logger = logger;
        _definitionsSection = sectionRegistry.GetSection<DefinitionsSection> ();
    }

    protected override bool TryParseOffset (string offset, OffsetAxis axis, FileRange range, out IXmlComponentPointOffset result)
    {
        if (!offset.Contains ('$'))
            return base.TryParseOffset (offset, axis, range, out result);

        offset = offset.Replace ("(", "").Replace (")", "");

        int variableIndex = offset.IndexOf ('$');
        string? variableName = offset.Substring (variableIndex + 1);

        if (!_definitionsSection.Definitions.TryGetValue (variableName, out ConditionalCollection<string>? variableValues))
        {
            _logger.Log (LogLevel.Error, range, $"Variable '{variableName}' does not exist");
            result = null!;
            return false;
        }

        ConditionalCollection<double>? parsedValues = new ConditionalCollection<double> ();
        foreach (Conditional<string>? variableValue in variableValues)
        {
            if (!double.TryParse (variableValue.Value, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double parsedValue))
            {
                _logger.Log (LogLevel.Error, range, $"Value '{variableValue.Value}' for ${variableName} is not a valid decimal");
                result = null!;
                return false;
            }

            parsedValues.Add (new Conditional<double> (parsedValue, variableValue.Conditions));
        }

        result = new ComponentPointOffsetWithDefinition (offset.First () == '-', parsedValues, axis);
        return true;
    }
}
