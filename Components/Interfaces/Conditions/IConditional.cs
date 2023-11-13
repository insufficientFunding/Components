namespace Components.Interfaces.Conditions;

public interface IConditional<T>
{
    public IConditionTreeItem Conditions { get; }

    public T Value { get; set; }
}
