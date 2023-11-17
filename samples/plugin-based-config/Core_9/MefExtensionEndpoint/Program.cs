using System;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    #region MefStartup

    static async Task Main()
    {
        Console.Title = "Samples.MefExtensionEndpoint";

        var containerConfiguration = new ContainerConfiguration();

        var currentDirectory = new DirectoryInfo(AppContext.BaseDirectory);
        var targetFramework = currentDirectory.Name;
        var configuration = currentDirectory.Parent.Name;

        var solutionDirectory = currentDirectory.Parent.Parent.Parent.Parent.FullName;
        var extensionDirectory = Path.Combine(solutionDirectory, "MefExtensions", "bin", configuration, targetFramework);

        var assemblies = Directory.EnumerateFiles(extensionDirectory, "*.dll", SearchOption.AllDirectories)
            .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath);

        containerConfiguration.WithAssemblies(assemblies);
        var compositionHost = containerConfiguration.CreateContainer();

        var endpointConfiguration = new EndpointConfiguration("Samples.MefExtensionEndpoint");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());
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
