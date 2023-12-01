using CommunityToolkit.Mvvm.ComponentModel;
namespace Components.VisualEditor.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private EditorViewModel _editorViewModel;

    public MainWindowViewModel ()
    {
        EditorViewModel = new EditorViewModel ();
    }
}
