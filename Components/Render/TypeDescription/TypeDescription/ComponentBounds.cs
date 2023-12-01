using Components.Interfaces.Conditions;
using Components.Interfaces.TypeDescription;
using Components.Primitives;
namespace Components.Render.TypeDescription.TypeDescription;

/// <inheritdoc cref="IComponentBounds"/>
public class ComponentBounds : IComponentBounds
{
    public Size Size { get; set; }
    
    public Point Offset { get; set; }

    public IConditionTreeItem Conditions { get; set; }

    /// <summary>
    ///     Creates a new instance of the <see cref="ComponentBounds"/> class.
    /// </summary>
    /// <param name="size">The size of the component's bounds.</param>
    /// <param name="offset">The offset of the component's bounds.</param>
    /// <param name="conditions">The conditions that must be met for the component bounds to be drawn.</param>
    internal protected ComponentBounds (Size size, Point offset, IConditionTreeItem conditions)
    {
        Size = size;
        Offset = offset;
        Conditions = conditions;
    }
}
