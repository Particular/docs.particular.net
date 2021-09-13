using System.Threading.Tasks;
using NServiceBus;
using Serilog;

public class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILogger log = Log.ForContext<MyHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Information("Hello from MyHandler");
        return Task.CompletedTask;
    }
}