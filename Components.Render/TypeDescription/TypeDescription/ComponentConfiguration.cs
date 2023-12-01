using Components.Base.Enums;
namespace Components.Render.TypeDescription.TypeDescription;

/// <inheritdoc cref="IComponentConfiguration"/>
public class ComponentConfiguration
{
    public IEnumerable<ComponentBounds> Bounds { get; set; } = null!;

    public AutoRotateType AutoRotate { get; set; }

    public FlipState AutoRotateFlip { get; set; }
}
