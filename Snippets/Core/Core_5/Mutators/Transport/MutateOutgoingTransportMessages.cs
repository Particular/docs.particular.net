namespace Core5.Mutators.Transport
{
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NServiceBus.Unicast.Messages;

    #region IMutateOutgoingTransportMessages
    public class MutateOutgoingTransportMessages :
        IMutateOutgoingTransportMessages
    {
        public void MutateOutgoing(LogicalMessage logicalMessage, TransportMessage transportMessage)
        {
            // the outgoing message instance
            var instance = logicalMessage.Instance;

            // the bytes containing the serialized outgoing messages.
            var bytes = transportMessage.Body;

            // optionally replace the Body.
            // this can be done using either the information from the logicalMessage or transportMessage
            transportMessage.Body = ServiceThatChangesBody.Mutate(logicalMessage.Instance);


            // the outgoing headers
            var headers = transportMessage.Headers;

            // optional manipulate headers

            // add a header
            headers.Add("MyHeaderKey1", "MyHeaderValue");

            // remove a header
            headers.Remove("MyHeaderKey2");
        }
    }
    #endregion
}
