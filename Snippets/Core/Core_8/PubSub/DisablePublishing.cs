namespace Core8.PublishSubscribe
{
    using NServiceBus;

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
}