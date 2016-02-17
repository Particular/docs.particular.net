
namespace Snippets6.Forwarding
{
    using NServiceBus;
    public class Usage
    {
        public Usage()
        {
            #region ForwardingWithCode
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.ForwardReceivedMessagesTo("destinationQueue@machine");
            #endregion
        }
    }
}

