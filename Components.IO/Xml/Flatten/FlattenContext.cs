using Components.Render.TypeDescription.Conditions;
namespace Components.IO.Xml.Flatten;

internal class FlattenContext
{
    public IConditionTreeItem AncestorConditions { get; }

    public AutoRotateContext AutoRotate { get; }

    public FlattenContext (IConditionTreeItem ancestorConditions, AutoRotateContext autoRotate)
    {
        AncestorConditions = ancestorConditions;
        AutoRotate = autoRotate;
    }
}
