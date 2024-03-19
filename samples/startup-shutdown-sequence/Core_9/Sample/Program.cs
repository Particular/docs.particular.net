using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static async Task Main()
    {
        Console.Title = "StartupShutdown";
        LogManager.Use<DefaultFactory>().Level(LogLevel.Error);
        #region Program
        Logger.WriteLine("Starting configuration");
        var endpointConfiguration = new EndpointConfiguration("Samples.StartupShutdown");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.EnableFeature<MyFeature>();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        Logger.WriteLine("Calling Endpoint.Start");
        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        // simulate some activity
        await Task.Delay(500);

        Logger.WriteLine("Endpoint is processing messages");
        Logger.WriteLine("Calling IEndpointInstance.Stop");
        await endpointInstance.Stop();
        Logger.WriteLine("Finished");
        #endregion
        Console.WriteLine($"Logged information to {Logger.OutputFilePath}");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}
