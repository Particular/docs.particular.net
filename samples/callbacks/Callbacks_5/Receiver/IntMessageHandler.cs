using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region IntMessageHandler

public class IntMessageHandler :
    IHandleMessages<IntMessage>
{

    private readonly ILogger<IntMessageHandler> logger;

    public IntMessageHandler(ILogger<IntMessageHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(IntMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Message received, Returning");
        return context.Reply(10);
    }
}

#endregion