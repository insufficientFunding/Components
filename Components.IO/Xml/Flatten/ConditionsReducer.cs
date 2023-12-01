using Components.Render.TypeDescription.Conditions;
namespace Components.IO.Xml.Flatten;

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


                    if (left.Equals (ConditionTree.Empty) && right.Equals (ConditionTree.Empty))
                    {
                        return ConditionTree.Empty;
                    }

                    if (left.Equals (ConditionTree.Empty))
                    {
                        return right;
                    }

                    if (right.Equals (ConditionTree.Empty))
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
