namespace Core6.MessageIdentity
{
    using System.Threading.Tasks;
    using NServiceBus.MessageMutator;

    #region MessageId-Mutator
    public class MessageIdFromMutator : IMutateOutgoingMessages
    {
        public Task MutateOutgoing(MutateOutgoingMessageContext context)
        {
            // NOTE: Currently this doesn't work!
            context.OutgoingHeaders["NServiceBus.MessageId"] = GenerateIdForMessage(context.OutgoingMessage);

            return Task.FromResult(0);
        }

        string GenerateIdForMessage(object message)
        {
            throw new System.NotImplementedException();
        }
    }
    #endregion
}
