using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Components.Interfaces.TypeDescription;
using Components.Render.TypeDescription.TypeDescription;
using Components.VisualEditor.Controls.Inspector;
using Components.VisualEditor.Enums;
using Components.VisualEditor.Extensions;
using Components.VisualEditor.Messages;
using Components.VisualEditor.Models;
using Components.VisualEditor.ViewModels.RenderCommands;
using Components.Xml.Parsers.Conditions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Components.VisualEditor.ViewModels;

public partial class EditorViewModel : ViewModelBase, IRecipient<EditorValueChangedMessage>
{
    public IComponentDescription PreviewDescription { get; private set; }
    
    [ObservableProperty] private ObservableCollection<IEditorRenderCommand> _renderDescriptions;
    [ObservableProperty] private ObservableCollection<IEditorProperty> _properties;
    [ObservableProperty] private object? _selectedNode;
    [ObservableProperty] private bool _isAutoUpdate;
    
    private readonly IConditionParser _conditionParser;

    public EditorViewModel ()
    {
        RenderDescriptions = [
            new RenderGroupViewModel("Group", [
                RenderCommandExtensions.GetRenderCommand(RenderCommandType.Ellipse),
            ])
        ];

        Properties = [];

        PreviewDescription = new ComponentDescription ();

        _conditionParser = new ConditionParser ();

        WeakReferenceMessenger.Default.RegisterAll (this);

        AddCommand = new RelayCommand<string> (AddRenderCommand);
    }

    
}
