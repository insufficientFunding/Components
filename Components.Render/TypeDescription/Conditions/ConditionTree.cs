using Components.Base.Models;
namespace Components.Render.TypeDescription.Conditions;

public class ConditionTree : IConditionTreeItem
{
    public static IConditionTreeItem Empty => new ConditionTreeLeaf ();
    public static IConditionTreeItem EmptyTree => new ConditionTree (ConditionOperator.AND, Empty, Empty);

    public enum ConditionOperator : ushort
    {
        AND = 1,
        OR = 2,
    }

    public static string ConditionOperatorToString (ConditionOperator op)
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

    public ConditionTree (ConditionOperator op, IConditionTreeItem left, IConditionTreeItem right)
    {
        Operator = op;
        Left = left;
        Right = right;
    }

    public bool IsMet (IPositionalComponent component)
    {
        if (Operator == ConditionOperator.AND)
            return Left.IsMet (component) && Right.IsMet (component);
        if (Operator == ConditionOperator.OR)
            return Left.IsMet (component) || Right.IsMet (component);

        return false;
    }

    public override string ToString ()
    {
        return Left + ConditionOperatorToString (Operator) + Right;
    }

    public override bool Equals (object? obj)
    {
        if (obj is not ConditionTree tree)
            return false;

        return Operator.Equals (tree.Operator)
               && Left.Equals (tree.Left)
               && Right.Equals (tree.Left);
    }

    public override int GetHashCode ()
    {
        return Operator.GetHashCode ()
               ^ Left.GetHashCode ()
               ^ Right.GetHashCode ();
    }
}
