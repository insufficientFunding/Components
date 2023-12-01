namespace Components.Render.TypeDescription.Conditions;

public class Conditional<T>
{
    public IConditionTreeItem Conditions { get; protected set; }

    public T Value { get; set; }

    public Conditional ()
    {
        Value = default!;
        Conditions = ConditionTree.Empty;
    }

    public Conditional (T value, IConditionTreeItem conditions)
    {
        Value = value;
        Conditions = conditions;
    }
}
