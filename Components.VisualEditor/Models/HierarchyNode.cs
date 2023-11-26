using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
namespace Components.VisualEditor.Models;

public partial class HierarchyNode : ObservableObject
{
    [ObservableProperty] private ObservableCollection<HierarchyNode>? _subNodes;
    [ObservableProperty] private string _name;

    public HierarchyNode (string name)
    {
        Name = name;
    }

    public HierarchyNode (string name, ObservableCollection<HierarchyNode> subNodes) : this (name)
    {
        SubNodes = subNodes;
    }
}
