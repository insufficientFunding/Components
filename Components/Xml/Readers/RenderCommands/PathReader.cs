using Components.Enums;
using Components.Interfaces.Render;
using Components.Primitives;
using Components.Render.Drawing.RenderCommands.Path;
using Components.Xml.Logging;
using System.Xml.Linq;
namespace Components.Xml.Readers.RenderCommands;

internal static class PathReader
{
    public static List<IPathCommand> ParseCommands (XElement pathElement, IXmlLoadLogger logger)
    {
        List<IPathCommand> commands = new List<IPathCommand> ();

        foreach (XElement commandElement in pathElement.Elements ())
        {
            switch (commandElement.Name.LocalName)
            {
                case "MoveTo":
                    commands.Add (ParseMoveTo (commandElement, logger));
                    break;
                case "LineTo":
                    commands.Add (ParseLineTo (commandElement, logger));
                    break;
                case "EllipticalArcTo":
                    commands.Add (ParseEllipticalArc (commandElement, logger));
                    break;
                case "ClosePath":
                    commands.Add (ParseClosePath ());
                    break;
            }
        }

        return commands;
    }

    private static bool ParseBool (XAttribute? attribute, bool fallbackValue = false)
    {
        if (attribute is null)
            return fallbackValue;

        return bool.TryParse (attribute.Value, out bool value) ? value : fallbackValue;
    }

    private static MoveTo ParseMoveTo (XElement element, IXmlLoadLogger logger)
    {
        XAttribute? relativeAtt = element.Attribute ("Relative");
        bool relative = ParseBool (relativeAtt, true);

        XAttribute? positionAtt = element.Attribute ("Position");
        Point.TryParse (positionAtt!.Value, out Point? position);
        
        if (position is null)
            logger.LogError (positionAtt, "Failed to parse position for <EllipticalArcTo> tag");

        return new MoveTo (position!, relative);
    }

    private static LineTo ParseLineTo (XElement element, IXmlLoadLogger logger)
    {
        XAttribute? relativeAtt = element.Attribute ("Relative");
        bool relative = ParseBool (relativeAtt, true);

        XAttribute? positionAtt = element.Attribute ("Position");
        Point.TryParse (positionAtt!.Value,  out Point? position);
        
        if (position is null)
            logger.LogError (positionAtt, "Failed to parse position for <EllipticalArcTo> tag");

        return new LineTo (position!, relative);
    }

    private static EllipticalArcTo ParseEllipticalArc (XElement element, IXmlLoadLogger logger)
    {
        XAttribute? relativeAtt = element.Attribute ("Relative");
        bool relative = ParseBool (relativeAtt, true);

        XAttribute? positionAtt = element.Attribute ("Position");
        Point.TryParse (positionAtt!.Value,  out Point? position);

        XAttribute? radiiAtt = element.Attribute ("Radii");
        Point.TryParse (radiiAtt!.Value,  out Point? radii);

        XAttribute? angleAtt = element.Attribute ("Angle");
        double.TryParse (angleAtt!.Value, out double angle);

        XAttribute? largeArcAtt = element.Attribute ("IsLargeArc");
        bool.TryParse (largeArcAtt!.Value, out bool isLargeArc);

        SweepDirection direction = SweepDirection.Clockwise;
        XAttribute? directionAtt = element.Attribute ("Direction");
        if (directionAtt!.Value == "Clockwise")
            direction = SweepDirection.Clockwise;
        else if (directionAtt.Value == "CounterClockwise")
            direction = SweepDirection.CounterClockwise;

        if (radii is null)
            logger.LogError (radiiAtt, "Failed to parse radii for <EllipticalArcTo> tag");
        if (position is null)
            logger.LogError (positionAtt, "Failed to parse position for <EllipticalArcTo> tag");

        return new EllipticalArcTo (radii!, position!, angle, isLargeArc, direction, relative);
    }

    private static ClosePath ParseClosePath ()
    {
        return new ClosePath ();
    }
}
