using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region ObjectMessageHandler

public class ObjectMessageHandler :
    IHandleMessages<ObjectMessage>
{
    private readonly ILogger<ObjectMessageHandler> logger;

    public ObjectMessageHandler(ILogger<ObjectMessageHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(ObjectMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Message received, Returning");
        var objectResponseMessage = new ObjectResponseMessage
        {
            Property = "PropertyValue"
        };
        return context.Reply(objectResponseMessage);
    }
}

#endregion