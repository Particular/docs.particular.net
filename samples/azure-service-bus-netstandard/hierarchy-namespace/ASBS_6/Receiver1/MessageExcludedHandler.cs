using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class MessageHandler (ILogger<MessageHandler> logger):
    IHandleMessages<ExcludedMessage>, IHandleMessages<RegularMessage>
{

    public Task Handle(ExcludedMessage excludedMessage, IMessageHandlerContext context)
    {
        logger.LogInformation("Received ExcludedMessage: {Property}", excludedMessage.Property);

        return Task.CompletedTask;
    }

    public Task Handle(RegularMessage regularMessage, IMessageHandlerContext context)
    {
        logger.LogInformation("Received RegularMessage: {Property}", regularMessage.Property);

        return Task.CompletedTask;
    }
}