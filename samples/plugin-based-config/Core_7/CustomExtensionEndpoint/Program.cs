using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static Program()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
    }

    #region CustomStartup

    static async Task Main()
    {
        Console.Title = "Samples.CustomExtensionEndpoint";
        var endpointConfiguration = new EndpointConfiguration("Samples.CustomExtensionEndpoint");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
        await RunCustomizeConfiguration(endpointConfiguration)
            .ConfigureAwait(false);
        await RunBeforeEndpointStart()
            .ConfigureAwait(false);
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await RunAfterEndpointStart(endpointInstance)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await RunBeforeEndpointStop( endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
        await RunAfterEndpointStop()
            .ConfigureAwait(false);
    }

    static Task RunBeforeEndpointStart()
    {
       return Resolver.Execute<IRunBeforeEndpointStart>(_ => _.Run());
    }

    // Other injection points excluded, but follow the same pattern as above

    #endregion

    static Task RunCustomizeConfiguration(EndpointConfiguration endpointConfiguration)
    {
        return Resolver.Execute<ICustomizeConfiguration>(_ => _.Run(endpointConfiguration));
    }

    static Task RunAfterEndpointStop()
    {
        return Resolver.Execute<IRunAfterEndpointStop>(_ => _.Run());
    }

    static Task RunBeforeEndpointStop(IEndpointInstance endpoint)
    {
       return Resolver.Execute<IRunBeforeEndpointStop>(_ => _.Run(endpoint));
    }

    static Task RunAfterEndpointStart(IEndpointInstance endpoint)
    {
        return Resolver.Execute<IRunAfterEndpointStart>(_ => _.Run(endpoint));
    }
}