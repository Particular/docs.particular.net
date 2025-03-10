using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region CommandMessageHandler
public class CommandMessageHandler(ILogger<CommandMessageHandler> logger) :
    IHandleMessages<Command>
{
    public Task Handle(Command message, IMessageHandlerContext context)
    {
       logger.LogInformation("Hello from CommandMessageHandler");

        Task reply;
        if (message.Id % 2 == 0)
        {
            logger.LogInformation("Returning Fail");
            reply = context.Reply(ErrorCodes.Fail);
        }
        else
        {
            logger.LogInformation("Returning None");
            reply = context.Reply(ErrorCodes.None);
        }
        return reply;
    }
}
#endregion
