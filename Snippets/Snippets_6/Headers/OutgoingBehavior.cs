namespace Snippets6.Headers
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.OutgoingPipeline;
    using NServiceBus.Pipeline;

    #region header-outgoing-behavior
    public class OutgoingBehavior : Behavior<OutgoingPhysicalMessageContext>
    {
        public override async Task Invoke(OutgoingPhysicalMessageContext context, Func<Task> next)
        {
            context.Headers["MyCustomHeader"] = "My custom value";
            await next();
        }
    }
    #endregion
}