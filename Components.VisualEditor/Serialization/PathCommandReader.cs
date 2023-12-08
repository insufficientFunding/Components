using Components.VisualEditor.Enums;
using Components.VisualEditor.Models;
using Components.VisualEditor.Serialization.Extensions;
using Components.VisualEditor.ViewModels.RenderCommands;
using System;
using System.Collections.Generic;
using System.Text.Json;
namespace Components.VisualEditor.Serialization;

public static class PathCommandReader
{
    public static void Read (ref Utf8JsonReader reader, ref RenderPathViewModel command, JsonSerializerOptions options)
    {
        var startDepth = reader.CurrentDepth;

        while (reader.Read ())
        {
            if (reader.TokenType is JsonTokenType.EndArray && reader.CurrentDepth == startDepth)
                break;

            if (reader.TokenType is JsonTokenType.StartObject)
                command.Commands.Add (ReadCommand (ref reader, options));
        }
    }

    private static PathCommandViewModel ReadCommand (ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var type = PathCommandType.MoveTo;
        var properties = new List<IPropertyView> ();

        var startDepth = reader.CurrentDepth;

        while (reader.Read ())
        {
            if (reader.TokenType is JsonTokenType.EndObject && reader.CurrentDepth == startDepth)
                break;

            if (reader.TokenType is not JsonTokenType.PropertyName)
                continue;

            switch (reader.GetString ())
            {
                case "Type":
                    type = ParseType (reader.ReadString ());
                    break;

                case "Vector":
                    properties.Add (RenderCommandReader.ReadVector (ref reader, options));
                    break;
                
                case "Enum":
                    properties.Add(RenderCommandReader.ReadEnum(ref reader, options));
                    break;
                
                case "ComponentPoint":
                    properties.Add (RenderCommandReader.ReadComponentPoint (ref reader, options));
                    break;

                case "Bool":
                    properties.Add (RenderCommandReader.ReadBool (ref reader, options));
                    break;

                case "Numeric":
                    properties.Add (RenderCommandReader.ReadNumeric (ref reader, options));
                    break;

                case "String":
                    properties.Add (RenderCommandReader.ReadString (ref reader, options));
                    break;
            }
        }

        return new PathCommandViewModel (type, properties);
    }

    public static void ReadProperties (ref Utf8JsonReader reader, ref RenderPathViewModel command, JsonSerializerOptions options)
    {
        string? propertyName = reader.GetString ();

        switch (propertyName)
        {
            case "Name":
                command.Name = reader.ReadString ();
                break;

            case "Vector":
                command.Properties.Add (RenderCommandReader.ReadVector (ref reader, options));
                break;

            case "ComponentPoint":
                command.Properties.Add (RenderCommandReader.ReadComponentPoint (ref reader, options));
                break;

            case "Bool":
                command.Properties.Add (RenderCommandReader.ReadBool (ref reader, options));
                break;

            case "Numeric":
                command.Properties.Add (RenderCommandReader.ReadNumeric (ref reader, options));
                break;

            case "String":
                command.Properties.Add (RenderCommandReader.ReadString (ref reader, options));
                break;
        }
    }

    private static PathCommandType ParseType (string type)
    {
        if (Enum.TryParse (type, out PathCommandType result))
            return result;

        return type switch
        {
            "MoveTo" => PathCommandType.MoveTo,
            "LineTo" => PathCommandType.LineTo,
            "EllipticalArcTo" => PathCommandType.EllipticalArcTo,
            "ClosePath" => PathCommandType.ClosePath,
            _ => PathCommandType.ClosePath,
        };
    }
}
