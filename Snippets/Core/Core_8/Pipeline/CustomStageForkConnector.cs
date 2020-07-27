namespace Core8.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Pipeline;
    using NServiceBus.Transport;

    #region CustomStageForkConnector

    public class CustomStageForkConnector :
        StageForkConnector<ITransportReceiveContext,
            IIncomingPhysicalMessageContext,
            IBatchDispatchContext>
    {
        public override async Task Invoke(ITransportReceiveContext context, Func<IIncomingPhysicalMessageContext, Task> stage, Func<IBatchDispatchContext, Task> fork)
        {
            // Finalize the work in the current stage
            var physicalMessageContext = this.CreateIncomingPhysicalMessageContext(context.Message, context);

            // Start the next stage
            await stage(physicalMessageContext)
                .ConfigureAwait(false);

            TransportOperation[] operations =
            {
            };
            var batchDispatchContext = this.CreateBatchDispatchContext(operations, physicalMessageContext);

            // Fork into new pipeline
            await fork(batchDispatchContext)
                .ConfigureAwait(false);
        }
    }

    public class FeatureReplacingExistingStageForkConnector :
        Feature
    {
        internal FeatureReplacingExistingStageForkConnector()
        {
            EnableByDefault();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            var pipeline = context.Pipeline;
            pipeline.Replace("TransportReceiveToPhysicalMessageProcessingConnector", new CustomStageForkConnector());
        }
    }

    #endregion
}