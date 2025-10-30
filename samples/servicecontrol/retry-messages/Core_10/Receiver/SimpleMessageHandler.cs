using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class SimpleMessageHandler(ILogger<SimpleMessageHandler> logger) :
    IHandleMessages<SimpleMessage>
{
    public static bool FaultMode { get; set; } = true;

    public Task Handle(SimpleMessage message, IMessageHandlerContext context)
    {
        #region ReceiverHandler

        logger.LogInformation("Received message.");
        if (FaultMode)
        {
            throw new Exception("Simulated error.");
        }
        logger.LogInformation("Successfully processed message.");
        return Task.CompletedTask;

        #endregion
    }
}