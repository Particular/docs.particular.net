namespace Snippets6.Headers
{
    using NServiceBus.MessageMutator;

    #region header-outgoing-mutator
    public class MutateOutgoingPhysicalMessages : IMutateOutgoingPhysicalMessages
    {
        public void MutateOutgoing(MutateOutgoingPhysicalMessageContext context)
        {
            context.SetHeader("MyCustomHeader", "My custom value");
        }
    }
    #endregion
}
