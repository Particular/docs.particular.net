namespace Snippets6.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.OutgoingPipeline;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.OutgoingPipeline;
    using NServiceBus.Routing;

    #region CustomStageConnector
    public class CustomStageConnector : StageConnector<IOutgoingLogicalMessageContext, IOutgoingPhysicalMessageContext>
    {
        public override async Task Invoke(IOutgoingLogicalMessageContext context, Func<IOutgoingPhysicalMessageContext, Task> stage)
        {
            // Finalize the work in the current stage

            byte[] body = { };
            RoutingStrategy[] routingStrategies = { };

            // Start the next stage
            await stage(new OutgoingPhysicalMessageContext(body, routingStrategies, context)).ConfigureAwait(false);
        }
    }
    #endregion
}