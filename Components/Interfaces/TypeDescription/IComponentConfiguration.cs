using Components.Enums;
namespace Components.Interfaces.TypeDescription;

/// <summary>
///     Defines the configuration of a component.
/// </summary>
public interface IComponentConfiguration
{
    /// <summary>
    ///     The bounds of the component.
    /// </summary>
    IEnumerable<IComponentBounds> Bounds { get; set; }

    /// <summary>
    ///     The auto-rotate type of the component.
    /// </summary>
    AutoRotateType AutoRotate { get; set; }

    /// <summary>
    ///     The auto-rotate flip state of the component.
    /// </summary>
    FlipState AutoRotateFlip { get; set; }
}