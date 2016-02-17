namespace Snippets6.Forwarding
{
    using NServiceBus;
    public class Usage
    {
        public Usage()
        {
            #region ForwardingWithCode
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.ForwardReceivedMessagesTo("destinationQueue@machine");
            #endregion
        }
    }
}

