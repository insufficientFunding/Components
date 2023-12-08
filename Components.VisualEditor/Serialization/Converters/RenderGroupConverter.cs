using Components.VisualEditor.Controls.Inspector;
using Components.VisualEditor.Models;
using Components.VisualEditor.Models.Render;
using Components.VisualEditor.Serialization.Extensions;
using Components.VisualEditor.ViewModels.RenderCommands;
using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Components.VisualEditor.Serialization.Converters;

public class RenderGroupConverter : JsonConverter<RenderGroupViewModel>
{
    public override RenderGroupViewModel? Read (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        RenderGroupViewModel command = new RenderGroupViewModel ();

        var startDepth = reader.CurrentDepth;

        while (reader.Read ())
        {
            if (reader.TokenType is JsonTokenType.EndObject && reader.CurrentDepth == startDepth)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
                ReadProperty (ref reader, ref command, options);
        }

        return command;
    }

    private static void ReadProperty (ref Utf8JsonReader reader, ref RenderGroupViewModel command, JsonSerializerOptions options)
    {
        string? propertyName = reader.GetString ();
        switch (propertyName)
        {
            case "Name":
                command.Name = reader.ReadString ();
                break;

            case "ForceHide":
                command.ForceHidden = reader.ReadBoolean ();
                break;

            case "Conditions":
                command.RawConditions.ConditionsCollection = [];
                command.RawConditions.Parse (reader.ReadString ());
                break;

            case "Children":
                var groupConverter = (JsonConverter<RenderGroupViewModel>)options.GetConverter (typeof (RenderGroupViewModel));
                var pathConverter = (JsonConverter<RenderPathViewModel>)options.GetConverter (typeof (RenderPathViewModel));
                var converter = (JsonConverter<RenderCommandViewModel>)options.GetConverter (typeof (RenderCommandViewModel));

                var startDepth = reader.CurrentDepth;
                
                while (reader.Read ())
                {
                    if (reader.TokenType is JsonTokenType.EndArray && reader.CurrentDepth == startDepth)
                        break;

                    if (reader.TokenType != JsonTokenType.StartObject)
                        continue;
                    
                    var readerCopy = reader;
                    readerCopy.Read ();
                    string type = readerCopy.ReadString ();

                    if (type is "Group" or "RenderGroup")
                        command.Children.Add (groupConverter.Read (ref reader, typeof (RenderGroupViewModel), options)!);
                    else if (type is "Path" or "RenderPath")
                        command.Children.Add (pathConverter.Read (ref reader, typeof (RenderPathViewModel), options)!);
                    else
                        command.Children.Add (converter.Read (ref reader, typeof (RenderCommandViewModel), options)!);
                }
                break;
        }

    }

    public override void Write (Utf8JsonWriter writer, RenderGroupViewModel value, JsonSerializerOptions options)
    {
        writer.WriteStartObject ();

        writer.WriteString ("Type", "Group");

        writer.WriteString ("Name", value.Name);
        writer.WriteString ("Conditions", value.RawConditions.Flatten ());
        writer.WriteBoolean ("ForceHide", value.ForceHidden);

        if (value.Children.Count > 0)
            WriteChildren (writer, value.Children, options);

        writer.WriteEndObject ();
    }

    private static void WriteChildren (Utf8JsonWriter writer, ObservableCollection<IEditorRenderCommand> children, JsonSerializerOptions options)
    {
        writer.WriteStartArray ("Children");

        foreach (IEditorRenderCommand child in children)
        {
            var converter = (JsonConverter<IEditorRenderCommand>)options.GetConverter (typeof (IEditorRenderCommand));

            converter.Write (writer, child, options);
        }

        writer.WriteEndArray ();
    }
}
