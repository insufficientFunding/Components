using Autofac;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
namespace Serialization.Converters;

public class ConverterService : IConverterService
{
    private readonly HashSet<ISerializationConverter> _converters = [];

    public ConverterService ()
    {
        var executingAssembly = Assembly.GetExecutingAssembly ();
        
        foreach (Type? type in executingAssembly.GetTypes ())
        {
            if (!type.IsAssignableTo (typeof (ISerializationConverter)))
                continue;
            
            if (type is { IsAbstract: false, IsInterface: false })
                Register (type);
        }
    }

    public void Register (Type converterType)
    {
        if (Activator.CreateInstance (converterType) is ISerializationConverter converter)
            _converters.Add (converter);
    }

    public bool TryFindConverter (Type type, [NotNullWhen (true)] out ISerializationConverter? converter)
    {
        if (_converters.FirstOrDefault (x => x.Type == type) is { } foundConverter)
        {
            converter = foundConverter;
            return true;
        }

        converter = null;
        return false;
    }
}
