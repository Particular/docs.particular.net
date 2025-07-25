using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region IntMessageHandler

public class IntMessageHandler(ILogger<IntMessageHandler> logger) :
    IHandleMessages<IntMessage>
{
    public Task Handle(IntMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Message received, Returning");
        return context.Reply(10);
    }
}

#endregion