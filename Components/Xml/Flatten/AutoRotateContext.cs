using Components.Enums;
namespace Components.Xml.Flatten;

internal class AutoRotateContext
{
    public bool Mirror { get; }

    public FlipType FlipType { get; }

    public FlipState FlipState { get; }

    public AutoRotateContext (
        bool mirror,
        FlipType flipType,
        FlipState flipState)
    {
        Mirror = mirror;
        FlipType = flipType;
        FlipState = flipState;
    }
}
