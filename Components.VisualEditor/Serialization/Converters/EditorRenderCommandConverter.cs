using Components.VisualEditor.Enums;
using Components.VisualEditor.Models;
using Components.VisualEditor.Models.Render;
using Components.VisualEditor.Serialization.Extensions;
using Components.VisualEditor.ViewModels.RenderCommands;
using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Components.VisualEditor.Serialization.Converters;

public class EditorRenderCommandConverter : JsonConverter<IEditorRenderCommand>
{
    public override IEditorRenderCommand? Read (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        IEditorRenderCommand? command = null!;
        
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            var readerCopy = reader;
            readerCopy.Read ();
            string type = readerCopy.ReadString ();
            
            switch (type)
            {
                case "Group" or "RenderGroup":
                    var groupConverter = (JsonConverter<RenderGroupViewModel>)options.GetConverter (typeof (RenderGroupViewModel));
                    command = groupConverter.Read (ref reader, typeof (RenderGroupViewModel), options);
                    break;

                case "Path":
                    var pathConverter = (JsonConverter<RenderPathViewModel>)options.GetConverter (typeof (RenderPathViewModel));
                    command = pathConverter.Read (ref reader, typeof (RenderPathViewModel), options);
                    break;

                default:
                    var commandConverter = (JsonConverter<RenderCommandViewModel>)options.GetConverter (typeof (RenderCommandViewModel));
                    command = commandConverter.Read (ref reader, typeof (RenderCommandViewModel), options);
                    break;
            }
        }

        return command;
    }

    public override void Write (Utf8JsonWriter writer, IEditorRenderCommand value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case RenderGroupViewModel renderGroupViewModel:
                var groupConverter = (JsonConverter<RenderGroupViewModel>)options.GetConverter (typeof (RenderGroupViewModel));
                groupConverter.Write (writer, renderGroupViewModel, options);
                break;

            case RenderPathViewModel renderPathViewModel:
                var pathConverter = (JsonConverter<RenderPathViewModel>)options.GetConverter (typeof (RenderPathViewModel));
                pathConverter.Write (writer, renderPathViewModel, options);
                break;

            case RenderCommandViewModel renderCommandViewModel:
                var commandConverter = (JsonConverter<RenderCommandViewModel>)options.GetConverter (typeof (RenderCommandViewModel));
                commandConverter.Write (writer, renderCommandViewModel, options);
                break;

            default:
                writer.WriteStringValue ("error");
                break;
        }
    }
}
