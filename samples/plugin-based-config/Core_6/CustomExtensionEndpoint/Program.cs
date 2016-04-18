using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    #region CustomStartup

    static async Task AsyncMain()
    {
        Console.Title = "Samples.CustomExtensionEndpoint";
        LogManager.Use<DefaultFactory>().Level(LogLevel.Info);
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.CustomExtensionEndpoint");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        await RunCustomizeConfiguration( endpointConfiguration);
        await RunBeforeEndpointStart();
        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        await RunAfterEndpointStart(endpoint);
        try
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await RunBeforeEndpointStop( endpoint);
            await endpoint.Stop();
        }
        await RunAfterEndpointStop();
    }

    static async Task RunBeforeEndpointStart()
    {
       await Resolver.Execute<IRunBeforeEndpointStart>(_ => _.Run());
    }

    // Other injection points excluded, but follow the same pattern as above

    #endregion

    static async Task RunCustomizeConfiguration(EndpointConfiguration endpointConfiguration)
    {
        await Resolver.Execute<ICustomizeConfiguration>(_ => _.Run(endpointConfiguration));
    }

    static async Task RunAfterEndpointStop()
    {
        await Resolver.Execute<IRunAfterEndpointStop>(_ => _.Run());
    }

    static async Task RunBeforeEndpointStop(IEndpointInstance endpoint)
    {
       await  Resolver.Execute<IRunBeforeEndpointStop>(_ => _.Run(endpoint));
    }

    static async Task RunAfterEndpointStart(IEndpointInstance endpoint)
    {
        await Resolver.Execute<IRunAfterEndpointStart>(_ => _.Run(endpoint));
    }
}