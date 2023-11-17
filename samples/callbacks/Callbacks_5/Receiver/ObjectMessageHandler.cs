using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region ObjectMessageHandler

public class ObjectMessageHandler :
    IHandleMessages<ObjectMessage>
{
    static ILog log = LogManager.GetLogger<ObjectMessageHandler>();

    public Task Handle(ObjectMessage message, IMessageHandlerContext context)
    {
        log.Info("Message received, Returning");
        var objectResponseMessage = new ObjectResponseMessage
        {
            Property = "PropertyValue"
        };
        return context.Reply(objectResponseMessage);
    }
}

#endregion