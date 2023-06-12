namespace Core8.Headers
{
    using System.Threading.Tasks;
    using NServiceBus.MessageMutator;

    #region header-outgoing-mutator

    public class MutateOutgoingTransportMessages :
        IMutateOutgoingTransportMessages
    {
        public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
        {
            context.OutgoingHeaders["MyCustomHeader"] = "My custom value";
            return Task.CompletedTask;
        }
    }

    #endregion
}