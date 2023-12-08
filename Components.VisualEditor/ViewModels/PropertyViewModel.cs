using CommunityToolkit.Mvvm.ComponentModel;
using Components.Base.Enums;
using Components.VisualEditor.Models;
using System.Collections.ObjectModel;
namespace Components.VisualEditor.ViewModels;

public partial class PropertyViewModel : ObservableObject, IEditorProperty
{
    [ObservableProperty] private string _name;
    [ObservableProperty] private string _value;
    [ObservableProperty] private PropertyType _type;
    [ObservableProperty] private ObservableCollection<string> _enumOptions;
    [ObservableProperty] private bool _serializable;

    public PropertyViewModel ()
    {
        Name = "New Property";
        Value = "";
        Type = PropertyType.String;
        EnumOptions = [];
        Serializable = false;
    }
}
