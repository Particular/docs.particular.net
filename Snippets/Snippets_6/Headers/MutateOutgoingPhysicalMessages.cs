namespace Snippets6.Headers
{
    using NServiceBus.MessageMutator;

    #region header-outgoing-mutator
    public class MutateOutgoingPhysicalMessages : IMutateOutgoingTransportMessages
    {
        public void MutateOutgoing(MutateOutgoingTransportMessagesContext context)
        {
            context.SetHeader("MyCustomHeader", "My custom value"); throw new System.NotImplementedException();
        }
    }
    #endregion
}
