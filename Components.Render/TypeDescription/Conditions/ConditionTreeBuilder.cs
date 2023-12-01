namespace Components.Render.TypeDescription.Conditions;

public static class ConditionTreeBuilder
{
    public static IConditionTreeItem And (IEnumerable<IConditionTreeItem> input)
    {
        IConditionTreeItem result = ConditionTree.Empty;
        foreach (IConditionTreeItem item in input)
        {
            result = new ConditionTree (ConditionTree.ConditionOperator.AND, result, item);
        }
        return result;
    }

    public static IConditionTreeItem And (params IConditionTreeItem [] input)
    {
        return And ((IEnumerable<IConditionTreeItem>)input);
    }
}
