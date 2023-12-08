using Components.Render.TypeDescription.TypeDescription;
using Components.VisualEditor.Models.Render;
using DynamicData.Binding;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
namespace Components.VisualEditor.Models;

public interface IEditor
{
    [JsonIgnore]
    ComponentDescription PreviewDescription { get; }
    
    IMetadata Metadata { get; set; }
    
    ObservableCollection<IEditorRenderCommand> RenderDescriptions { get; set; }
    
    ObservableCollection<IEditorProperty> Properties { get; set; }
    
    [JsonIgnore]
    object? SelectedNode { get; set; }
    
    [JsonIgnore]
    bool IsAutoUpdate { get; set; }
}
