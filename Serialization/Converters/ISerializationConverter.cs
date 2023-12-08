using Serialization.Writer;
using System.Text;
namespace Serialization.Converters;

public interface ISerializationConverter
{
    Type Type { get; }
    void Serialize (ref StringCreator builder, object value);
}

public interface ISerializationConverter<T> : ISerializationConverter
{ }