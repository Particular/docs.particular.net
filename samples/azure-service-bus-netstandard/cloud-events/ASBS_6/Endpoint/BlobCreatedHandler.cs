using System;
using System.Threading.Tasks;
using Azure.Messaging.EventGrid.SystemEvents;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class BlobCreatedHandler(ILogger<StorageBlobCreatedEventData> logger) :
    IHandleMessages<StorageBlobCreatedEventData>
{
    public Task Handle(StorageBlobCreatedEventData message, IMessageHandlerContext context)
    {
        logger.LogInformation("Blob {Url} created!", message.Url);
        return Task.CompletedTask;
    }
}
