using Components.Base.Models;
using Components.IO.Xml;
using Components.Logging;
using Components.Render.TypeDescription.TypeDescription;
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
        if (!TryGetDescription (name, out ComponentDescription? description))
            return _logger.LogErrorReturnDefault<IPositionalComponent?> ($"Could not find component description with name: {name}");

        return CreateComponent (description!);
    }

    private IPositionalComponent CreateComponent (ComponentDescription description)
    {
        IPositionalComponent component = new PositionalComponent (description.Metadata.Name);

        component.Properties = CreateProperties (component);

        _components.Add (component);

        return component;
    }

    public IEnumerable<IComponentProperty> CreateProperties (IPositionalComponent component)
    {
        if (!TryGetDescription(component.Name, out ComponentDescription? description))
            throw new WarningException ($"Could not find component description with name: {component.Name}");
        
        List<IComponentProperty> properties = description!.Properties
            .Select (descriptionProperty => new ComponentProperty (descriptionProperty.Name, descriptionProperty.Default, descriptionProperty.ShowInEditor, descriptionProperty.EnumOptions))
            .Cast<IComponentProperty> ().ToList ();

        return properties;
    }

    private ComponentDescription ReadComponent (XmlLoader loader, string path)
    {
        using FileStream stream = File.OpenRead (path);

        if (!loader.Load (stream, _logger, out ComponentDescription description))
            Console.WriteLine ("error error on the wall, who's the angriest of them all?");

        return description;
    }
}
