namespace Core6.Mutators.Transport
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus.MessageMutator;

    #region IMutateOutgoingTransportMessages
    public class MutateOutgoingTransportMessages : IMutateOutgoingTransportMessages
    {
        public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
        {
            object incomingMessage;
            if (context.TryGetIncomingMessage(out incomingMessage))
            {
                // do something with the incoming message
            }

            IReadOnlyDictionary<string, string> incomingHeaders;
            if (context.TryGetIncomingHeaders(out incomingHeaders))
            {
                // do something with the incoming headers
            }

            // the outgoing message
            object outgoingMessage = context.OutgoingMessage;
            
            // the bytes containing the serialized outgoing messages.
            byte[] bytes = context.OutgoingBody;

            // optionally replace the Body. 
            // this can be done using any information from the context
            context.OutgoingBody = ServiceThatChangesBody.Mutate(context.OutgoingMessage);


            // the outgoing headers
            IDictionary<string, string> headers = context.OutgoingHeaders;

            // optional manipulate headers

            // add a header
            headers.Add("MyHeaderKey1", "MyHeaderValue");

            // remove a header
            headers.Remove("MyHeaderKey2");

            return Task.FromResult(0);
        }
    }
    #endregion
}
