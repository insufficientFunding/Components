using Components.Base.Enums;
using System.Collections.ObjectModel;
namespace Components.VisualEditor.Models;

public interface IEditorProperty
{
    string Name { get; set; }
    
    string Value { get; set; }
    
    PropertyType Type { get; set; }
    
    ObservableCollection<string> EnumOptions { get; set; }
    
    bool Serializable { get; set; }
}
