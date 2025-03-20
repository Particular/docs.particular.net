using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region EnumMessageHandler

public class EnumMessageHandler :
    IHandleMessages<EnumMessage>
{
    private readonly ILogger<EnumMessageHandler> logger;

    public EnumMessageHandler(ILogger<EnumMessageHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(EnumMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Message received, Returning");
        return context.Reply(Status.OK);
    }
}

#endregion