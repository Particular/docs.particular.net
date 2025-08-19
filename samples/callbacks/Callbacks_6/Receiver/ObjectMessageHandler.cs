using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region ObjectMessageHandler

public class ObjectMessageHandler(ILogger<ObjectMessageHandler> logger) :
    IHandleMessages<ObjectMessage>
{
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