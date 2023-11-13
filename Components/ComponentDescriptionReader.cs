using Components.Interfaces.TypeDescription;
using Components.Render.TypeDescription.TypeDescription;
using Components.Xml;
using Components.Xml.Definitions;
using Microsoft.Extensions.Logging;

namespace Components;

public class ComponentDescriptionReader : XmlLoader
{
    private readonly ILogger<ComponentDescriptionReader> _logger;

    public ComponentDescriptionReader (ILoggerFactory loggerFactory)
    {
        this.UseDefinitions ();

        _logger = loggerFactory.CreateLogger<ComponentDescriptionReader> ();
    }

    public IEnumerable<IComponentDescription> ReadDescriptions (string libraryPath)
    {
        DirectoryInfo directory = new DirectoryInfo (libraryPath);

        IComponentDescription [] xmlComponentDescriptions =
            directory
                .GetFiles ("*.xml", SearchOption.AllDirectories).Select ((x) =>
                {
                    IComponentDescription component = ReadComponent (x.FullName);
                    component.Metadata.Group = x.Directory!.Name;
                    return component;
                }).ToArray ();

        IList<ComponentDescription> componentDescriptions =
            xmlComponentDescriptions
                .Select (xmlComponentDescription => (ComponentDescription)xmlComponentDescription)
                .ToList ();

        return new List<IComponentDescription> (componentDescriptions);
    }

    private IComponentDescription ReadComponent (string path)
    {
        using Stream stream = File.OpenRead (path);

        return Load (stream, _logger, out IComponentDescription description) ? description : throw new Exception ("Failed to load component.");
    }
}
