namespace Core4.Headers
{
    using NServiceBus;
    using NServiceBus.MessageMutator;

    #region header-outgoing-mutator
    public class MutateOutgoingTransportMessages : IMutateOutgoingTransportMessages
    {
        public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
        {
            var headers = transportMessage.Headers;
            headers["MyCustomHeader"] = "My custom value";
        }
    }
    #endregion
}
