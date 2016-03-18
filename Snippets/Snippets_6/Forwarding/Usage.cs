namespace Snippets6.Forwarding
{
    using NServiceBus;
    public class Usage
    {
        public Usage(EndpointConfiguration endpointConfiguration)
        {
            #region ForwardingWithCode
            endpointConfiguration.ForwardReceivedMessagesTo("destinationQueue@machine");
            #endregion
        }
    }
}

