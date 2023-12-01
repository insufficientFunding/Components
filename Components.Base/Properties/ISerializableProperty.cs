using Components.Base.Models;
namespace Components.Base.Properties;

public interface ISerializableProperty : IComponentProperty
{
    bool IsVisible { get; set; }
}
