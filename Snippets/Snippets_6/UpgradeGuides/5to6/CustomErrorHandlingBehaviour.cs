namespace Snippets6.UpgradeGuides._5to6
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Pipeline;

    #region 5to6-customerrorhandlingbehaviour
    class CustomErrorHandlingBehaviour : Behavior<ITransportReceiveContext>
    {
        public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
        {
            try
            {
                await next().ConfigureAwait(false);
            }
            catch (MessageDeserializationException)
            {
                Console.WriteLine("CustomFaultManager - MessageDeserializationException");
            }
            catch (Exception)
            {
                Console.WriteLine("CustomFaultManager - ProcessingAlwaysFailsForMessage");
                // do not rethrow the message if you want to mark the message as processed.
                // rethrow to move the message to the error queue.
                throw;
                // if you want to rollback the receive operation instead of mark as processed:
                //context.AbortReceiveOperation();
            }
        }
    }
    #endregion

    #region 5to6-newmessageprocessingpipelinestep
    class NewMessageProcessingPipelineStep : RegisterStep
    {
        public NewMessageProcessingPipelineStep()
            : base("CustomErrorHandlingBehaviour", typeof(CustomErrorHandlingBehaviour), "Adds custom error behavior to pipeline")
        {
            InsertAfter("MoveFaultsToErrorQueue");
            // only invoke this behavior if FLR fails to handle the message
            InsertBeforeIfExists("FirstLevelRetries");
            // if you want to handle the message before it is moved to error queue, insert after SLR.
            // if you want to handle the message before it is handled by SLR, insert it before SLR.
            InsertBeforeIfExists("SecondLevelRetries");
        }
    }
    #endregion

    #region 5to6-registercustomerrorhandling
    public void Customize(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.Pipeline.Register<NewMessageProcessingPipelineStep>();
    }
    #endregion
}
