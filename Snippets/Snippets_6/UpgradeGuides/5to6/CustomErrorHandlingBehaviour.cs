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
                // To mark the message as processed DO NOT rethrow the message.
                // Rethrow to move the message to the error queue.
                throw;
                // To rollback the receive operation instead of mark as processed:
                // context.AbortReceiveOperation();
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
            // Only invoke this behavior if FLR fails to handle the message
            InsertBeforeIfExists("FirstLevelRetries");
            // To handle the message before it is moved to error queue, insert after SLR.
            // To handle the message before it is handled by SLR, insert it before SLR.
            InsertBeforeIfExists("SecondLevelRetries");
        }
    }
    #endregion

    class RegisterCustomErrorHandling : INeedInitialization {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-registercustomerrorhandling
            endpointConfiguration.Pipeline.Register<NewMessageProcessingPipelineStep>();
            #endregion
        }
    }
}
