namespace Core6.Headers
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.Pipeline;

    #region header-outgoing-behavior
    public class OutgoingBehavior : Behavior<IOutgoingPhysicalMessageContext>
    {
        public override Task Invoke(IOutgoingPhysicalMessageContext context, Func<Task> next)
        {
            context.Headers["MyCustomHeader"] = "My custom value";
            return next();
        }
    }
    #endregion
}