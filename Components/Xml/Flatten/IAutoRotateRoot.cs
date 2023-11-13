using Components.Enums;
namespace Components.Xml.Flatten;

internal interface IAutoRotateRoot
{
    /// <summary>
    ///     Determines whether additional render commands are generated automatically to support component rotation.
    /// </summary>
    AutoRotateType AutoRotate { get; set; }

    /// <summary>
    ///     Determines whether to apply a component flip when generating auto-rotated commands.
    /// </summary>
    FlipState AutoRotateFlip { get; set; }
}
