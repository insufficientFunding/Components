using Serialization.Converters;
namespace Serialization;

public interface ISerializer
{
    Task<string> SerializeAsync (object target);
    
    Task<T> DeserializeAsync<T> (string json);
    
    void RegisterConverter<T> () where T : class;
}
