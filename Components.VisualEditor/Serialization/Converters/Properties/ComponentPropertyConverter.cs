using Components.VisualEditor.Controls.Inspector;
using Components.VisualEditor.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Components.VisualEditor.Serialization.Converters.Properties;

public class ComponentPropertyConverter : JsonConverter<IPropertyView>
{

    public override IPropertyView? Read (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        while (reader.Read ())
        {
            //Console.WriteLine (reader.TokenType);
        }

        return null;
    }

    public override void Write (Utf8JsonWriter writer, IPropertyView value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case VectorProperty vectorProperty:
                WriteVector (writer, vectorProperty);
                break;

            case BoolProperty boolProperty:
                WriteBool (writer, boolProperty);
                break;

            case NumericProperty numericProperty:
                WriteNumeric (writer, numericProperty);
                break;

            case EnumProperty enumProperty:
                WriteEnum (writer, enumProperty);
                break;

            case ComponentPointProperty componentPointProperty:
                WriteComponentPoint (writer, componentPointProperty);
                break;

            case StringProperty stringProperty:
                WriteString (writer, stringProperty);
                break;
        }
    }

    private static void WriteString (Utf8JsonWriter writer, StringProperty property)
    {
        writer.WriteStartObject ("String");

        writer.WriteString ("Name", property.PropertyName);
        writer.WriteString ("Value", property.Value);

        writer.WriteEndObject ();
    }

    private static void WriteComponentPoint (Utf8JsonWriter writer, ComponentPointProperty property)
    {
        writer.WriteStartObject ("ComponentPoint");

        writer.WriteString ("Name", property.PropertyName);

        writer.WriteString ("AnchorX", property.AnchorX);
        writer.WriteString ("AnchorY", property.AnchorY);

        writer.WriteString ("OffsetX", property.OffsetX);
        writer.WriteString ("OffsetY", property.OffsetY);

        writer.WriteEndObject ();
    }

    private static void WriteNumeric (Utf8JsonWriter writer, NumericProperty property)
    {
        writer.WriteStartObject ("Numeric");

        writer.WriteString ("Name", property.PropertyName);
        writer.WriteString ("Value", property.Value);

        writer.WriteEndObject ();
    }

    private static void WriteBool (Utf8JsonWriter writer, BoolProperty property)
    {
        writer.WriteStartObject ("Bool");

        writer.WriteString ("Name", property.PropertyName);
        writer.WriteBoolean ("Value", property.Value);

        writer.WriteEndObject ();
    }

    private static void WriteEnum (Utf8JsonWriter writer, EnumProperty property)
    {
        writer.WriteStartObject ("Enum");

        writer.WriteString ("Name", property.PropertyName);
        writer.WriteString ("Value", property.SelectedItem?.ToString ());

        writer.WriteStartArray ("Options");
        foreach (var option in property.ItemsSource!)
        {
            writer.WriteStringValue (option.ToString ());
        }
        writer.WriteEndArray ();

        writer.WriteEndObject ();
    }

    private static void WriteVector (Utf8JsonWriter writer, VectorProperty property)
    {
        writer.WriteStartObject ("Vector");

        writer.WriteString ("Name", property.PropertyName);

        writer.WriteString ("X", property.X);
        writer.WriteString ("Y", property.Y);

        writer.WriteEndObject ();
    }
}
