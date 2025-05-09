using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public sealed class MyRequestResponseHandler(ILogger<MyRequestResponseHandler> logger)
    : IHandleMessages<Endpoint2.MyRequest>
{
    public Task Handle(Endpoint2.MyRequest message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received MyRequest: {Property}", message.Property);
        var myResponse = new Endpoint2.MyResponse("Hello from Endpoint2");
        return context.Reply(myResponse);
    }
}