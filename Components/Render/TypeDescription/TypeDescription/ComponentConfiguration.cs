using Components.Enums;
using Components.Interfaces.TypeDescription;
namespace Components.Render.TypeDescription.TypeDescription;

/// <inheritdoc cref="IComponentConfiguration"/>
public class ComponentConfiguration : IComponentConfiguration
{
    public IEnumerable<IComponentBounds> Bounds { get; internal protected set; } = null!;

    public AutoRotateType AutoRotate { get; internal protected set; }

    public FlipState AutoRotateFlip { get; internal protected set; }
}
