namespace Snippets6.Headers
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.OutgoingPipeline;
    using NServiceBus.TransportDispatch;

    #region header-outgoing-behavior
    public class OutgoingBehavior : PhysicalOutgoingContextStageBehavior
    {
        public override async Task Invoke(Context context, Func<Task> next)
        {
            context.SetHeader("MyCustomHeader", "My custom value");
            await next();
        }
    }
    #endregion
}