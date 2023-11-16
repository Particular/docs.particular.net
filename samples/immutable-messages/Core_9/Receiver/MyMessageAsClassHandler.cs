using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using UsingClasses.Messages;

#region immutable-messages-as-class-handling
public class MyMessageAsClassHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyMessageAsClassHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info($"MyMessage (as class) received from server with data: {message.Data}");
        return Task.CompletedTask;
    }
}
#endregion