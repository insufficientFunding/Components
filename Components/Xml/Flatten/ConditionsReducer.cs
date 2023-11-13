using Components.Interfaces.Conditions;
using Components.Render.TypeDescription.Conditions;
namespace Components.Xml.Flatten;

internal static class ConditionsReducer
{
    public static IConditionTreeItem SimplifyConditions (IConditionTreeItem item)
    {
        switch (item)
        {
            case ConditionTreeLeaf leaf:
                return leaf;
            case ConditionTree tree:
                {
                    IConditionTreeItem left = SimplifyConditions (tree.Left);
                    IConditionTreeItem right = SimplifyConditions (tree.Right);

                    if (Equals (left, ConditionTree.Empty) 
                        && Equals (right, ConditionTree.Empty))
                    {
                        return ConditionTree.Empty;
                    }

                    if (Equals (left, ConditionTree.Empty))
                    {
                        return right;
                    }

                    if (Equals (right, ConditionTree.Empty))
                    {
                        return left;
                    }

                    return tree;
                }
            default:
                throw new NotSupportedException ();
        }
    }
}
