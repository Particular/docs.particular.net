using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class BlobCreatedHandler(ILogger<BlobCreated> logger) :
    IHandleMessages<BlobCreated>
{
    public Task Handle(BlobCreated message, IMessageHandlerContext context)
    {
        logger.LogInformation("Blob {Url} created!", message.Url);
        return Task.CompletedTask;
    }
}
