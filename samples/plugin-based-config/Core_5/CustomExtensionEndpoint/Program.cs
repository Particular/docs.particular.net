using System;
using NServiceBus;
using NServiceBus.Logging;

static class Program
{
    #region CustomStartup

    static void Main()
    {
        Console.Title = "Samples.CustomExtensionEndpoint";
        LogManager.Use<DefaultFactory>().Level(LogLevel.Info);
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.CustomExtensionEndpoint");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        RunCustomizeConfiguration(busConfiguration);
        RunBeforeEndpointStart();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            RunAfterEndpointStart(bus);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            RunBeforeEndpointStop(bus);
        }
        RunAfterEndpointStop();
    }

    static void RunBeforeEndpointStart()
    {
        Resolver.Execute<IRunBeforeEndpointStart>(_ => _.Run());
    }

    // Other injection points excluded, but follow the same pattern as above

    #endregion

    static void RunCustomizeConfiguration(BusConfiguration busConfiguration)
    {
        Resolver.Execute<ICustomizeConfiguration>(_ => _.Run(busConfiguration));
    }

    static void RunAfterEndpointStop()
    {
        Resolver.Execute<IRunAfterEndpointStop>(_ => _.Run());
    }

    static void RunBeforeEndpointStop(IBus bus)
    {
        Resolver.Execute<IRunBeforeEndpointStop>(_ => _.Run(bus));
    }

    static void RunAfterEndpointStart(IBus bus)
    {
        Resolver.Execute<IRunAfterEndpointStart>(_ => _.Run(bus));
    }
}