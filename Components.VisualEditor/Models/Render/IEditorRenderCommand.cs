using Components.IO.Xml.Parsers.Conditions;
using Components.Render.TypeDescription.TypeDescription;
using Components.VisualEditor.Enums;
namespace Components.VisualEditor.Models.Render;

public interface IEditorRenderCommand
{
    RenderCommandType Type { get; }

    string Name { get; set; }

    object Flatten (ComponentDescription description, IConditionParser conditionParser);
}
