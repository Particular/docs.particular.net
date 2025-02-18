using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

public class EventOneHandler(ILogger<EventOneHandler> logger) : IHandleMessages<EventOne>
{
    public Task Handle(EventOne message, IMessageHandlerContext context)
    {
        logger.LogInformation("{PublishedOnUtc} Received EventOne with content: {Content}", message.PublishedOnUtc,message.Content);
        return Task.CompletedTask;
    }
}