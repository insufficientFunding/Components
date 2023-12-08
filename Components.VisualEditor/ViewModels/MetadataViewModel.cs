using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using Components.VisualEditor.Models;
namespace Components.VisualEditor.ViewModels;

public partial class MetadataViewModel : ObservableObject, IMetadata
{
    [ObservableProperty] private string _name = string.Empty;
}
