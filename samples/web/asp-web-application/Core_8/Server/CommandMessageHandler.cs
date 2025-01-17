using NServiceBus;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

#region Handler

public class CommandMessageHandler(ILogger<CommandMessageHandler> logger) :
    IHandleMessages<Command>
{
    public Task Handle(Command message, IMessageHandlerContext context)
    {
        logger.LogInformation("Hello from CommandMessageHandler");

        return context.Reply(message.Id%2 == 0 ? ErrorCodes.Fail : ErrorCodes.None);
    }
}

#endregion