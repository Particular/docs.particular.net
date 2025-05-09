using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MyRequestHandler :
    IHandleMessages<MyRequest>
{
    static ILog log = LogManager.GetLogger<MyRequestHandler>();

    public Task Handle(MyRequest message, IMessageHandlerContext context)
    {
        log.Info($"Received Message1: {message.Property}");
        var MyResponse = new MyResponse
        {
            Property = "Hello from Endpoint2"
        };
        return context.Reply(MyResponse);
    }
}