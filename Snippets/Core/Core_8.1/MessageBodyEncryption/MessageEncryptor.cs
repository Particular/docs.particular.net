namespace Core8.MessageBodyEncryption
{
    using System.Linq;
    using System.Threading.Tasks;
    using NServiceBus.MessageMutator;

    #region MessageBodyEncryptor

    public class MessageEncryptor :
        IMutateIncomingTransportMessages,
        IMutateOutgoingTransportMessages
    {
        public Task MutateIncoming(MutateIncomingTransportMessageContext context)
        {
            context.Body = context.Body.ToArray().Reverse().ToArray();
            return Task.CompletedTask;
        }

        public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
        {
            context.OutgoingBody = context.OutgoingBody.ToArray().Reverse().ToArray();
            return Task.CompletedTask;
        }
    }

    #endregion
}
