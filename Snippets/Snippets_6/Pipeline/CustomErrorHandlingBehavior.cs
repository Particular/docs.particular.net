namespace Snippets6.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
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

    class CustomErrorHandlingBehaviourForDeserializationFailures : Behavior<ITransportReceiveContext>
    {
        public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
        {
            #region DeserializationCustomization
            try
            {
                await next().ConfigureAwait(false);
            }
            catch (MessageDeserializationException)
            {
                // Handle any custom processing that need to occur when a serialization failure occurs.
                throw;
            }
            #endregion
        }
    }

    class CustomErrorHandlingBehaviourForAllFailures : Behavior<ITransportReceiveContext>
    {
        public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
        {
            #region AllErrorsCustomization
            try
            {
                await next().ConfigureAwait(false);
            }
            catch (Exception)
            {
                // Handle any custom processing that need to occur when a message always fails.
                throw;                
            }
            #endregion
        }
    }

    class CustomErrorHandlingBehaviourRollbackOnFailures : Behavior<ITransportReceiveContext>
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
