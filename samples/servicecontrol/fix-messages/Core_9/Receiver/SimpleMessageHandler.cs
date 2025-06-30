using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class SimpleMessageHandler(ILogger<SimpleMessageHandler> logger) :
    IHandleMessages<SimpleMessage>
{
    public Task Handle(SimpleMessage message, IMessageHandlerContext context)
    {
        #region ReceiverHandler

        logger.LogInformation("Received message with Id = {Id}.", message.Id);
        if (message.Id.Any(char.IsLower))
        {
            throw new Exception("Lowercase characters are not allowed in message Id.");
        }
        logger.LogInformation("Successfully processed message with Id = {Id}.", message.Id);
        return Task.CompletedTask;

        #endregion
    }
}