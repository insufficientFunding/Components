using Components.Base.Enums;
using Components.Base.Primitives;
using Components.VisualEditor.Controls.Inspector;
using Components.VisualEditor.Models;
using Components.VisualEditor.Parsers;
using Components.VisualEditor.Serialization.Extensions;
using Components.VisualEditor.ViewModels.RenderCommands;
using System;
using System.Collections.Generic;
using System.Text.Json;
using RenderCommandType = Components.VisualEditor.Enums.RenderCommandType;
namespace Components.VisualEditor.Serialization;

public static class RenderCommandReader
{
    public static void ReadProperties (ref Utf8JsonReader reader, ref RenderCommandViewModel command, JsonSerializerOptions options)
    {
        string? propertyName = reader.GetString ();

        switch (propertyName)
        {
            case "Name":
                command.Name = reader.ReadString ();
                break;

            case "Vector":
                command.Properties.Add (ReadVector (ref reader, options));
                break;

            case "ComponentPoint":
                command.Properties.Add (ReadComponentPoint (ref reader, options));
                break;

            case "Bool":
                command.Properties.Add (ReadBool (ref reader, options));
                break;

            case "Numeric":
                command.Properties.Add (ReadNumeric (ref reader, options));
                break;

            case "String":
                command.Properties.Add (ReadString (ref reader, options));
                break;
            
            case "Enum":
                command.Properties.Add (ReadEnum (ref reader, options));
                break;
        }
    }

    public static BoolProperty ReadBool (ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var name = "";
        var value = false;

        while (reader.Read ())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType != JsonTokenType.PropertyName)
                continue;

            switch (reader.GetString ())
            {
                case "Name":
                    name = reader.ReadString ();
                    break;

                case "Value":
                    value = reader.ReadBoolean ();
                    break;

            }
        }

        return new BoolProperty (name, value);
    }

    public static StringProperty ReadString (ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var name = "";
        var value = "";

        while (reader.Read ())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType != JsonTokenType.PropertyName)
                continue;

            switch (reader.GetString ())
            {
                case "Name":
                    name = reader.ReadString ();
                    break;

                case "Value":
                    value = reader.ReadString ();
                    break;

            }
        }

        return new StringProperty (name, value);
    }

    public static EnumProperty ReadEnum (ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var name = "";
        var value = "";
        var enumOptions = new List<string> ();

        while (reader.Read ())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType != JsonTokenType.PropertyName)
                continue;

            switch (reader.GetString ())
            {
                case "Name":
                    name = reader.ReadString ();
                    break;

                case "Value":
                    value = reader.ReadString ();
                    break;

                case "Options":
                    while (reader.Read ())
                    {
                        if (reader.TokenType is JsonTokenType.EndArray)
                            break;
                        
                        if (reader.TokenType is not JsonTokenType.String)
                            continue;

                        var option = reader.GetString ();
                        if (option is not null)
                            enumOptions.Add (option);
                    }
                    break;

            }
        }

        return new EnumProperty (name, enumOptions, value);
    }

    public static NumericProperty ReadNumeric (ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var name = "";
        var value = "0";

        while (reader.Read ())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType != JsonTokenType.PropertyName)
                continue;

            switch (reader.GetString ())
            {
                case "Name":
                    name = reader.ReadString ();
                    break;

                case "Value":
                    value = reader.ReadString ();
                    break;

            }
        }

        return new NumericProperty (name, value.ParseDouble ());
    }

    public static VectorProperty ReadVector (ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var name = "Vector";
        var x = "0";
        var y = "0";

        while (reader.Read ())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType != JsonTokenType.PropertyName)
                continue;

            switch (reader.GetString ())
            {
                case "Name":
                    name = reader.ReadString ();
                    break;

                case "X":
                    x = reader.ReadString ();
                    break;

                case "Y":
                    y = reader.ReadString ();
                    break;

            }
        }

        return new VectorProperty (name)
        {
            X = x,
            Y = y,
        };
    }

    public static ComponentPointProperty ReadComponentPoint (ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var name = "Component Point";
        var anchorX = "Middle";
        var anchorY = "Middle";
        var offsetX = "0";
        var offsetY = "0";

        while (reader.Read ())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType != JsonTokenType.PropertyName)
                continue;

            switch (reader.GetString ())
            {
                case "Name":
                    name = reader.ReadString ();
                    break;

                case "AnchorX":
                    anchorX = reader.ReadString ();
                    break;

                case "AnchorY":
                    anchorY = reader.ReadString ();
                    break;

                case "OffsetX":
                    offsetX = reader.ReadString ();
                    break;

                case "OffsetY":
                    offsetY = reader.ReadString ();
                    break;

            }
        }

        return new ComponentPointProperty (name)
        {
            AnchorX = anchorX,
            AnchorY = anchorY,
            OffsetX = offsetX,
            OffsetY = offsetY,
        };
    }

}
