using Serialization.Writer;
using System.Text;
namespace Serialization.Converters;

public abstract class SerializationConverter<T> : ISerializationConverter<T>
{
    public Type Type => typeof (T);

    public abstract void Serialize (ref StringCreator writer, object value);
}
