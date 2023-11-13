namespace Components.Interfaces.TypeDescription;

/// <summary>
///     Represents a component's different properties and configurations, as well as its render descriptions.
/// </summary>
public interface IComponentDescription
{
    /// <summary>
    ///     The properties of the component.
    /// </summary>
    IComponentDescriptionProperty [] Properties { get; }

    /// <summary>
    ///     The render descriptions of the component.
    /// </summary>
    IRenderDescription [] RenderDescriptions { get; }

    /// <summary>
    ///     The configuration of the component.
    /// </summary>
    IComponentConfiguration Configuration { get; }

    /// <summary>
    ///     The miscellaneous metadata of the component.
    /// </summary>
    IComponentDescriptionMetadata Metadata { get; }
}
