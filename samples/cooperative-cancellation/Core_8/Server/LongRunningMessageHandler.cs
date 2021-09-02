using NServiceBus;
using System.Threading.Tasks;
using NServiceBus.Logging;

public class LongRunningMessageHandler :
    IHandleMessages<LongRunningMessage>
{
    static ILog log = LogManager.GetLogger<LongRunningMessageHandler>();

    #region LongRunningMessageHandler
    public async Task Handle(LongRunningMessage message, IMessageHandlerContext context)
    {
        log.Info($"Received message {message.DataId}. Entering loop.");

        do
        {
            await Task.Delay(2000, context.CancellationToken);
            log.Info("Press any key to cancel the loop and stop the endpoint.");
        } while (!context.CancellationToken.IsCancellationRequested);
    }
    #endregion
}