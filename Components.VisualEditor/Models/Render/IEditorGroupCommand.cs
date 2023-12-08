using Components.VisualEditor.Controls.Inspector;
using System.Collections.ObjectModel;
namespace Components.VisualEditor.Models.Render;

public interface IEditorGroupCommand : IEditorRenderCommand
{
    ObservableCollection<IEditorRenderCommand> Children { get; set; }

    ConditionsProperty RawConditions { get; set; }
}
