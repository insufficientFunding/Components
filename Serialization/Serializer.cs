using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serialization.Converters;
using Serialization.Logging;
namespace Serialization;

public sealed partial class Serializer : ISerializer
{
    private readonly ILogger<Serializer> _logger;
    private readonly IConverterService _converterService;

    private IContainer _container;

    public Serializer ()
    {
        _container = SetupContainer ();

        _logger = _container.Resolve<ILogger<Serializer>> ();
        _converterService = _container.Resolve<IConverterService> ();
    }

    private IContainer SetupContainer ()
    {
        // Add Microsoft.Extensions.Logging to the DI container.
        var services = new ServiceCollection ();
        services.AddLogging (x => x.SetupLogging (true, false));

        // Build the DI container.
        var builder = new ContainerBuilder ();

        builder.Populate (services);

        builder.RegisterModule<SerializerModule> ();

        return builder.Build ();
    }

    public void RegisterConverter<T> () where T : class
    {
        if (typeof (T).IsAssignableTo (typeof (ISerializationConverter)))
            _converterService.Register (typeof (T));
    }
}
