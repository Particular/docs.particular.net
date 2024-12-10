namespace NServiceBus
{
    #region message-forwarding-configuration
    public static class MessageForwardingConfigurationExtensions
    {
        public static void ForwardMessagesAfterProcessingTo(this EndpointConfiguration endpointConfiguration, string forwardingAddress)
        {
            endpointConfiguration.Pipeline.Register(
                new ForwardProcessedMessagesBehavior(forwardingAddress), 
                "Forwards a copy of each processed message to a forwarding address"
            );
        }
    }
    #endregion
}
