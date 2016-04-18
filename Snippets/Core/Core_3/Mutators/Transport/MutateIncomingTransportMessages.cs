namespace Core3.Mutators.Transport
{
    using System.Collections.Generic;
    using NServiceBus.MessageMutator;
    using NServiceBus.Unicast.Transport;

    #region IMutateIncomingTransportMessages
    public class MutateIncomingTransportMessages : IMutateIncomingTransportMessages
    {
        public void MutateIncoming(TransportMessage transportMessage)
        {
            // the bytes containing the incoming messages.
            byte[] bytes = transportMessage.Body;

            // optionally replace the Body
            transportMessage.Body = ServiceThatChangesBody.Mutate(transportMessage.Body);


            // the incoming headers
            Dictionary<string, string> headers = transportMessage.Headers;

            // optional manipulate headers

            // add a header
            headers.Add("MyHeaderKey1", "MyHeaderValue");

            // remove a header
            headers.Remove("MyHeaderKey2");
        }
    }
    #endregion
}
