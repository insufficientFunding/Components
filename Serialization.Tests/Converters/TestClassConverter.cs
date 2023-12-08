using Serialization.Converters;
using Serialization.Tests.Models;
using Serialization.Writer;
using System;
using System.Text;
namespace Serialization.Tests.Converters;

public class TestClassConverter : SerializationConverter<TestClass>
{

    public override void Serialize (ref StringCreator writer, object value)
    {
        var testClass = (TestClass) value;

        writer.WriteElement ("Person");

        string content = string.Join (Environment.NewLine, [
            writer.GetAttribute("Name", testClass.Name),
            writer.GetAttribute("Age", testClass.Number.ToString())
        ]);

        writer.AppendIndented (content);
        
        writer.WriteEndElement ();
    }
}
