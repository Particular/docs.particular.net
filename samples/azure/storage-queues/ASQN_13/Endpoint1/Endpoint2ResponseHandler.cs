using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using Shared;

public sealed class Endpoint2ResponseHandler(ILogger<Endpoint2ResponseHandler> logger)
    : IHandleMessages<MyResponse>
{
    public Task Handle(MyResponse message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received MyResponse: {Property}", message.Property);
        return Task.CompletedTask;
    }
}