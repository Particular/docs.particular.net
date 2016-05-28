using System;
using System.ComponentModel.Composition.Hosting;
using NServiceBus;
using NServiceBus.Logging;

static class Program
{

    #region MefStartup

    static void Main()
    {
        var catalog = new AggregateCatalog();
        catalog.Catalogs.Add(new DirectoryCatalog("."));

        var compositionContainer = new CompositionContainer(catalog);

        Console.Title = "Samples.MefExtensionEndpoint";
        LogManager.Use<DefaultFactory>().Level(LogLevel.Info);
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MefExtensionEndpoint");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        RunCustomizeConfiguration(compositionContainer, busConfiguration);
        RunBeforeEndpointStart(compositionContainer);
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            RunAfterEndpointStart(compositionContainer, bus);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            RunBeforeEndpointStop(compositionContainer, bus);
        }
        RunAfterEndpointStop(compositionContainer);
    }

    static void RunBeforeEndpointStart(CompositionContainer compositionContainer)
    {
        compositionContainer.ExecuteExports<IRunBeforeEndpointStart>(_ => _.Run());
    }

    // Other injection points excluded, but follow the same pattern as above

    #endregion

    static void RunCustomizeConfiguration(CompositionContainer compositionContainer, BusConfiguration busConfiguration)
    {
        compositionContainer.ExecuteExports<ICustomizeConfiguration>(_ => _.Run(busConfiguration));
    }

    static void RunAfterEndpointStop(CompositionContainer compositionContainer)
    {
        compositionContainer.ExecuteExports<IRunAfterEndpointStop>(_ => _.Run());
    }

    static void RunBeforeEndpointStop(CompositionContainer compositionContainer, IBus bus)
    {
        compositionContainer.ExecuteExports<IRunBeforeEndpointStop>(_ => _.Run(bus));
    }

    static void RunAfterEndpointStart(CompositionContainer compositionContainer, IBus bus)
    {
        compositionContainer.ExecuteExports<IRunAfterEndpointStart>(_ => _.Run(bus));
    }
}