using Components.Interfaces.TypeDescription;
namespace Components.Interfaces.Conditions;

public interface IConditionTreeItem
{
    bool IsMet (IPositionalComponent component, IComponentDescription description);
}
