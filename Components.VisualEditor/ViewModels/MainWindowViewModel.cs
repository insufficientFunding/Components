using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Components.VisualEditor.Models;
using Components.VisualEditor.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
namespace Components.VisualEditor.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IFileService? _fileService;

    [ObservableProperty] private IEditor? _editor;
    
    public MainWindowViewModel (IFileService fileService)
    {
        Editor = new EditorViewModel ();
        _fileService = fileService;
    }

    public MainWindowViewModel ()
    {
        Editor = new EditorViewModel ();
    }

    #region RelayCommands
    [RelayCommand]
    private void New ()
    {
        Editor = new EditorViewModel ();
    }

    [RelayCommand]
    private async Task Open ()
    {
        if (_fileService is null)
            return;

        IEditor? editor = await _fileService.OpenComponentAsync ();

        if (editor is not null)
            Editor = editor;
    }

    [RelayCommand]
    private void Save ()
    {
        if (Editor is null)
            return;

        _fileService?.SaveComponentAsync (Editor);
    }
    #endregion
}
