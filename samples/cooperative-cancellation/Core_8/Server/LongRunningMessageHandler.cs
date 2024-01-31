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

        while (true)
        {
            log.Info("Handler still running. Press any key to forcibly stop the endpoint.");
            await Task.Delay(2000, context.CancellationToken);
        }
    }
    #endregion
}