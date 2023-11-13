using Components.Enums;
using Components.Primitives;
namespace Components.DataModels;

/// <summary>
///     Represents the layout information of a component.
/// </summary>
public class LayoutInformation
{
    /// <summary>
    /// Gets or sets a value indicating whether the component is flipped.
    /// </summary>
    public FlipState Flip { get; set; }

    /// <summary>
    /// Gets or sets the size of the component.
    /// </summary>
    public double Size { get; set; } = 6d;

    /// <summary>
    ///     Gets or sets the orientation of the component.
    /// </summary>
    public Orientation Orientation { get; set; } = Orientation.Horizontal;
}

public static class LayoutInformationExtensions
{
    /// <summary>
    ///     Gets a <see cref="FlipType"/> value from the given <see cref="LayoutInformation"/>.
    /// </summary>
    /// <param name="layout">The layout information to get the flip type from.</param>
    /// <returns>A <see cref="FlipType"/> value representing the flip type of the given <see cref="LayoutInformation"/>.</returns>
    public static FlipType GetFlipType (this LayoutInformation layout)
    {
        switch (layout.Flip)
        {
            case FlipState.None:
                return FlipType.None;
            case FlipState.Primary:
                return layout.Orientation == Orientation.Horizontal ? FlipType.Horizontal : FlipType.Vertical;
            case FlipState.Secondary:
                return layout.Orientation == Orientation.Horizontal ? FlipType.Vertical : FlipType.Horizontal;
            default:
                return FlipType.Both;
        }
    }
}
