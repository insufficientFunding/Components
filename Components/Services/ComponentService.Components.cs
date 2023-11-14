using Components.Interfaces;
using Components.Interfaces.TypeDescription;
using Components.Logging;
using Components.Xml;
using Components.Xml.Definitions;
using System.ComponentModel;
namespace Components.Services;

public sealed partial class ComponentService
{
    public HashSet<IPositionalComponent> GetComponents ()
    {
        return _components;
    }

    public void RemoveComponent (IPositionalComponent component)
    {
        if (!_components.Remove (component))
            throw new WarningException ("No matching component was found in the internal collection, the component was not removed.");
        
        component.Dispose ();
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

    private IComponentDescription ReadComponent (XmlLoader loader, string path)
    {
        using Stream stream = File.OpenRead (path);

        return loader.Load (stream, _logger, out IComponentDescription description) ? description : throw new Exception ("Failed to load component.");
    }
}
