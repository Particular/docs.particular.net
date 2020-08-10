namespace Core8.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Pipeline;
    using NServiceBus.Routing;

    #region CustomStageConnector
    public class CustomStageConnector :
        StageConnector<IOutgoingLogicalMessageContext, IOutgoingPhysicalMessageContext>
    {
        public override Task Invoke(IOutgoingLogicalMessageContext context, Func<IOutgoingPhysicalMessageContext, Task> stage)
        {
            // Finalize the work in the current stage

            byte[] body = { };
            RoutingStrategy[] routingStrategies = { };

            // Start the next stage
            return stage(this.CreateOutgoingPhysicalMessageContext(body, routingStrategies, context));
        }
    }

    public class FeatureReplacingExistingStage :
        Feature
    {
        internal FeatureReplacingExistingStage()
        {
            EnableByDefault();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            var pipeline = context.Pipeline;
            pipeline.Replace("NServiceBus.SerializeMessageConnector", new CustomStageConnector());
        }
    }
    #endregion
}