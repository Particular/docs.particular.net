namespace Snippets6.Mutators.Instance
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus.MessageMutator;

    #region IMutateIncomingMessages
    public class MutateIncomingMessages : IMutateIncomingMessages
    {
        public Task MutateIncoming(MutateIncomingMessageContext context)
        {
            // the incoming headers
            IDictionary<string, string> headers = context.Headers;

            // the incoming message
            // optionally replace the message instance by setting context.Message
            object message = context.Message;

            
            return Task.FromResult(0);
        }
    }
    #endregion
}
