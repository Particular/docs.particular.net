using NServiceBus;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

#region Handler

public class CommandMessageHandler(ILogger<CommandMessageHandler> logger) :
    IHandleMessages<Command>
{
    public Task Handle(Command message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Hello from CommandMessageHandler! Received Command with Id: {message.Id}");

        return Task.CompletedTask;
    }
}

#endregion