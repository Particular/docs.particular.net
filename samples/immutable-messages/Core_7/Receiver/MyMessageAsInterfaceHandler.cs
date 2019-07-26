using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using UsingInterfaces.Messages;

public class MyMessageAsInterfaceHandler :
    IHandleMessages<IMyMessage>
{
    static ILog log = LogManager.GetLogger<MyMessageAsInterfaceHandler>();

    public Task Handle(IMyMessage message, IMessageHandlerContext context)
    {
        log.Info($"IMyMessage (as interface) received from server with data: {message.Data}");
        return Task.CompletedTask;
    }
}