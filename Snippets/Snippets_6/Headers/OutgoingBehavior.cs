namespace Snippets6.Headers
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.OutgoingPipeline;
    using NServiceBus.Pipeline;
    using NServiceBus.TransportDispatch;

    #region header-outgoing-behavior
    public class OutgoingBehavior : Behavior<OutgoingPhysicalMessageContext>
    {
        public override async Task Invoke(OutgoingPhysicalMessageContext context, Func<Task> next)
        {
            context.SetHeader("MyCustomHeader", "My custom value");
            await next();
        }
    }
    #endregion
}