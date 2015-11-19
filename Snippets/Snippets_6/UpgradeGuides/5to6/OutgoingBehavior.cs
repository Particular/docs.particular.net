namespace Snippets6.UpgradeGuides._5to6
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.OutgoingPipeline;
    using NServiceBus.TransportDispatch;

    #region 5to6header-outgoing-behavior
    public class OutgoingBehavior : Behavior<OutgoingLogicalMessageContext>
    {
        public override async Task Invoke(OutgoingLogicalMessageContext context, Func<Task> next)
        {
            context.SetHeader("MyCustomHeader", "My custom value");
            await next();
        }
    }
    #endregion
}
