namespace Core;

using System.Threading.Tasks;
using NServiceBus;
using Transports;

class BestPracticesConfiguration
{
    void DisableFeature(EndpointConfiguration endpointConfiguration)
    {
        #region DisableBestPracticeEnforcementPerEndpoint

        var routing = endpointConfiguration.UseTransport(new TransportDefinition());
        routing.DoNotEnforceBestPractices();

        #endregion
    }

    async Task DisablePerMessage(IPipelineContext context)
    {
        #region DisableBestPracticeEnforcementPerMessage

        var options = new SendOptions();

        options.DoNotEnforceBestPractices();

        await context.Send(new MyEvent(), options);

        #endregion
    }

    class MyEvent
    {
    }
}