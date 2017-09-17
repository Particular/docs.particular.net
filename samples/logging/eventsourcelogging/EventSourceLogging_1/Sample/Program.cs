using System;
using System.Threading.Tasks;
using NServiceBus;
using System.Diagnostics.Tracing;
using NServiceBus.EventSourceLogging;
using NServiceBus.Logging;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Logging.SimpleSourceLogger";

        #region ConfigureLogging

        using (var listener = new CustomEventListener())
        using (var eventSourceLogger = new SimpleSourceLogger())
        {
            listener.EnableEvents(eventSourceLogger, EventLevel.Informational);
            var loggingFactory = LogManager.Use<EventSourceLoggingFactory>();
            loggingFactory.WithLogger(eventSourceLogger);

            var endpointConfiguration = new EndpointConfiguration("Samples.Logging.SimpleSourceLogger");
            ApplyBasicConfig(endpointConfiguration);

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            await SendMessage(endpointInstance)
                .ConfigureAwait(false);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }

        #endregion
    }

    static Task SendMessage(IEndpointInstance endpointInstance)
    {
        var myMessage = new MyMessage();
        return endpointInstance.SendLocal(myMessage);
    }

    static void ApplyBasicConfig(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
    }
}