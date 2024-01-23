using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

public class LongRunningMessageHandler :
    IHandleMessages<LongRunningMessage>
{
    static ILog log = LogManager.GetLogger<LongRunningMessageHandler>();

    #region LongRunningMessageHandler
    public async Task Handle(LongRunningMessage message, IMessageHandlerContext context)
    {
        log.Info($"Received message {message.DataId}. Entering loop.");

        // The try-catch block is only for the purpose of demonstrating the sample.
        try
        {
            while (true)
            {
                log.Info("Press any key to cancel the 3 second delay loop and stop the endpoint.");
                await Task.Delay(3000, context.CancellationToken);
            }
        }
        catch (OperationCanceledException) when (context.CancellationToken.IsCancellationRequested)
        {
            log.Info("LongRunningMessageHandler exiting.");
            // re-throw the exception to propagate the cancellation to the caller of the current method
            throw;
        }
    }
    #endregion
}