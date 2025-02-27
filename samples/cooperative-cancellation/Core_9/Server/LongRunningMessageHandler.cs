using NServiceBus;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public class LongRunningMessageHandler :
    IHandleMessages<LongRunningMessage>
{
    private readonly ILogger<LongRunningMessageHandler> logger;

    public LongRunningMessageHandler(ILogger<LongRunningMessageHandler> logger)
    {
        this.logger = logger;
    }

    #region LongRunningMessageHandler
    public async Task Handle(LongRunningMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Received message {message.DataId}. Entering loop.");

        while (true)
        {
            logger.LogInformation("Handler still running. Press any key to forcibly stop the endpoint.");
            await Task.Delay(2000, context.CancellationToken);
        }
    }
    #endregion
}