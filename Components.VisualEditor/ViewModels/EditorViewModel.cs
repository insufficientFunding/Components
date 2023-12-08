using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Components.IO.Xml.Parsers.Conditions;
using Components.Render.TypeDescription.TypeDescription;
using Components.VisualEditor.Enums;
using Components.VisualEditor.Extensions;
using Components.VisualEditor.Messages;
using Components.VisualEditor.Models;
using Components.VisualEditor.Models.Render;
using Components.VisualEditor.ViewModels.RenderCommands;
using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Components.VisualEditor.ViewModels;

public partial class EditorViewModel : ViewModelBase, IRecipient<EditorValueChangedMessage>, IEditor
{ 
    public ComponentDescription PreviewDescription { get; }

    [ObservableProperty] private IMetadata _metadata;
    [ObservableProperty] private ObservableCollection<IEditorRenderCommand> _renderDescriptions;
    [ObservableProperty] private ObservableCollection<IEditorProperty> _properties;

    [ObservableProperty] private object? _selectedNode;
    [ObservableProperty] private bool _isAutoUpdate;

    private readonly IConditionParser _conditionParser;

    public EditorViewModel ()
    {
        PreviewDescription = new ComponentDescription ();

        Metadata = new MetadataViewModel
        {
            Name = "New Component",
        };
        
        Properties = [];
        RenderDescriptions = [];


        _conditionParser = new ConditionParser ();

        WeakReferenceMessenger.Default.RegisterAll (this);

        AddCommand = new RelayCommand<string> (AddRenderCommand);
    }


}
