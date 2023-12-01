using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Components.Interfaces.TypeDescription;
using Components.VisualEditor.Enums;
using Components.VisualEditor.Extensions;
using Components.VisualEditor.Messages;
using Components.VisualEditor.Models;
using Components.VisualEditor.ViewModels.RenderCommands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace Components.VisualEditor.ViewModels;

public partial class EditorViewModel
{
    public void RenderPreview ()
    {
        IList<IRenderDescription> renderDescriptions = [];
        foreach (IEditorRenderCommand node in RenderDescriptions)
        {
            if (node.Flatten (PreviewDescription, _conditionParser) is IRenderDescription flattenedNode)
                renderDescriptions.Add (flattenedNode);
        }
        PreviewDescription.RenderDescriptions = renderDescriptions.ToArray ();

        WeakReferenceMessenger.Default.Send<RenderPreviewMessage> ();
    }

    public RelayCommand<string> AddCommand { get; }
    public void AddRenderCommand (string? parameter)
    {
        Console.WriteLine ("hi");
        if (!Enum.TryParse (parameter, out RenderCommandType type))
            return;

        if (SelectedNode is not RenderGroupViewModel renderGroup)
        {
            if (SelectedNode is null && type is RenderCommandType.Group)
                RenderDescriptions.Add (RenderCommandExtensions.GetRenderCommand (RenderCommandType.Group)!);

            return;
        }

        var targetCollection = renderGroup.Children;
        targetCollection.Add (RenderCommandExtensions.GetRenderCommand (type)!);
    }

    public void DeleteRenderCommand ()
    {
        if (SelectedNode is null)
            return;

        if (SelectedNode is IEditorRenderCommand renderCommand)
            HeuristicallyDeleteNode (renderCommand, RenderDescriptions);
        else if (SelectedNode is PathCommandViewModel pathCommand)
        {
            IEnumerable<bool> enumerable = RenderDescriptions.Where (x => x.Type == RenderCommandType.Group)
                .Cast<RenderGroupViewModel> ()
                .Select (x => HeuristicallyDeletePathCommand (pathCommand, x));
        }
    }

    private static bool HeuristicallyDeletePathCommand (PathCommandViewModel pathCommand, RenderGroupViewModel group)
    {
        foreach (var editorRenderCommand in group.Children)
        {
            var node = (RenderCommandViewModel)editorRenderCommand;
            if (node.Type == RenderCommandType.Path &&
                (node.Properties.First (x => x.GetType () == typeof (ObservableCollection<PathCommandViewModel>)) as ObservableCollection<PathCommandViewModel>)!.Remove (pathCommand))
                return true;

            if (editorRenderCommand is RenderGroupViewModel renderGroup)
                HeuristicallyDeletePathCommand (pathCommand, renderGroup);
        }

        return false;
    }

    /// <summary>
    ///     Heuristically delete a node from the given collection of nodes.
    /// </summary>
    /// <param name="editorRenderCommand">The node to be deleted.</param>
    /// <param name="nodes">The collection of nodes.</param>
    private static void HeuristicallyDeleteNode (IEditorRenderCommand? editorRenderCommand, ObservableCollection<IEditorRenderCommand> nodes)
    {
        //  If the target node is found in the current collection, remove it, and return.
        if (nodes.Remove (editorRenderCommand!))
            return;

        //  Loop through the collection of nodes and try to find and remove the target node.
        //  If the target node is not found, call this method recursively on each node
        //  that is of the RenderGroupViewModel type.
        foreach (IEditorRenderCommand renderCommand in nodes)
        {
            if (renderCommand is not RenderGroupViewModel renderGroupViewModel)
                continue;

            if (renderGroupViewModel.Children is null)
                continue;

            if (renderGroupViewModel.Children.Remove (editorRenderCommand!))
                break;

            HeuristicallyDeleteNode (editorRenderCommand, renderGroupViewModel.Children);
        }
    }
    public void Receive (EditorValueChangedMessage message)
    {
        if (IsAutoUpdate)
            RenderPreview ();
    }
}
