namespace Components.Render.TypeDescription.TypeDescription;

/// <summary>
///     Represents a component description.
/// </summary>
public class ComponentDescription
{
    public ComponentDescriptionProperty [] Properties { get; set; }

    public RenderDescription [] RenderDescriptions { get; set; }

    public ComponentConfiguration Configuration { get; set; }

    public ComponentDescriptionMetadata Metadata { get; set; }

    /// <summary>
    ///     Creates a new instance of the <see cref="ComponentDescription"/> class.
    /// </summary>
    public ComponentDescription ()
    {
        Properties = Array.Empty<ComponentDescriptionProperty> ();
        RenderDescriptions = Array.Empty<RenderDescription> ();
        Configuration = new ComponentConfiguration ();
        Metadata = new ComponentDescriptionMetadata ();
    }
}
