using System;
using System.ComponentModel.Composition.Hosting;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

static class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }
    #region MefStartup


    static async Task AsyncMain()
    {
        AggregateCatalog catalog = new AggregateCatalog();
        catalog.Catalogs.Add(new DirectoryCatalog("."));

        CompositionContainer compositionContainer = new CompositionContainer(catalog);

        Console.Title = "Samples.MefExtensionEndpoint";
        LogManager.Use<DefaultFactory>().Level(LogLevel.Info);
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.MefExtensionEndpoint");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        await RunCustomizeConfiguration(compositionContainer, endpointConfiguration);
        await RunBeforeEndpointStart(compositionContainer);
        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        await RunAfterEndpointStart(compositionContainer, endpoint);
        try
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await RunBeforeEndpointStop(compositionContainer, endpoint);
            await endpoint.Stop();
        }
        await RunAfterEndpointStop(compositionContainer);
    }

    static async Task RunBeforeEndpointStart(CompositionContainer compositionContainer)
    {
        await compositionContainer.ExecuteExports<IRunBeforeEndpointStart>(_ => _.Run());
    }

    // Other injection points excluded, but follow the same pattern as above

    #endregion

    static async Task RunCustomizeConfiguration(CompositionContainer compositionContainer, EndpointConfiguration endpointConfiguration)
    {
        await compositionContainer.ExecuteExports<ICustomizeConfiguration>(_ => _.Run(endpointConfiguration));
    }

    static async Task RunAfterEndpointStop(CompositionContainer compositionContainer)
    {
        await compositionContainer.ExecuteExports<IRunAfterEndpointStop>(_ => _.Run());
    }

    static async Task RunBeforeEndpointStop(CompositionContainer compositionContainer, IEndpointInstance endpoint)
    {
        await compositionContainer.ExecuteExports<IRunBeforeEndpointStop>(_ => _.Run(endpoint));
    }

    static async Task RunAfterEndpointStart(CompositionContainer compositionContainer, IEndpointInstance endpoint)
    {
       await compositionContainer.ExecuteExports<IRunAfterEndpointStart>(_ => _.Run(endpoint));
    }
}