using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.StartupShutdown";
        LogManager.Use<DefaultFactory>().Level(LogLevel.Error);
        #region Program
        Logger.WriteLine("Starting configuration");
        var endpointConfiguration = new EndpointConfiguration("Samples.StartupShutdown");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.EnableFeature<MyFeature>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        Logger.WriteLine("Calling Endpoint.Start");
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            // simulate some activity
            await Task.Delay(500)
                .ConfigureAwait(false);

            Logger.WriteLine("Endpoint is processing messages");
        }
        finally
        {
            Logger.WriteLine("Calling IEndpointInstance.Stop");
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
        Logger.WriteLine("Finished");
        #endregion
        Console.WriteLine($"Logged information to {Logger.OutputFilePath}");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}