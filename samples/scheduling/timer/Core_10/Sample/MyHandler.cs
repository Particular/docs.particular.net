using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
public class MyHandler(ILogger<MyHandler> logger) :
    IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Hello from MyHandler");
        return Task.CompletedTask;
    }
}