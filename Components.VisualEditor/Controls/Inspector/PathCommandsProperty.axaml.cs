using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Input;
using Components.VisualEditor.ViewModels.RenderCommands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Components.VisualEditor.Controls.Inspector;

public class PathCommandsProperty : TemplatedControl
{
    public static readonly StyledProperty<string> PropertyNameProperty = AvaloniaProperty.Register<PathCommandsProperty, string> (
        "PropertyName", "Commands");

    public string PropertyName
    {
        get => GetValue (PropertyNameProperty);
        set => SetValue (PropertyNameProperty, value);
    }

    public static readonly StyledProperty<ObservableCollection<PathCommandViewModel>> CommandsProperty = AvaloniaProperty.Register<PathCommandsProperty, ObservableCollection<PathCommandViewModel>> (
        "Commands");

    public ObservableCollection<PathCommandViewModel> Commands
    {
        get => GetValue (CommandsProperty);
        set => SetValue (CommandsProperty, value);
    }

    public static readonly StyledProperty<ICommand> AddPathCommandProperty = AvaloniaProperty.Register<PathCommandsProperty, ICommand> (
        "AddPathCommand");

    public ICommand AddPathCommand
    {
        get => GetValue (AddPathCommandProperty);
        set => SetValue (AddPathCommandProperty, value);
    }

    public static readonly StyledProperty<ICommand> DeletePathCommandProperty = AvaloniaProperty.Register<PathCommandsProperty, ICommand> (
        "DeletePathCommand");

    public ICommand DeletePathCommand
    {
        get => GetValue (DeletePathCommandProperty);
        set => SetValue (DeletePathCommandProperty, value);
    }

    public PathCommandsProperty ()
    {
        Commands = [];
    }

    public void AddPathHandler (string? parameter)
    {
        if (parameter is null)
            return;

        AddPathCommand.Execute (parameter);
    }

    public void DeletePathHandler (object? parameter)
    {
        DeletePathCommand.Execute (parameter as PathCommandViewModel);
    }
}