using System.Threading.Tasks;
using NServiceBus;
using System.Diagnostics.Tracing;
using NServiceBus.EventSourceLogging;
using NServiceBus.Logging;

class Usage
{

    static async Task AsyncMain()
    {
        #region ConfigureLogging

        using (var listener = new CustomEventListener())
        using (var eventSourceLogger = new SimpleSourceLogger())
        {
            listener.EnableEvents(eventSourceLogger, EventLevel.Informational);
            var loggingFactory = LogManager.Use<EventSourceLoggingFactory>();
            loggingFactory.WithLogger(eventSourceLogger);

            var endpointConfiguration = new EndpointConfiguration("EndpointName");
            // other Endpoint Configuration

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            try
            {
                // either a blocking action or split the instantiation
                // and disposal in to the server startup and shutdown
            }
            finally
            {
                await endpointInstance.Stop()
                    .ConfigureAwait(false);
            }
        }

        #endregion
    }

}
