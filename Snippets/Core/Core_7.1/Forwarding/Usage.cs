namespace Core7.Forwarding
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region ForwardingWithCode
            endpointConfiguration.ForwardReceivedMessagesTo("destinationQueue@machine");
            #endregion
        }
    }
}

