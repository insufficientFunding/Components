using Components.VisualEditor.Models;
using Components.VisualEditor.Serialization.Converters.Properties;
using Components.VisualEditor.ViewModels.RenderCommands;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Components.VisualEditor.Serialization.Converters.Path;

public class RenderPathConverter : JsonConverter<RenderPathViewModel>
{

    public override RenderPathViewModel? Read (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var command = new RenderPathViewModel ();
        
        var startDepth = reader.CurrentDepth;

        while (reader.Read ())
        {
            if (reader.TokenType is JsonTokenType.EndObject && reader.CurrentDepth == startDepth)
                break;

            switch (reader.TokenType)
            {
                case JsonTokenType.PropertyName:
                    if (reader.GetString() == "Commands")
                        PathCommandReader.Read (ref reader, ref command, options);
                    else
                        PathCommandReader.ReadProperties (ref reader, ref command, options);
                    break;
            }
        }

        return command;
    }
    public override void Write (Utf8JsonWriter writer, RenderPathViewModel value, JsonSerializerOptions options)
    {
        writer.WriteStartObject ();

        writer.WriteString ("Type", "Path");
        writer.WriteString ("Name", value.Name);

        var propertyConverter = (JsonConverter<IPropertyView>)options.GetConverter (typeof (IPropertyView));
        foreach (var property in value.Properties)
        {
            propertyConverter.Write (writer, property, options);
        }
        
        writer.WriteStartArray("Commands");
        foreach (var command in value.Commands)
        {
            writer.WriteStartObject();
            
            writer.WriteString("Type", command.Type.ToString());
            foreach (var property in command.Properties)
                propertyConverter.Write(writer, property, options);

            writer.WriteEndObject();
        }
        writer.WriteEndArray();

        writer.WriteEndObject ();
    }

    private static void WriteCommand (Utf8JsonWriter writer, PathCommandViewModel command, JsonSerializerOptions options)
    { }
}
