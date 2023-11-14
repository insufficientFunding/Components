using Components.Interfaces.TypeDescription;
using Components.Xml;
using Components.Xml.Definitions;
namespace Components.Services;

public sealed partial class ComponentService
{
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

}
