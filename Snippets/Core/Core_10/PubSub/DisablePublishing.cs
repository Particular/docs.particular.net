namespace Core.PubSub;

using NServiceBus;
using Transports;

class DisablePublishing
{
    void DisablePublishingConfiguration(EndpointConfiguration endpointConfiguration)
    {
        #region DisablePublishing
        var transportConfiguration = endpointConfiguration.UseTransport(new TransportDefinition());
        transportConfiguration.DisablePublishing();
        #endregion
    }
}