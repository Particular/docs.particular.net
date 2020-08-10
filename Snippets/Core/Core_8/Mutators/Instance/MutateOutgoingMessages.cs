namespace Core8.Mutators.Instance
{
    using System.Threading.Tasks;
    using NServiceBus.MessageMutator;

    #region IMutateOutgoingMessages
    public class MutateOutgoingMessages :
        IMutateOutgoingMessages
    {
        public Task MutateOutgoing(MutateOutgoingMessageContext context)
        {
            // the outgoing headers
            var outgoingHeaders = context.OutgoingHeaders;

            if (context.TryGetIncomingMessage(out var incomingMessage))
            {
                // do something with the incoming message
            }

            if (context.TryGetIncomingHeaders(out var incomingHeaders))
            {
                // do something with the incoming headers
            }

            // the outgoing message
            // optionally replace the message instance by setting context.OutgoingMessage
            var outgoingMessage = context.OutgoingMessage;

            return Task.CompletedTask;
        }
    }
    #endregion
}
