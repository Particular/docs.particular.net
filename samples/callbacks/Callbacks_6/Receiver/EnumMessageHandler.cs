using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region EnumMessageHandler

public class EnumMessageHandler(ILogger<EnumMessageHandler> logger) :
    IHandleMessages<EnumMessage>
{
    public Task Handle(EnumMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Message received, Returning");
        return context.Reply(Status.OK);
    }
}

#endregion