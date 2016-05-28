namespace Core6.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Pipeline;
    using NServiceBus.Transports;

    #region CustomStageForkConnector
    public class CustomStageForkConnector : StageForkConnector<ITransportReceiveContext, IIncomingPhysicalMessageContext, IBatchDispatchContext>
    {
        public override async Task Invoke(ITransportReceiveContext context, Func<IIncomingPhysicalMessageContext, Task> stage, Func<IBatchDispatchContext, Task> fork)
        {
            // Finalize the work in the current stage
            var physicalMessageContext = this.CreateIncomingPhysicalMessageContext(context.Message, context);

            // Start the next stage
            await stage(physicalMessageContext)
                .ConfigureAwait(false);

            TransportOperation[] operations = { };
            var batchDispatchContext = this.CreateBatchDispatchContext(operations, physicalMessageContext);

            // Fork into new pipeline
            await fork(batchDispatchContext)
                .ConfigureAwait(false);
        }
    }
    #endregion
}