using Components.Enums;
using Components.Interfaces.TypeDescription;
namespace Components.Render.TypeDescription.TypeDescription;

/// <inheritdoc cref="IComponentConfiguration"/>
public class ComponentConfiguration : IComponentConfiguration
{
    public IEnumerable<IComponentBounds> Bounds { get; set; } = null!;

    public AutoRotateType AutoRotate { get; set; }

    public FlipState AutoRotateFlip { get; set; }
}
