using System.Diagnostics.CodeAnalysis;
namespace Serialization.Converters;

public interface IConverterService
{
    public bool TryFindConverter (Type type, [NotNullWhen (true)] out ISerializationConverter? converter);

    void Register (Type converterType);
}
