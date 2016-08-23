namespace Core5.Mutators.Transport
{
    using NServiceBus;
    using NServiceBus.MessageMutator;

    #region IMutateIncomingTransportMessages
    public class MutateIncomingTransportMessages :
        IMutateIncomingTransportMessages
    {
        public void MutateIncoming(TransportMessage transportMessage)
        {
            // the bytes containing the incoming messages.
            var bytes = transportMessage.Body;

            // optionally replace the Body
            transportMessage.Body = ServiceThatChangesBody.Mutate(transportMessage.Body);

            // the incoming headers
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
