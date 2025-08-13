using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using Subscriber1.Contracts;

class Consumer1EventHandler(ILogger<Consumer1EventHandler> logger) : IHandleMessages<Consumer1Contract>
{
    public Task Handle(Consumer1Contract message, IMessageHandlerContext context)
    {
        logger.LogInformation(message.Consumer1Property);
        return Task.CompletedTask;
    }
}