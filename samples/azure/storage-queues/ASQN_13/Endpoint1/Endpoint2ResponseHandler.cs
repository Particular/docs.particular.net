using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public sealed class Endpoint2ResponseHandler(ILogger<Endpoint2ResponseHandler> logger)
    : IHandleMessages<Endpoint2.MyResponse>
{
    public Task Handle(Endpoint2.MyResponse message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received Message2: {Property}", message.Property);
        return Task.CompletedTask;
    }
}