namespace Components.Interfaces.TypeDescription;

/// <summary>
///     Defines the metadata of a component.
/// </summary>
public interface IComponentDescriptionMetadata
{
    /// <summary>
    ///     The name of the component.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///     The group of the component, 
    /// </summary>
    string Group { get; set; }

    /// <summary>
    ///     The size of the component's SVG grid.
    /// </summary>
    double Size { get; set; }

    /// <summary>
    ///     Miscellaneous entries.
    /// </summary>
    IDictionary<string, string> Entries { get; }
}
