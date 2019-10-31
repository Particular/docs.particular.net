using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region ObjectMessageHandler

public class ObjectMessageHandler :
    IHandleMessages<ObjectMessage>
{
    static ILog log = LogManager.GetLogger<ObjectMessageHandler>();

    public async Task Handle(ObjectMessage message, IMessageHandlerContext context)
    {
        log.Info("Message received, Returning");
        var objectResponseMessage = new ObjectResponseMessage
        {
            Property = "PropertyValue"
        };
        await Task.Delay(3000);
        await context.Reply(objectResponseMessage);
    }
}

#endregion