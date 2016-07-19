namespace Core4.Mutators.Transport
{
    using NServiceBus;
    using NServiceBus.MessageMutator;

    #region IMutateOutgoingTransportMessages
    public class MutateOutgoingTransportMessages :
        IMutateOutgoingTransportMessages
    {
        public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
        {
            // the bytes containing the serialized outgoing messages.
            var bytes = transportMessage.Body;

            // optionally replace the Body
            transportMessage.Body = ServiceThatChangesBody.Mutate(messages);

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
