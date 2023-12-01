namespace Components.Render.TypeDescription.TypeDescription;

/// <inheritdoc cref="IComponentDescriptionMetadata"/>
public class ComponentDescriptionMetadata
{
    public string Name { get; set; }
    
    public string Group { get; set; }
    
    public double Size { get;  set; }
    
    public IDictionary<string, string> Entries { get;  }

    /// <summary>
    ///     Creates a new instance of the <see cref="ComponentDescriptionMetadata"/> class.
    /// </summary>
    internal protected ComponentDescriptionMetadata ()
    {
        Name = "undefined";
        Group = "undefined";
        Size = 6D;
        Entries = new Dictionary<string, string> ();
    }
}
