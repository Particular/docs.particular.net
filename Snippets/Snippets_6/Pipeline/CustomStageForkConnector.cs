namespace Snippets6.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;
    using NServiceBus.Transports;

    #region CustomStageForkConnector
    public class CustomStageForkConnector : StageForkConnector<ITransportReceiveContext, IIncomingPhysicalMessageContext, IBatchDispatchContext>
    {
        public override async Task Invoke(ITransportReceiveContext context, Func<IIncomingPhysicalMessageContext, Task> stage, Func<IBatchDispatchContext, Task> fork)
        {
            // Finalize the work in the current stage

            IncomingMessage message = null;

            // Start the next stage
            await stage(new IncomingPhysicalMessageContext(message, context)).ConfigureAwait(false);

            TransportOperation[] operations = { };

            // Fork into new pipeline
            await fork(new BatchDispatchContext(operations, context)).ConfigureAwait(false);
        }
    }
    #endregion
}