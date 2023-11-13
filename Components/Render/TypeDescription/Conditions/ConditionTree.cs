using Components.Interfaces;
using Components.Interfaces.Conditions;
using Components.Interfaces.TypeDescription;
namespace Components.Render.TypeDescription.Conditions;

public class ConditionTree : IConditionTree
{
    public static IConditionTreeItem Empty => new ConditionTreeLeaf ();

    public enum ConditionOperator : ushort
    {
        AND = 1,
        OR = 2,
    }

    private static string ConditionOperatorToString (ConditionOperator op)
    {
        switch (op)
        {
            case ConditionOperator.AND:
                return ",";
            case ConditionOperator.OR:
                return "|";
            default:
                return "??";
        }
    }

    public ConditionOperator Operator { get; }
    public IConditionTreeItem Left { get; }
    public IConditionTreeItem Right { get; }

    internal protected ConditionTree (ConditionOperator op, IConditionTreeItem left, IConditionTreeItem right)
    {
        Operator = op;
        Left = left;
        Right = right;
    }

    public bool IsMet (IPositionalComponent component, IComponentDescription description)
    {
        if (Operator == ConditionOperator.AND)
            return Left.IsMet (component, description) && Right.IsMet (component, description);
        if (Operator == ConditionOperator.OR)
            return Left.IsMet (component, description) || Right.IsMet (component, description);

        return false;
    }

    public override string ToString ()
    {
        return Left + ConditionOperatorToString (Operator) + Right;
    }

    public override bool Equals (object? obj)
    {
        if (obj == null)
            return false;

        ConditionTree? o = obj as ConditionTree;
        if (o == null)
            return false;

        return Operator.Equals (o.Operator)
               && Left.Equals (o.Left)
               && Right.Equals (o.Left);
    }

    public override int GetHashCode ()
    {
        return Operator.GetHashCode ()
               ^ Left.GetHashCode ()
               ^ Right.GetHashCode ();
    }
}
