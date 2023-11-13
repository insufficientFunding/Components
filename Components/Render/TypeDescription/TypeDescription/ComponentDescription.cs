using Components.Interfaces.TypeDescription;
namespace Components.Render.TypeDescription.TypeDescription;

/// <inheritdoc cref="IComponentDescription"/>
/// <summary>
///     Represents a component description.
/// </summary>
public class ComponentDescription : IComponentDescription
{
    public IComponentDescriptionProperty [] Properties { get; internal protected set; }

    public IRenderDescription [] RenderDescriptions { get; internal protected set; }

    public IComponentConfiguration Configuration { get; internal protected set; }

    public IComponentDescriptionMetadata Metadata { get; internal protected set; }

    /// <summary>
    ///     Creates a new instance of the <see cref="ComponentDescription"/> class.
    /// </summary>
    internal protected ComponentDescription ()
    {
        Properties = Array.Empty<IComponentDescriptionProperty> ();
        RenderDescriptions = Array.Empty<IRenderDescription> ();
        Configuration = new ComponentConfiguration ();
        Metadata = new ComponentDescriptionMetadata ();
    }
}
