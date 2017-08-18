using System;
using System.Threading.Tasks;
using Common.Logging;
using NServiceBus;
// ReSharper disable RedundantNameQualifier

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Logging.CommonLogging";

        #region ConfigureLogging

        Common.Logging.LogManager.Adapter = new ConsoleLoggerFactoryAdapter
        {
            Level = LogLevel.Info
        };

        NServiceBus.Logging.LogManager.Use<CommonLoggingFactory>();

        var endpointConfiguration = new EndpointConfiguration("Samples.Logging.CommonLogging");

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