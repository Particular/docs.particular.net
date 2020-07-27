namespace Core8.Mutators.Instance
{
    using System.Threading.Tasks;
    using NServiceBus.MessageMutator;

    #region IMutateIncomingMessages
    public class MutateIncomingMessages :
        IMutateIncomingMessages
    {
        public Task MutateIncoming(MutateIncomingMessageContext context)
        {
            // the incoming headers
            var headers = context.Headers;

            // the incoming message
            // optionally replace the message instance by setting context.Message
            var message = context.Message;

            return Task.CompletedTask;
        }
    }
    #endregion
}
