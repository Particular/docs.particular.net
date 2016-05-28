namespace Core3.Headers
{
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NServiceBus.Unicast.Transport;

    #region header-incoming-mutator
    public class MutateIncomingTransportMessages : IMutateIncomingTransportMessages
    {
        public void MutateIncoming(TransportMessage transportMessage)
        {
            var headers = transportMessage.Headers;
            var nsbVersion = headers[Headers.NServiceBusVersion];
            var customHeader = headers["MyCustomHeader"];
        }
    }
    #endregion
}
