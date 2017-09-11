using System;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using NServiceBus.Logging;

class Program
{
    public static async Task Main()
    {
        Console.Title = "Samples.Logging.MsLoggingCustom";

        #region MsLoggingInCode
        using (var loggerFactory = new LoggerFactory())
        {
            loggerFactory.AddConsole(Microsoft.Extensions.Logging.LogLevel.Information);
            var logFactory = LogManager.Use<MicrosoftLogFactory>();
            logFactory.UseMsFactory(loggerFactory);

            var endpointConfiguration = new EndpointConfiguration("Samples.Logging.MsLoggingCustom");
            await ConfigureAndRunEndpoint(endpointConfiguration);
        }
        #endregion
    }

    static async Task ConfigureAndRunEndpoint(EndpointConfiguration endpointConfiguration)
    {
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