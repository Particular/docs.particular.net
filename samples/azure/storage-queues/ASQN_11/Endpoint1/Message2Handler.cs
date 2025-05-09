using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MyResponseHandler :
    IHandleMessages<MyResponse>
{
    static ILog log = LogManager.GetLogger<MyResponseHandler>();

    public Task Handle(MyResponse message, IMessageHandlerContext context)
    {
        log.Info($"Received Message2: {message.Property}");
        return Task.CompletedTask;
    }
}