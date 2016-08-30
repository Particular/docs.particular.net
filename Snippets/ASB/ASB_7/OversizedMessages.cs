using NServiceBus;

class OversizedMessages
{
    void ConfigureOversizedMessagesHandler(EndpointConfiguration endpointConfiguration)
    {
        #region asb-configure-oversized-messages-handler-config

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var messageSenders = transport.MessageSenders();
        messageSenders.OversizedBrokeredMessageHandler(new CustomOversizedBrokeredMessageHandler());

        #endregion
    }
}