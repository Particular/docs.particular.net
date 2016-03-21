using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region ObjectMessageHandler

public class ObjectMessageHandler : IHandleMessages<ObjectMessage>
{
    static ILog log = LogManager.GetLogger<ObjectMessageHandler>();

    public async Task Handle(ObjectMessage message, IMessageHandlerContext context)
    {
        log.Info("Message received, Returning");
        await context.Reply(new ObjectResponseMessage
        {
            Property = "PropertyValue"
        });
    }
}

#endregion