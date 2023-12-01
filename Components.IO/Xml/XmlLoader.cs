using Autofac;
using Components.IO.Xml.Features;
using Components.IO.Xml.Interfaces;
using Components.IO.Xml.Logging;
using Components.IO.Xml.Parsers;
using Components.IO.Xml.Parsers.ComponentPoints;
using Components.IO.Xml.Parsers.Conditions;
using Components.IO.Xml.Readers;
using Components.IO.Xml.Readers.RenderCommands;
using Components.IO.Xml.Sections;
using Components.Render.TypeDescription.TypeDescription;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;
namespace Components.IO.Xml;

/// <summary>
///     A class that loads component descriptions from XML files.
/// </summary>
public class XmlLoader : IDisposable
{
    // The namespace that all component XML files must use.
    public static readonly XNamespace ComponentNamespace = "https://schemas.alaskasvingen.com/components/schema.xsd";

    // The container is lazy so that it is only built when it is needed.
    private readonly Lazy<IContainer> _container;

    // A dictionary of features that can be enabled/disabled.
    private readonly Dictionary<string, Action<ContainerBuilder>> _features = new Dictionary<string, Action<ContainerBuilder>> ();
    
    public XmlLoader ()
    {
        ContainerBuilder serviceBuilder = new ContainerBuilder ();

        serviceBuilder.RegisterType<ConditionParser> ().As<IConditionParser> ().InstancePerLifetimeScope ();
        serviceBuilder.RegisterType<ComponentPointParser> ().As<IComponentPointParser> ().Named<IComponentPointParser> ("default").InstancePerLifetimeScope ();
        serviceBuilder.RegisterType<AutoRotateOptionsReader> ().As<IAutoRotateOptionsReader> ().InstancePerDependency ();
        serviceBuilder.RegisterType<AttributeParser> ().As<IAttributeParser> ().InstancePerLifetimeScope ();

        serviceBuilder.RegisterType<DeclarationSectionReader> ().Named<IXmlSectionReader> (ComponentNamespace.NamespaceName + "Declaration").InstancePerDependency ();

        serviceBuilder.RegisterType<RenderSectionReader> ().Named<IXmlSectionReader> (ComponentNamespace.NamespaceName + "Render").InstancePerLifetimeScope ();
        serviceBuilder.RegisterType<TextCommandReader> ().Named<IRenderCommandReader> (ComponentNamespace.NamespaceName + "Text");

        _container = new Lazy<IContainer> (() => serviceBuilder.Build ());
    }

    /// <summary>
    ///     Registers a feature that can be enabled/disabled.
    /// </summary>
    /// <param name="key">The key of the feature.</param>
    /// <param name="configure">A delegate that configures the feature.</param>
    public void RegisterFeature (string key, Action<ContainerBuilder> configure)
    {
        _features.Add (key, configure);
    }

    public bool Load (Stream stream, out ComponentDescription description)
    {
        return Load (stream, new NullXmlLoadLogger (), out description);
    }

    public bool Load (Stream stream, ILogger logger, out ComponentDescription description)
    {
        return Load (stream, new XmlLoadLogger (logger, (stream as FileStream)?.Name!), out description);
    }

    internal bool Load (Stream stream, IXmlLoadLogger logger, out ComponentDescription description)
    {
        description = new ComponentDescription ();

        ErrorCheckingLogger errorCheckingLogger = new ErrorCheckingLogger (logger);
        SectionRegistry sectionRegistry = new SectionRegistry ();

        try
        {
            XElement root = XElement.Load (stream, LoadOptions.SetLineInfo);
            XElement? declaration = root.Element (ComponentNamespace + "Declaration");
            if (declaration == null)
                return logger.LogErrorReturnFalse (declaration, $"Declaration element not found in component XML file '{(stream as FileStream)?.Name}'");

            FeatureSwitcher featureSwitcher = new FeatureSwitcher ();

            IXmlSectionReader declarationReader = _container.Value.ResolveNamed<IXmlSectionReader> (ComponentNamespace.NamespaceName + declaration.Name.LocalName,
                                                                                                    new TypedParameter (typeof (IXmlLoadLogger), errorCheckingLogger),
                                                                                                    new TypedParameter (typeof (FeatureSwitcher), featureSwitcher));
            declarationReader.ReadSection (declaration, description);

            ComponentDescription descriptionInstance = description;
            ILifetimeScope scope = _container.Value.BeginLifetimeScope (configure =>
            {
                configure.RegisterInstance (errorCheckingLogger).As<IXmlLoadLogger> ();
                configure.RegisterInstance (sectionRegistry).As<ISectionRegistry> ();
                configure.RegisterInstance (descriptionInstance);
                configure.RegisterInstance (featureSwitcher).As<IFeatureSwitcher> ();

                foreach (KeyValuePair<string, Action<ContainerBuilder>> feature in _features)
                {
                    if (featureSwitcher.IsFeatureEnabled (feature.Key, out XElement? featureSourceElement))
                    {
                        logger.Log (LogLevel.Information, featureSourceElement!, $"Feature enabled: {feature.Key}");
                        feature.Value (configure);
                    }
                }
            });

            try
            {
                foreach (XElement element in root.Elements ().Except (new [] { declaration }))
                {
                    IXmlSectionReader? sectionReader = scope.ResolveOptionalNamed<IXmlSectionReader> (ComponentNamespace.NamespaceName + element.Name.LocalName);
                    sectionReader?.ReadSection (element, description);
                }

                return !errorCheckingLogger.HasErrors;
            }
            finally
            {
                scope.Dispose ();
            }
        }
        catch (Exception exception)
        {
            logger.Log (LogLevel.Error, new FileRange (1, 1, 1, 2), exception.Message, exception);
            return false;
        }
    }
    
    public void Dispose ()
    {
        if (_container.IsValueCreated)
            _container.Value.Dispose ();
    }
}
