using Autofac;
using Serialization.Converters;
using System.Reflection;
using Module = Autofac.Module;
namespace Serialization;

public class SerializerModule : Module
{
    protected override void Load (ContainerBuilder builder)
    {
        // Container
        ILifetimeScope lifetimeScope = null!;
        builder.Register (_ => lifetimeScope).AsSelf ().SingleInstance ();
        builder.RegisterBuildCallback (x => lifetimeScope = x);

        // Services
        builder.RegisterType<ConverterService> ().As<IConverterService> ().SingleInstance ();

        Console.WriteLine ("!hi");
    }
}
