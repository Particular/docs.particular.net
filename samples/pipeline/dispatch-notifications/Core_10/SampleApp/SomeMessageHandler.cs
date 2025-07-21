using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

class SomeMessageHandler (ILogger<SomeMessageHandler> logger):
    IHandleMessages<SomeMessage>
{
  
    public Task Handle(SomeMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Got SomeMessage");
        return Task.CompletedTask;
    }
}