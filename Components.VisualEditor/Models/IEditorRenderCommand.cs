using Components.Interfaces.TypeDescription;
using Components.VisualEditor.Enums;
using Components.VisualEditor.ViewModels.RenderCommands;
using Components.Xml.Parsers.Conditions;
namespace Components.VisualEditor.Models;

public interface IEditorRenderCommand
{
    RenderCommandType Type { get; }

    string Name { get; set; }

    object Flatten (IComponentDescription description, IConditionParser conditionParser);
}
