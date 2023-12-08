using Components.VisualEditor.Models;
using Components.VisualEditor.Models.Render;
using Components.VisualEditor.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Components.VisualEditor.Serialization.Converters;

public class EditorConverter : JsonConverter<IEditor>
{

    public override IEditor? Read (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        IEditor editor = new EditorViewModel ();
        
        var metadataConverter = (JsonConverter<IMetadata>)options.GetConverter (typeof (IMetadata));
        var renderCommandConverter = (JsonConverter<IEditorRenderCommand>)options.GetConverter (typeof (IEditorRenderCommand));
        
        var startDepth = reader.CurrentDepth;
        while (reader.Read ())
        {
            if (reader.TokenType is JsonTokenType.EndObject && reader.CurrentDepth == startDepth)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                switch (reader.GetString ())
                {
                    case "Metadata":
                        editor.Metadata = metadataConverter.Read (ref reader, typeof (IMetadata), options)!;
                        break;
                    case "RenderDescriptions":
                        editor.RenderDescriptions = new ObservableCollection<IEditorRenderCommand> ();
                        while (reader.Read ())
                        {
                            if (reader.TokenType == JsonTokenType.EndArray)
                                break;

                            if (reader.TokenType == JsonTokenType.StartObject)
                                editor.RenderDescriptions.Add (renderCommandConverter.Read (ref reader, typeof (IEditorRenderCommand), options)!);
                        }
                        break;
                }
            }
        }

        return editor;
    }
    public override void Write (Utf8JsonWriter writer, IEditor value, JsonSerializerOptions options)
    {
        var metadataConverter = (JsonConverter<IMetadata>)options.GetConverter (typeof (IMetadata));
        var renderCommandConverter = (JsonConverter<IEditorRenderCommand>)options.GetConverter (typeof (IEditorRenderCommand));
        writer.WriteStartObject ();
        
        writer.WritePropertyName ("Metadata");
        metadataConverter.Write (writer, value.Metadata, options);
        
        writer.WritePropertyName("RenderDescriptions");
        writer.WriteStartArray ();
        foreach (var renderDescription in value.RenderDescriptions)
        {
            renderCommandConverter.Write (writer, renderDescription, options);
        }
        writer.WriteEndArray ();
        
        writer.WriteEndObject ();
    }
}
