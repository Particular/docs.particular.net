using System;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using NServiceBus;

static class Program
{

    #region MefStartup

    static void Main()
    {
        Console.Title = "Samples.MefExtensionEndpoint";

        var containerConfiguration = new ContainerConfiguration();
        var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var assemblies = Directory.EnumerateFiles(location, "*.dll")
            .Select(Assembly.LoadFrom);
        containerConfiguration.WithAssemblies(assemblies);
        var compositionHost = containerConfiguration.CreateContainer();

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MefExtensionEndpoint");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        RunCustomizeConfiguration(compositionHost, busConfiguration);
        RunBeforeEndpointStart(compositionHost);
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            RunAfterEndpointStart(compositionHost, bus);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            RunBeforeEndpointStop(compositionHost, bus);
        }
        RunAfterEndpointStop(compositionHost);
    }

    static void RunBeforeEndpointStart(CompositionHost compositionHost)
    {
        compositionHost.ExecuteExports<IRunBeforeEndpointStart>(_ => _.Run());
    }

    // Other injection points excluded, but follow the same pattern as above

    #endregion

    static void RunCustomizeConfiguration(CompositionHost compositionHost, BusConfiguration busConfiguration)
    {
        compositionHost.ExecuteExports<ICustomizeConfiguration>(_ => _.Run(busConfiguration));
    }

    static void RunAfterEndpointStop(CompositionHost compositionHost)
    {
        compositionHost.ExecuteExports<IRunAfterEndpointStop>(_ => _.Run());
    }

    static void RunBeforeEndpointStop(CompositionHost compositionHost, IBus bus)
    {
        compositionHost.ExecuteExports<IRunBeforeEndpointStop>(_ => _.Run(bus));
    }

    static void RunAfterEndpointStart(CompositionHost compositionHost, IBus bus)
    {
        compositionHost.ExecuteExports<IRunAfterEndpointStart>(_ => _.Run(bus));
    }
}