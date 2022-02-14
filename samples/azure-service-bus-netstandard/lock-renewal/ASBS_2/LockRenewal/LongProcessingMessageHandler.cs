using System;
using System.Threading.Tasks;
using LockRenewal;
using NServiceBus;
using NServiceBus.Logging;

public class LongProcessingMessageHandler : IHandleMessages<LongProcessingMessage>
{
    static readonly ILog log = LogManager.GetLogger<LongProcessingMessageHandler>();

    #region handler-processing

    public async Task Handle(LongProcessingMessage message, IMessageHandlerContext context)
    {
        //var duration = message.ProcessingDuration;
        var duration = Program.ProcessingDuration;
        log.Info($"--- Received a message with processing duration of {duration}, delay until {DateTime.Now+duration}");

        await Task.Delay(duration).ConfigureAwait(false);

        log.Info("--- Processing completed");
    }

    #endregion
}