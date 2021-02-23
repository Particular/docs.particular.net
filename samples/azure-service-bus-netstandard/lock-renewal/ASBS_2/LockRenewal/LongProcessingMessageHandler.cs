using System.Threading.Tasks;
using LockRenewal;
using NServiceBus;
using NServiceBus.Logging;

public class LongProcessingMessageHandler : IHandleMessages<LongProcessingMessage>
{
    static ILog log = LogManager.GetLogger<LongProcessingMessageHandler>();

    #region handler-processing

    public async Task Handle(LongProcessingMessage message, IMessageHandlerContext context)
    {
        log.Info($"--- Received a message with processing duration of {message.ProcessingDuration}");

        await Task.Delay(message.ProcessingDuration).ConfigureAwait(false);

        log.Info("--- Processing completed");
    }

    #endregion
}