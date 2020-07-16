namespace CustomTimeoutManager
{
    using System.Threading.Tasks;
    using NServiceBus.Extensibility;
    using NServiceBus.Logging;
    using NServiceBus.Raw;
    using NServiceBus.Routing;
    using NServiceBus.Transport;

    /// <summary>
    /// Do 5 FLR attempts and move the message to the back of the queue. There is no reason why message processing can fail in the timeout manager other
    /// than SQL server of MSMQ being not available so we can retry until we succeed.
    ///
    /// Exceptions are poison messages that don't contain required information. These, we move to the "poison" queue.
    /// </summary>
    internal class RetryForeverPolicy : IErrorHandlingPolicy
    {
        static ILog log = LogManager.GetLogger<RetryForeverPolicy>();

        public async Task<ErrorHandleResult> OnError(IErrorHandlingPolicyContext handlingContext, IDispatchMessages dispatcher)
        {
            if (handlingContext.Error.Exception is PoisonMessageException)
            {
                log.Info($"Message {handlingContext.Error.Message.MessageId} is considered poison and is going to be moved to the poison message queue.");
                await handlingContext.MoveToErrorQueue("poison").ConfigureAwait(false);
                return ErrorHandleResult.Handled;
            }

            if (handlingContext.Error.ImmediateProcessingFailures < 5)
            {
                log.Info($"Message {handlingContext.Error.Message.MessageId} failed and is going to be retried.");
                return ErrorHandleResult.RetryRequired;
            }

            log.Error($"Message {handlingContext.Error.Message.MessageId} failed FLR and will be moved to the back of the queue.");

            var message = new OutgoingMessage(handlingContext.Error.Message.MessageId, handlingContext.Error.Message.Headers, handlingContext.Error.Message.Body);
            var operation = new TransportOperation(message, new UnicastAddressTag(handlingContext.FailedQueue));

            await dispatcher.Dispatch(new TransportOperations(operation), handlingContext.Error.TransportTransaction, new ContextBag()).ConfigureAwait(false);

            return ErrorHandleResult.Handled;
        }
    }
}