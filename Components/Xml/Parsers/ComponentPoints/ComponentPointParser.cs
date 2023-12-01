using Components.Render.TypeDescription;
using Components.Xml.Logging;
using Components.Xml.Primitives;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;
namespace Components.Xml.Parsers.ComponentPoints;

internal class ComponentPointParser : IComponentPointParser
{
    private readonly IXmlLoadLogger _logger;

    public ComponentPointParser (IXmlLoadLogger logger)
    {
        _logger = logger;
    }

    public bool TryParse (string x, string y, FileRange xRange, FileRange yRange, out XmlComponentPoint? componentPoint)
    {
        componentPoint = null;

        x = x.Replace (" ", string.Empty);
        y = y.Replace (" ", string.Empty);

        if (!TryParseComponentPosition (x, xRange, out ComponentPoint.Anchor relativeToX, out string remainingX))
            return false;

        if (!TryParseOffsets (remainingX, OffsetAxis.X, xRange, out IList<IXmlComponentPointOffset> xOffsets))
            return false;

        if (!TryParseComponentPosition (y, yRange, out ComponentPoint.Anchor relativeToY, out string remainingY))
            return false;

        if (!TryParseOffsets (remainingY, OffsetAxis.Y, yRange, out IList<IXmlComponentPointOffset> yOffsets))
            return false;

        List<IXmlComponentPointOffset>? offsets = xOffsets.Concat (yOffsets).ToList ();

        componentPoint = new XmlComponentPoint (relativeToX, relativeToY, offsets);
        return true;
    }

    public bool TryParse (string position, FileRange range, out XmlComponentPoint? componentPoint)
    {
        position = position.Replace (" ", string.Empty);

        if (!TryParseComponentPosition (position, range, out ComponentPoint.Anchor relativeTo, out string remaining))
        {
            componentPoint = null;
            return false;
        }

        if (!TryParseOffsets (remaining, null, range, out IList<IXmlComponentPointOffset> offsets))
        {
            componentPoint = null;
            return false;
        }

        componentPoint = new XmlComponentPoint (relativeTo, relativeTo, offsets);
        return true;
    }

    private bool TryParseComponentPosition (string s, FileRange range, out ComponentPoint.Anchor componentPosition, out string remaining)
    {
        if (s.StartsWith ("_Start", StringComparison.OrdinalIgnoreCase))
        {
            componentPosition = ComponentPoint.Anchor.Start;
            remaining = s.Substring ("_Start".Length);
            return true;
        }
        if (s.StartsWith ("_Middle", StringComparison.OrdinalIgnoreCase))
        {
            componentPosition = ComponentPoint.Anchor.Middle;
            remaining = s.Substring ("_Middle".Length);
            return true;
        }
        if (s.StartsWith ("_End", StringComparison.OrdinalIgnoreCase))
        {
            componentPosition = ComponentPoint.Anchor.End;
            remaining = s.Substring ("_End".Length);
            return true;
        }
        _logger.Log (LogLevel.Error, range, $"Invalid point '{s}' (expected a string beginning with '_Start', '_Middle' or '_End'");
        componentPosition = ComponentPoint.Anchor.Absolute;
        remaining = string.Empty;
        return false;
    }

    protected virtual bool TryParseOffsets (string s, OffsetAxis? contextAxis, FileRange range, out IList<IXmlComponentPointOffset> offsets)
    {
        StringReader? reader = new StringReader (s);
        offsets = new List<IXmlComponentPointOffset> ();

        StringBuilder? offsetBuilder = new StringBuilder ();
        int ci;
        while ((ci = reader.Read ()) != -1)
        {
            char c = (char)ci;

            if ((c == '+' || c == '-') && offsetBuilder.Length != 0)
            {
                _logger.Log (LogLevel.Error, range, "Invalid offset");
                return false;
            }

            if (c == '+')
                continue;

            if (c == '-' || c == '.')
            {
                offsetBuilder.Append (c);
                continue;
            }

            if (reader.Peek () != -1 && (char)reader.Peek () != '+' && (char)reader.Peek () != '-')
            {
                offsetBuilder.Append (c);
                continue;
            }

            offsetBuilder.Append (c);
            string? offsetStr = offsetBuilder.ToString ();

            OffsetAxis axis;
            string remaining;
            if (contextAxis != null)
            {
                axis = contextAxis.Value;
                remaining = offsetStr;
            }
            else
            {
                remaining = offsetStr.Substring (0, offsetStr.Length - 1);
                if (!TryParseOffsetAxis (offsetStr.Last (), range, out axis))
                    return false;
            }

            if (!TryParseOffset (remaining, axis, range, out IXmlComponentPointOffset? offset))
                return false;

            offsets.Add (offset);
            offsetBuilder.Clear ();
        }

        return true;
    }

    protected virtual bool TryParseOffset (string offset, OffsetAxis axis, FileRange range, out IXmlComponentPointOffset result)
    {
        if (offset.Length == 0)
        {
            _logger.Log (LogLevel.Error, range, $"Expected a number before '{axis.ToString ().ToLowerInvariant ()}'");
            result = null!;
            return false;
        }

        if (!double.TryParse (offset, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double offsetValue))
        {
            _logger.Log (LogLevel.Error, range, $"Unable to parse '{offset}' as double");
            result = null!;
            return false;
        }

        result = new XmlComponentPointOffset (offsetValue, axis);
        return true;
    }

    private bool TryParseOffsetAxis (char c, FileRange range, out OffsetAxis axis)
    {
        switch (c)
        {
            case 'x':
                {
                    axis = OffsetAxis.X;
                    return true;
                }
            case 'y':
                {
                    axis = OffsetAxis.Y;
                    return true;
                }
            default:
                {
                    _logger.Log (LogLevel.Error, range, $"Unexpected '{c}'; expected 'x' or 'y'");
                    axis = OffsetAxis.X;
                    return false;
                }
        }
    }
}
