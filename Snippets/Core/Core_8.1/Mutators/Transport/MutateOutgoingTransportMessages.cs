namespace Core8.Mutators.Transport
{
    using System.Threading.Tasks;
    using NServiceBus.MessageMutator;

    #region IMutateOutgoingTransportMessages
    public class MutateOutgoingTransportMessages :
        IMutateOutgoingTransportMessages
    {
        public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
        {
            if (context.TryGetIncomingMessage(out var incomingMessage))
            {
                // do something with the incoming message
            }

            if (context.TryGetIncomingHeaders(out var incomingHeaders))
            {
                // do something with the incoming headers
            }

            // the outgoing message
            var outgoingMessage = context.OutgoingMessage;

            // the bytes containing the serialized outgoing messages.
            var bytes = context.OutgoingBody;

            // optionally replace the Body.
            // this can be done using any information from the context
            context.OutgoingBody = ServiceThatChangesBody.Mutate(context.OutgoingMessage);

            // the outgoing headers
            var headers = context.OutgoingHeaders;

            // optional manipulate headers

            // add a header
            headers.Add("MyHeaderKey1", "MyHeaderValue");

            // remove a header
            headers.Remove("MyHeaderKey2");

            return Task.CompletedTask;
        }
    }
    #endregion
}
