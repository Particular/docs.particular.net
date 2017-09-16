using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{

    static async Task Main()
    {
        Console.Title = "Samples.Logging.CustomFactory";
        #region ConfigureLogging

        var loggerDefinition = LogManager.Use<ConsoleLoggerDefinition>();

        // optionally set the log level in code or read from app.config
        loggerDefinition.Level(LogLevel.Info);

        // logging configuration should occur prior to endpoint configuration
        var endpointConfiguration = new EndpointConfiguration("Samples.Logging.CustomFactory");

        #endregion
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
