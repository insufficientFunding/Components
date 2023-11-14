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
public sealed partial class ComponentService : IComponentService
{
    private readonly HashSet<IComponentDescription> _descriptions = new HashSet<IComponentDescription> ();
    private readonly HashSet<IPositionalComponent> _components = new HashSet<IPositionalComponent> ();

    private readonly ILogger<ComponentService> _logger;

    public ComponentService (ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ComponentService> ();
    }
}
