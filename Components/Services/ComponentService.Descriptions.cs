using Components.IO.Xml;
using Components.IO.Xml.Definitions;
using Components.Render.TypeDescription.TypeDescription;
namespace Components.Services;

public sealed partial class ComponentService
{
    public bool TryGetDescription (string name, out ComponentDescription? description)
    {
        description = _descriptions.FirstOrDefault (x => x.Metadata.Name == name);

        return description is not null;
    }

    public HashSet<ComponentDescription> GetDescriptions ()
    {
        return _descriptions;
    }

    public void ReadDescriptions (string libraryPath)
    {
        DirectoryInfo directory = new DirectoryInfo (libraryPath);

        using XmlLoader loader = new XmlLoader ();
        loader.UseDefinitions ();

        IEnumerable<ComponentDescription> componentDescriptions =
            directory
                .GetFiles ("*.xml", SearchOption.AllDirectories).Select ((x) =>
                {
                    ComponentDescription component = ReadComponent (loader, x.FullName);
                    component.Metadata.Group = x.Directory!.Name;
                    return component;
                }).ToArray ();

        componentDescriptions.ToList ().ForEach (x => _descriptions.Add (x));
    }

}
