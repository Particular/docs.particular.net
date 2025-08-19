using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

public class EventTwoHandler(ILogger<EventTwoHandler> logger) : IHandleMessages<EventTwo>
{
    public Task Handle(EventTwo message, IMessageHandlerContext context)
    {
        logger.LogInformation("{PublishedOnUtc} Received EventTwo with content: {Content}", message.PublishedOnUtc,message.Content);
        return Task.CompletedTask;
    }
}