using Components.Base.Models;
using Components.Render.TypeDescription.TypeDescription;
using Microsoft.Extensions.Logging;
namespace Components.Services;

/// <inheritdoc cref="IComponentService"/>
/// <summary>
///     A service for reading and creating components.
/// </summary>
public sealed partial class ComponentService : IComponentService
{
    private readonly HashSet<ComponentDescription> _descriptions = new HashSet<ComponentDescription> ();
    private readonly HashSet<IPositionalComponent> _components = new HashSet<IPositionalComponent> ();

    private readonly ILogger<ComponentService> _logger;

    public ComponentService (ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ComponentService> ();
    }
}
