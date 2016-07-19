namespace Core6.Headers
{
    using System.Threading.Tasks;
    using NServiceBus.MessageMutator;

    #region header-outgoing-mutator

    public class MutateOutgoingTransportMessages :
        IMutateOutgoingTransportMessages
    {
        public async Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
        {
            context.OutgoingHeaders["MyCustomHeader"] = "My custom value";
        }
    }

    #endregion
}