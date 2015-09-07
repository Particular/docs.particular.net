namespace Snippets6.Headers
{
    using NServiceBus.MessageMutator;

    #region header-outgoing-mutator

    public class MutateOutgoingTransportMessages : IMutateOutgoingTransportMessages
    {
        public void MutateOutgoing(MutateOutgoingTransportMessageContext context)
        {
            context.OutgoingHeaders["MyCustomHeader"] = "My custom value";
        }
    }

    #endregion
}