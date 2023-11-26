using CommunityToolkit.Mvvm.ComponentModel;
using Components.VisualEditor.Models;
using System.Collections.ObjectModel;
namespace Components.VisualEditor.ViewModels;

public partial class EditorViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<HierarchyNode> _nodes;

    public EditorViewModel ()
    {
        Nodes =
        [
            new HierarchyNode ("RenderGroup (text)", [
                new HierarchyNode ("Text"),
                new HierarchyNode ("Text"),
                new HierarchyNode ("Text"),
            ]),
            
            new HierarchyNode ("RenderGroup (body)", [
                new HierarchyNode ("Rectangle (body)"),
                new HierarchyNode ("Line (input)"),
                new HierarchyNode ("Line (output)"),
            ]),
        ];
    }
}
