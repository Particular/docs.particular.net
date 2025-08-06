using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using Shared;

public sealed class MyRequestResponseHandler(ILogger<MyRequestResponseHandler> logger)
    : IHandleMessages<MyRequest>
{
    public Task Handle(MyRequest message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received MyRequest: {Property}", message.Property);
        var myResponse = new MyResponse("Hello from Endpoint2");
        return context.Reply(myResponse);
    }
}