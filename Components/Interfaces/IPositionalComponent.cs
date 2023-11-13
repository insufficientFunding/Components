using Components.DataModels;
namespace Components.Interfaces;

/// <summary>
///     Represents a component that can be positioned.
/// </summary>
public interface IPositionalComponent : IElectricalComponent
{
    /// <summary>
    ///     Gets or sets the layout information of this component.
    /// </summary>
    LayoutInformation Layout { get; set; }
}
