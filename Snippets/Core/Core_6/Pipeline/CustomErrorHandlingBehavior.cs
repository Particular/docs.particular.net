namespace Snippets6.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;
    using NServiceBus.Pipeline;

    #region ErrorHandlingBehavior
    class CustomErrorHandlingBehavior : Behavior<ITransportReceiveContext>
    {
        public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
        {
            await next().ConfigureAwait(false);
        }
    }
    #endregion

    class CustomErrorHandlingBehaviorForDeserializationFailures : Behavior<ITransportReceiveContext>
    {
        ILog log = LogManager.GetLogger<CustomErrorHandlingBehaviorForDeserializationFailures>();

        public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
        {
            #region DeserializationCustomization
            try
            {
                await next().ConfigureAwait(false);
            }
            catch (MessageDeserializationException deserializationException)
            {
                // Handle any custom processing that needs to occur when a serialization failure occurs.
                log.Error("Message deserialization failed", deserializationException);
                throw;
            }
            #endregion
        }
    }

    class CustomErrorHandlingBehaviorForAllFailures : Behavior<ITransportReceiveContext>
    {
        ILog log = LogManager.GetLogger<CustomErrorHandlingBehaviorForAllFailures>();

        public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
        {
            #region AllErrorsCustomization
            try
            {
                await next().ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                // Handle any custom processing that need to occur when a message always fails.
                log.Error("Message processing failed", exception);
                throw;
            }
            #endregion
        }
    }

    class CustomErrorHandlingBehaviorRollbackOnFailures : Behavior<ITransportReceiveContext>
    {
        public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
        {
            #region RollbackMessage
            try
            {
                await next().ConfigureAwait(false);
            }
            catch (Exception)
            {
                // Handle any custom processing that need to occur when a message always fails.
                // if you want to rollback the receive operation instead of mark as processed:
                context.AbortReceiveOperation();
            }
            #endregion
        }
    }

    #region RegisterCustomErrorHandlingBehavior
    class NewMessageProcessingPipelineStep : RegisterStep
    {
        public NewMessageProcessingPipelineStep()
            : base("CustomErrorHandlingBehaviour", typeof(CustomErrorHandlingBehavior), "Adds custom error behavior to pipeline")
        {
            InsertAfter("MoveFaultsToErrorQueue");
            InsertBeforeIfExists("FirstLevelRetries");
            InsertBeforeIfExists("SecondLevelRetries");
        }
    }
    #endregion
}
