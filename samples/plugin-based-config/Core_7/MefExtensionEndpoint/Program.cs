using System;
using System.ComponentModel.Composition.Hosting;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }
    #region MefStartup


    static async Task AsyncMain()
    {
        var catalog = new AggregateCatalog();
        catalog.Catalogs.Add(new DirectoryCatalog("."));

        var compositionContainer = new CompositionContainer(catalog);

        Console.Title = "Samples.MefExtensionEndpoint";
        var endpointConfiguration = new EndpointConfiguration("Samples.MefExtensionEndpoint");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
        await RunCustomizeConfiguration(compositionContainer, endpointConfiguration)
            .ConfigureAwait(false);
        await RunBeforeEndpointStart(compositionContainer)
            .ConfigureAwait(false);
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await RunAfterEndpointStart(compositionContainer, endpointInstance)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await RunBeforeEndpointStop(compositionContainer, endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
        await RunAfterEndpointStop(compositionContainer)
            .ConfigureAwait(false);
    }

    static Task RunBeforeEndpointStart(CompositionContainer compositionContainer)
    {
        return compositionContainer.ExecuteExports<IRunBeforeEndpointStart>(_ => _.Run());
    }

    // Other injection points excluded, but follow the same pattern as above

    #endregion

    static Task RunCustomizeConfiguration(CompositionContainer compositionContainer, EndpointConfiguration endpointConfiguration)
    {
        return compositionContainer.ExecuteExports<ICustomizeConfiguration>(_ => _.Run(endpointConfiguration));
    }

    static Task RunAfterEndpointStop(CompositionContainer compositionContainer)
    {
        return compositionContainer.ExecuteExports<IRunAfterEndpointStop>(_ => _.Run());
    }

    static Task RunBeforeEndpointStop(CompositionContainer compositionContainer, IEndpointInstance endpoint)
    {
        return compositionContainer.ExecuteExports<IRunBeforeEndpointStop>(_ => _.Run(endpoint));
    }

    static Task RunAfterEndpointStart(CompositionContainer compositionContainer, IEndpointInstance endpoint)
    {
       return compositionContainer.ExecuteExports<IRunAfterEndpointStart>(_ => _.Run(endpoint));
    }
}