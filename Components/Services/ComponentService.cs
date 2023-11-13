using Components.Interfaces;
using Components.Interfaces.TypeDescription;
using Components.Logging;
using Components.Xml;
using Components.Xml.Definitions;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.ObjectModel;
namespace Components.Services;

/// <inheritdoc cref="IComponentService"/>
/// <summary>
///     A service for reading and creating components.
/// </summary>
public sealed class ComponentService : IComponentService
{
    private readonly HashSet<IComponentDescription> _descriptions = new HashSet<IComponentDescription> ();
    private readonly HashSet<IPositionalComponent> _components = new HashSet<IPositionalComponent> ();

    private readonly ILogger<ComponentService> _logger;

    public ComponentService (ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ComponentService> ();
    }

    public IPositionalComponent? CreateComponent (string name)
    {
        if (!TryGetDescription (name, out IComponentDescription? description))
            return _logger.LogErrorReturnDefault<IPositionalComponent?> ($"Could not find component description with name: {name}");

        return CreateComponent (description!);
    }

    private IPositionalComponent CreateComponent (IComponentDescription description)
    {
        IPositionalComponent component = new PositionalComponent (description.Metadata.Name);

        List<IComponentProperty> properties = description.Properties
            .Select (descriptionProperty => new ComponentProperty (descriptionProperty.Name, descriptionProperty.Default, descriptionProperty.ShowInEditor, descriptionProperty.EnumOptions))
            .Cast<IComponentProperty> ().ToList ();

        component.Properties = properties;

        _components.Add (component);

        return component;
    }

    public bool TryGetDescription (string name, out IComponentDescription? description)
    {
        description = _descriptions.FirstOrDefault (x => x.Metadata.Name == name);

        return description is not null;
    }
    
    public HashSet<IComponentDescription> GetDescriptions ()
    {
        return _descriptions;
    }

    public void ReadDescriptions (string libraryPath)
    {
        DirectoryInfo directory = new DirectoryInfo (libraryPath);

        using XmlLoader loader = new XmlLoader ();
        loader.UseDefinitions ();

        IEnumerable<IComponentDescription> componentDescriptions =
            directory
                .GetFiles ("*.xml", SearchOption.AllDirectories).Select ((x) =>
                {
                    IComponentDescription component = ReadComponent (loader, x.FullName);
                    component.Metadata.Group = x.Directory!.Name;
                    return component;
                }).ToArray ();

        componentDescriptions.ToList ().ForEach (x => _descriptions.Add (x));
    }

    private IComponentDescription ReadComponent (XmlLoader loader, string path)
    {
        using Stream stream = File.OpenRead (path);

        return loader.Load (stream, _logger, out IComponentDescription description) ? description : throw new Exception ("Failed to load component.");
    }
}
