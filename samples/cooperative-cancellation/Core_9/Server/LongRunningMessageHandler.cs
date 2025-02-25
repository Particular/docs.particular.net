using NServiceBus;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public class LongRunningMessageHandler :
    IHandleMessages<LongRunningMessage>
{
    private static readonly ILogger<LongRunningMessageHandler> logger =
      LoggerFactory.Create(builder =>
      {
          builder.AddConsole();
      }).CreateLogger<LongRunningMessageHandler>();

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