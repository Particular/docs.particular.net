namespace Core3.Headers
{
    using System.Collections.Generic;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NServiceBus.Unicast.Transport;

    #region header-incoming-mutator
    public class MutateIncomingTransportMessages : IMutateIncomingTransportMessages
    {
        public void MutateIncoming(TransportMessage transportMessage)
        {
            Dictionary<string, string> headers = transportMessage.Headers;
            string nsbVersion = headers[Headers.NServiceBusVersion];
            string customHeader = headers["MyCustomHeader"];
        }
    }
    #endregion
}
