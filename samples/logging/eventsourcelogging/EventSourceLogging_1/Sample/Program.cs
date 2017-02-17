using System;
using System.Threading.Tasks;
using NServiceBus;
using System.Diagnostics.Tracing;
using NServiceBus.EventSourceLogging;
using NServiceBus.Logging;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
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
            try
            {
                await SendMessage(endpointInstance)
                    .ConfigureAwait(false);
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            finally
            {
                await endpointInstance.Stop()
                    .ConfigureAwait(false);
            }
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
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
    }
}