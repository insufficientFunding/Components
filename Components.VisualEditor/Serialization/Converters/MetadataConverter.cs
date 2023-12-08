using Components.VisualEditor.Models;
using Components.VisualEditor.Serialization.Extensions;
using Components.VisualEditor.ViewModels;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Components.VisualEditor.Serialization.Converters;

public class MetadataConverter : JsonConverter<IMetadata>
{
    public override IMetadata? Read (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        IMetadata metadata = new MetadataViewModel ();
        
        while (reader.Read ())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return metadata;

            switch (reader.TokenType)
            {
                case JsonTokenType.PropertyName:
                    if (reader.ValueTextEquals ("Name"))
                        metadata.Name = reader.ReadString ();
                    break;
            }
        }

        return null;
    }
    
    public override void Write (Utf8JsonWriter writer, IMetadata value, JsonSerializerOptions options)
    {
        writer.WriteStartObject ();

        writer.WriteString ("Name", value.Name);

        writer.WriteEndObject ();
    }
}
