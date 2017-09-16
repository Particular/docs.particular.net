using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static async Task Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        Console.Title = "Samples.StartupShutdown";
        LogManager.Use<DefaultFactory>().Level(LogLevel.Error);
        #region Program
        Logger.WriteLine("Starting configuration");
        var endpointConfiguration = new EndpointConfiguration("Samples.StartupShutdown");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.EnableFeature<MyFeature>();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        Logger.WriteLine("Calling Endpoint.Start");
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        // simulate some activity
        await Task.Delay(500)
            .ConfigureAwait(false);

        Logger.WriteLine("Endpoint is processing messages");
        Logger.WriteLine("Calling IEndpointInstance.Stop");
        await endpointInstance.Stop()
            .ConfigureAwait(false);
        Logger.WriteLine("Finished");
        #endregion
        Console.WriteLine($"Logged information to {Logger.OutputFilePath}");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}