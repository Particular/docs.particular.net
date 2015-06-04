namespace Snippets6.Headers
{
    using NServiceBus;

    #region header-outgoing-mutator
    public class MutateOutgoingPhysicalContext : IMutateOutgoingPhysicalContext
    {
        public void MutateOutgoing(OutgoingPhysicalMutatorContext context)
        {
            context.SetHeader("MyCustomHeader", "My custom value");
        }
    }
    #endregion
}
