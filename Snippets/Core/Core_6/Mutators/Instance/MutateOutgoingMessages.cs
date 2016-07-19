namespace Core6.Mutators.Instance
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus.MessageMutator;

    #region IMutateOutgoingMessages
    public class MutateOutgoingMessages :
        IMutateOutgoingMessages
    {
        public Task MutateOutgoing(MutateOutgoingMessageContext context)
        {
            // the outgoing headers
            IDictionary<string, string> outgoingHeaders = context.OutgoingHeaders;

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
            // optionally replace the message instance by setting context.OutgoingMessage
            var outgoingMessage = context.OutgoingMessage;

            return Task.FromResult(0);
        }
    }
    #endregion
}
