namespace Core3.Headers
{
    using System.Collections.Generic;
    using NServiceBus.MessageMutator;
    using NServiceBus.Unicast.Transport;

    #region header-outgoing-mutator
    public class MutateOutgoingTransportMessages : IMutateOutgoingTransportMessages
    {
        public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
        {
            Dictionary<string, string> headers = transportMessage.Headers;
            headers["MyCustomHeader"] = "My custom value";
        }
    }
    #endregion
}
