using Serialization.Writer;
using System.Text;
namespace Serialization;

public partial class Serializer
{

    public async Task<string> SerializeAsync (object target)
    {
        StringCreator builder = new StringCreator ();
        
        var objectType = target.GetType ();
        if (_converterService.TryFindConverter (objectType, out var foundConverter))
            foundConverter.Serialize(ref builder, target);
        
        return builder.ToString ();
    }

    public async Task<T> DeserializeAsync<T> (string json)
    {
        throw new System.NotImplementedException ();
    }
}
