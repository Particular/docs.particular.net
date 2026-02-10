using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace Receiver2;

public class EventOneHandler(ILogger<EventOneHandler> logger) : IHandleMessages<SampleEvent>
{
    public Task Handle(SampleEvent message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received an event with content: {Content}", message.Property);
        return Task.CompletedTask;
    }
}