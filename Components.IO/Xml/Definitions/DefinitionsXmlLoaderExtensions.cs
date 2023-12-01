using Autofac;
using Components.IO.Xml.Parsers.ComponentPoints;
using Components.IO.Xml.Readers;
using Components.IO.Xml.Readers.RenderCommands;
namespace Components.IO.Xml.Definitions;

public static class DefinitionsXmlLoaderExtensions
{
    public const string FeatureName = "Experimental.Features.Definitions";

    public static void UseDefinitions (this XmlLoader loader)
    {
        loader.RegisterFeature (FeatureName, builder =>
        {
            builder.RegisterType<ComponentPointWithDefinitionsParser> ().As<IComponentPointParser> ();
            builder.RegisterType<DefinitionsSectionReader> ().Named<IXmlSectionReader> (XmlLoader.ComponentNamespace.NamespaceName + "Definitions");
            builder.RegisterType<TextCommandWithDefinitionsReader> ().Named<IRenderCommandReader> (XmlLoader.ComponentNamespace.NamespaceName + "Text");
        });
    }
}
