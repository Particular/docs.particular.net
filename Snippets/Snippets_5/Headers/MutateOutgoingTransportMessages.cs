namespace Snippets5.Headers
{
    using System.Collections.Generic;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NServiceBus.Unicast.Messages;

    #region header-outgoing-mutator
    public class MutateOutgoingTransportMessages : IMutateOutgoingTransportMessages
    {
        public void MutateOutgoing(LogicalMessage logicalMessage, TransportMessage transportMessage)
        {
            Dictionary<string, string> headers = transportMessage.Headers;
            headers["MyCustomHeader"] = "My custom value";
        }
    }
    #endregion
}
