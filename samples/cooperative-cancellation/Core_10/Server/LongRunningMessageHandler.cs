using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class LongRunningMessageHandler(ILogger<LongRunningMessageHandler> logger) :
    IHandleMessages<LongRunningMessage>
{
    #region LongRunningMessageHandler
    public async Task Handle(LongRunningMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received message {MessageDataId}. Entering loop.", message.DataId);

        while (true)
        {
            logger.LogInformation("Handler still running. Press any key to forcibly stop the endpoint.");
            await Task.Delay(2000, context.CancellationToken);
        }
    }
    #endregion
}