namespace Core5.Mutators.Transport
{
    using System.Collections.Generic;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NServiceBus.Unicast.Messages;

    #region IMutateOutgoingTransportMessages
    public class MutateOutgoingTransportMessages : IMutateOutgoingTransportMessages
    {
        public void MutateOutgoing(LogicalMessage logicalMessage, TransportMessage transportMessage)
        {
            // the outgoing message instance
            object instance = logicalMessage.Instance;

            // the bytes containing the serialized outgoing messages.
            byte[] bytes = transportMessage.Body;

            // optionally replace the Body. 
            // this can be done using either the information from the logicalMessage or transportMessage
            transportMessage.Body = ServiceThatChangesBody.Mutate(logicalMessage.Instance);


            // the outgoing headers
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
