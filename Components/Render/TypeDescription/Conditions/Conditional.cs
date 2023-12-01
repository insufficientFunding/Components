using Components.Interfaces.Conditions;
namespace Components.Render.TypeDescription.Conditions;

public class Conditional<T> : IConditional<T>
{
    public IConditionTreeItem Conditions { get; protected set; }

    public T Value { get; set; }

    internal protected Conditional ()
    {
        Value = default!;
        Conditions = ConditionTree.Empty;
    }

    internal protected Conditional (T value, IConditionTreeItem conditions)
    {
        Value = value;
        Conditions = conditions;
    }
}
