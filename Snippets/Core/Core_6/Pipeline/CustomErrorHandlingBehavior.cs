namespace Snippets6.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;
    using NServiceBus.Pipeline;

    #region ErrorHandlingBehavior

    class CustomErrorHandlingBehavior :
        Behavior<ITransportReceiveContext>
    {
        public override Task Invoke(ITransportReceiveContext context, Func<Task> next)
        {
            return next();
        }
    }

    #endregion

    class CustomErrorHandlingBehaviorForDeserializationFailures :
        Behavior<ITransportReceiveContext>
    {
        static ILog log = LogManager.GetLogger<CustomErrorHandlingBehaviorForDeserializationFailures>();

        public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
        {
            #region DeserializationCustomization

            try
            {
                await next()
                    .ConfigureAwait(false);
            }
            catch (MessageDeserializationException deserializationException)
            {
                // Custom processing that needs to occur when a serialization failure occurs.
                log.Error("Message deserialization failed", deserializationException);
                throw;
            }

            #endregion
        }
    }

    class CustomErrorHandlingBehaviorForAllFailures :
        Behavior<ITransportReceiveContext>
    {
        static ILog log = LogManager.GetLogger<CustomErrorHandlingBehaviorForAllFailures>();

        public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
        {
            #region AllErrorsCustomization

            try
            {
                await next()
                    .ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                // Custom processing that need to occur when a message always fails.
                log.Error("Message processing failed", exception);
                throw;
            }

            #endregion
        }
    }

    class CustomErrorHandlingBehaviorRollbackOnFailures :
        Behavior<ITransportReceiveContext>
    {
        public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
        {
            #region RollbackMessage

            try
            {
                await next()
                    .ConfigureAwait(false);
            }
            catch (Exception)
            {
                // Custom processing that need to occur when a message always fails.
                // To rollback the receive operation instead of mark as processed:
                context.AbortReceiveOperation();
            }

            #endregion
        }
    }

    #region RegisterCustomErrorHandlingBehavior

    class NewMessageProcessingPipelineStep :
        RegisterStep
    {
        public NewMessageProcessingPipelineStep()
            : base(
                stepId: "CustomErrorHandlingBehavior",
                behavior: typeof(CustomErrorHandlingBehavior),
                description: "Adds custom error behavior to pipeline")
        {
            // Within a stage it is sometimes necessary to configure a specific
            // step order. This can be achieved by invoking on of the following methods:
            //  - InsertAfter,
            //  - InsertAfterIfExists,
            //  - InsertBefore,
            //  - InsertBeforeIfExists
        }
    }

    #endregion
}
