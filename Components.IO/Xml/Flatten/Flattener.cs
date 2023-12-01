using Components.Base.DataModels;
using Components.Base.Enums;
using Components.Render.TypeDescription.Conditions;
namespace Components.IO.Xml.Flatten;

internal static class Flattener
{
    public static IEnumerable<T> FlattenRoot<T> (this IRootFlattenable<T> rootFlattenable)
    {
        if (rootFlattenable.AutoRotate == AutoRotateType.Off)
        {
            AutoRotateContext? autoRotateContext = new AutoRotateContext (false, FlipType.None, FlipState.None);
            FlattenContext? context = new FlattenContext (ConditionTree.Empty, autoRotateContext);
            return rootFlattenable.Flatten (context);
        }

        AutoRotateContext? horizontalAutoRotateContext = new AutoRotateContext (false, FlipType.None, FlipState.None);
        ConditionTreeLeaf? horizontalConditions = new ConditionTreeLeaf (ConditionType.State, "horizontal", ConditionComparison.Equal, new PropertyValue (true));
        FlattenContext? horizontalContext = new FlattenContext (horizontalConditions, horizontalAutoRotateContext);

        FlipType flipType = FlipType.None;
        if ((rootFlattenable.AutoRotateFlip & FlipState.Primary) == FlipState.Primary)
            flipType |= FlipType.Vertical;
        if ((rootFlattenable.AutoRotateFlip & FlipState.Secondary) == FlipState.Secondary)
            flipType |= FlipType.Horizontal;

        AutoRotateContext? verticalAutoRotateContext = new AutoRotateContext (true, flipType, rootFlattenable.AutoRotateFlip);
        ConditionTreeLeaf? verticalConditions = new ConditionTreeLeaf (ConditionType.State, "horizontal", ConditionComparison.Equal, new PropertyValue (false));
        FlattenContext? verticalContext = new FlattenContext (verticalConditions, verticalAutoRotateContext);

        return rootFlattenable.Flatten (horizontalContext).Concat (rootFlattenable.Flatten (verticalContext));
    }
}
