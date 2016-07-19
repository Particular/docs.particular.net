namespace Core3.Headers
{
    using NServiceBus.MessageMutator;
    using NServiceBus.Unicast.Transport;

    #region header-outgoing-mutator
    public class MutateOutgoingTransportMessages :
        IMutateOutgoingTransportMessages
    {
        public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
        {
            var headers = transportMessage.Headers;
            headers["MyCustomHeader"] = "My custom value";
        }
    }
    #endregion
}
