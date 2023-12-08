using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Components.IO.Xml.Parsers.Conditions;
using Components.Render.Drawing.RenderCommands;
using Components.Render.TypeDescription;
using Components.Render.TypeDescription.Conditions;
using Components.Render.TypeDescription.TypeDescription;
using Components.VisualEditor.Controls.Inspector;
using Components.VisualEditor.Enums;
using Components.VisualEditor.Messages;
using Components.VisualEditor.Models;
using Components.VisualEditor.Models.Render;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
namespace Components.VisualEditor.ViewModels.RenderCommands;

[ObservableObject]
public partial class RenderGroupViewModel : Conditional<IEditorRenderCommand []?>, IEditorRenderCommand
{
    public RenderCommandType Type => RenderCommandType.Group;

    [ObservableProperty] private string _name = "RenderGroup";

    [ObservableProperty] private ConditionsProperty _rawConditions;

    [ObservableProperty] private bool _forceHidden;

    [ObservableProperty] private ObservableCollection<IEditorRenderCommand> _children;

    #region Constructors
    public RenderGroupViewModel (string name, ObservableCollection<IEditorRenderCommand> children = null!)
    {
        Name = name;
        RawConditions = new ConditionsProperty ();
        Children = children ?? [];
        Children.CollectionChanged += OnChildrenCollectionChanged;
    }

    public RenderGroupViewModel ()
        : this ("")
    { }
    #endregion

    public object Flatten (ComponentDescription description, IConditionParser conditionParser)
    {
        if (ForceHidden)
            return new RenderDescription (ConditionTree.Empty, []);
        
        IEnumerable<IRenderCommand> commands = Children.Select (c => c.Flatten (description, conditionParser)).Cast<IRenderCommand> ();

        IConditionTreeItem conditions;
        try
        {
            conditions = conditionParser.Parse (description, RawConditions.Flatten());
        }
        catch (Exception e)
        {
            Console.WriteLine (e.Message);

            conditions = ConditionTree.Empty;
        }

        return new RenderDescription (conditions, commands.ToArray ());
    }

    private void OnChildrenCollectionChanged (object? sender, NotifyCollectionChangedEventArgs eventArgs)
    {
        Value = Children.ToArray ();

        WeakReferenceMessenger.Default.Send (new EditorValueChangedMessage ("Children"));
    }
}
