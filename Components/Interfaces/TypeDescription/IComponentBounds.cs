using Components.Interfaces.Conditions;
using Components.Primitives;
using Components.Render.TypeDescription;
namespace Components.Interfaces.TypeDescription;

/// <summary>
///     Defines the bounds of a component.
/// </summary>
public interface IComponentBounds
{
    /// <summary>
    ///     The size of the component's bounds.
    /// </summary>
    public Size Size { get; }

    /// <summary>
    ///     The offset of the component's bounds.
    /// </summary>
    public Point Offset { get; }

    /// <summary>
    ///     The conditions that must be met for the component bounds to be drawn.
    /// </summary>
    public IConditionTreeItem Conditions { get; }
}
