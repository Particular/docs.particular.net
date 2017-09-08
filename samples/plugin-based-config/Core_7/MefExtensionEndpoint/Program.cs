using System;
using System.Composition.Hosting;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{

    static void Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        AsyncMain().GetAwaiter().GetResult();
    }

    #region MefStartup

    static async Task AsyncMain()
    {
        Console.Title = "Samples.MefExtensionEndpoint";

        var containerConfiguration = new ContainerConfiguration();
        var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var assemblies = Directory.EnumerateFiles(location, "*.dll")
            .Select(Assembly.LoadFrom);
        containerConfiguration.WithAssemblies(assemblies);
        var compositionHost = containerConfiguration.CreateContainer();

        var endpointConfiguration = new EndpointConfiguration("Samples.MefExtensionEndpoint");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
        await RunCustomizeConfiguration(compositionHost, endpointConfiguration)
            .ConfigureAwait(false);
        await RunBeforeEndpointStart(compositionHost)
            .ConfigureAwait(false);
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await RunAfterEndpointStart(compositionHost, endpointInstance)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await RunBeforeEndpointStop(compositionHost, endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
        await RunAfterEndpointStop(compositionHost)
            .ConfigureAwait(false);
    }

    static Task RunBeforeEndpointStart(CompositionHost compositionHost)
    {
        return compositionHost.ExecuteExports<IRunBeforeEndpointStart>(_ => _.Run());
    }

    // Other injection points excluded, but follow the same pattern as above

    #endregion

    static Task RunCustomizeConfiguration(CompositionHost compositionHost, EndpointConfiguration endpointConfiguration)
    {
        return compositionHost.ExecuteExports<ICustomizeConfiguration>(_ => _.Run(endpointConfiguration));
    }

    static Task RunAfterEndpointStop(CompositionHost compositionHost)
    {
        return compositionHost.ExecuteExports<IRunAfterEndpointStop>(_ => _.Run());
    }

    static Task RunBeforeEndpointStop(CompositionHost compositionHost, IEndpointInstance endpoint)
    {
        return compositionHost.ExecuteExports<IRunBeforeEndpointStop>(_ => _.Run(endpoint));
    }

    static Task RunAfterEndpointStart(CompositionHost compositionHost, IEndpointInstance endpoint)
    {
       return compositionHost.ExecuteExports<IRunAfterEndpointStart>(_ => _.Run(endpoint));
    }
}