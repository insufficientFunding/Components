using Components.VisualEditor.Enums;
using Components.VisualEditor.Models;
using Components.VisualEditor.Serialization.Extensions;
using Components.VisualEditor.ViewModels.RenderCommands;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Components.VisualEditor.Serialization.Converters;

public class RenderCommandConverter : JsonConverter<RenderCommandViewModel>
{

    public override RenderCommandViewModel? Read (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var command = new RenderCommandViewModel ();
        
        var startDepth = reader.CurrentDepth;

        while (reader.Read ())
        {
            if (reader.TokenType is JsonTokenType.EndObject && reader.CurrentDepth == startDepth)
                break;

            switch (reader.TokenType)
            {
                case JsonTokenType.PropertyName:
                    if (reader.GetString () == "Type")
                    {
                        var copy = reader;
                        command = new RenderCommandViewModel (ParseType (copy.ReadString ()));
                    }
                    else
                        RenderCommandReader.ReadProperties (ref reader, ref command, options);
                    break;
            }
        }

        return command;
    }

    private static RenderCommandType ParseType (string type)
    {
        if (Enum.TryParse (type, out RenderCommandType result))
            return result;

        return type switch
        {
            "Line" => RenderCommandType.Line,
            "Rectangle" => RenderCommandType.Rectangle,
            "Ellipse" => RenderCommandType.Ellipse,
            "Text" => RenderCommandType.Text,
            _ => RenderCommandType.Line,
        };
    }

    public override void Write (Utf8JsonWriter writer, RenderCommandViewModel value, JsonSerializerOptions options)
    {
        writer.WriteStartObject ();

        writer.WriteString ("Type", value.Type.ToString ());

        writer.WriteString ("Name", value.Name);

        var propertyConverter = (JsonConverter<IPropertyView>)options.GetConverter (typeof (IPropertyView));
        foreach (var property in value.Properties)
        {
            propertyConverter.Write (writer, property, options);
        }

        writer.WriteEndObject ();
    }
}
