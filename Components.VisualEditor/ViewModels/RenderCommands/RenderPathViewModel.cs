using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Components.Interfaces.Render;
using Components.Interfaces.TypeDescription;
using Components.Render.Drawing.RenderCommands;
using Components.Render.Drawing.RenderCommands.Path;
using Components.Render.TypeDescription;
using Components.VisualEditor.Controls.Inspector;
using Components.VisualEditor.Enums;
using Components.VisualEditor.Extensions;
using Components.VisualEditor.Models;
using Components.Xml.Parsers.Conditions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
namespace Components.VisualEditor.ViewModels.RenderCommands;

public partial class RenderPathViewModel : ObservableObject, IEditorRenderCommand
{
    public RenderCommandType Type { get; }

    [ObservableProperty] private string _name = string.Empty;

    [ObservableProperty] private ObservableCollection<object> _properties = [];
    [ObservableProperty] private ObservableCollection<PathCommandViewModel> _commands = [];
    
    public ICommand AddPathCommand { get; }
    public ICommand DeletePathCommand { get; }
    
    public RenderPathViewModel (RenderCommandType type, string? name = null, IEnumerable<object>? properties = null)
    {
        Name = name ?? $"New {type.ToString ()}";
        Type = type;

        AddPathCommand = new RelayCommand<string> (AddCommand);
        DeletePathCommand = new RelayCommand<PathCommandViewModel> (DeleteCommand);
        
        if (properties is not null)
            Properties = new ObservableCollection<object> (properties);
    }

    private void AddCommand (string? parameter)
    {
        if (!Enum.TryParse (parameter, out PathCommandType type))
            return;

        Commands.Add (RenderCommandExtensions.GetPathCommand (type));
    }

    private void DeleteCommand (PathCommandViewModel? parameter)
    {
        Console.WriteLine (parameter);
        if (parameter is not null)
            Commands.Remove (parameter);
    }

    public object Flatten (IComponentDescription description, IConditionParser conditionParser)
    {
        List<IPathCommand> commands = [];
        foreach (var command in Commands)
        {
            if (command.Flatten (description, conditionParser) is IPathCommand flattenedCommand)
                commands.Add (flattenedCommand);
        }

        ComponentPoint position = GetProperty<ComponentPointProperty> ("Start").Flatten ();
        double thickness = double.TryParse (GetProperty<NumericProperty> ("Thickness").Value, out double thicknessValue) ? thicknessValue : 1D;
        bool fill = GetProperty<BoolProperty> ("Fill").Value;

        return new RenderPath (position, commands, thickness, fill);
    }

    public T GetProperty<T> (string propertyName)
    {
        var property = typeof (T).GetProperty ("PropertyName");

        return Properties.Where (x => x.GetType () == typeof (T))
            .Cast<T> ()
            .First (x => (string)property?.GetValue (x)! == propertyName);
    }

}
