using Components.Base.Models;
namespace Components.Render.TypeDescription.Conditions;

public interface IConditionTreeItem
{
    bool IsMet (IPositionalComponent component);
}
