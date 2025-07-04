using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
public class MyCommandHandler(ILogger<MyCommandHandler> logger) : IHandleMessages<MyCommand>
{
    public Task Handle(MyCommand commandMessage, IMessageHandlerContext context)
    {
        logger.LogInformation("Received {MessageType} with a payload of {PayloadLength} bytes.", nameof(MyCommand), commandMessage.Data?.Length ?? 0);
        return Task.CompletedTask;
    }
}