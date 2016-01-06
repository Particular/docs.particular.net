namespace Snippets6.UpgradeGuides._5to6
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.OutgoingPipeline;

    #region 5to6header-outgoing-behavior
    public class OutgoingBehavior : Behavior<IOutgoingLogicalMessageContext>
    {
        public override async Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
        {
            context.Headers["MyCustomHeader"] = "My custom value";
            await next().ConfigureAwait(false);
        }
    }
    #endregion
}
